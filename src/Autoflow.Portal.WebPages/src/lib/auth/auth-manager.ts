import {
	InMemoryWebStorage,
	User,
	UserManager,
	WebStorageStateStore,
	type INavigator,
	type UserManagerSettings,
} from 'oidc-client-ts';
import type { ResponseData } from '$lib/types/interfaces';
import StatusCodes from '$lib/status-code/status-codes';
import ClientConfiguration from '$lib/configurations/client-configurations';
import { goto } from '$app/navigation';

type TokenResponseProps = {
	access_token: string;
	id_token?: string;
};

type MediaType = 'application/x-www-form-urlencoded' | 'application/json' | 'text/plain';
const getMediaType = (mediaType: MediaType) => mediaType;

const CONTENT_TYPE_HEADER_KEY = 'Content-Type';

class AuthManager extends UserManager {
	constructor(
		settings: UserManagerSettings,
		redirectNavigator?: INavigator,
		popupNavigator?: INavigator,
		iframeNavigator?: INavigator,
	) {
		super(settings, redirectNavigator, popupNavigator, iframeNavigator);
	}

	private _access_token: string | null = null;
	private _id_token: string | undefined;

	public override async signinCallback(url?: string): Promise<User | void> {
		const user = await super.signinCallback(url);
		if (user) {
			this._access_token = user.access_token;
		}
		return user;
	}

	public async signInWithUsernamePasswordAsync(username: string, password: string): Promise<void> {
		await this.signinRedirect({
			extraQueryParams: {
				username,
				password,
			},
		});
	}

	public async signOutSilentAsync() {
		await fetch('/api/_logout');
		goto('/login');
	}

	public async signInSilentWithCookieAsync(isRedirect: boolean = true): Promise<boolean> {
		const tokenEndpoint = await this._client.metadataService.getTokenEndpoint();

		const res = await fetch(tokenEndpoint!, {
			credentials: 'include',
			headers: {
				Accept: getMediaType('application/json'),
				[CONTENT_TYPE_HEADER_KEY]: getMediaType('application/x-www-form-urlencoded'),
			},
			body: 'grant_type=refresh_token&refresh_token=http_cookies&scope=openid+offline_access&client_id=autoflow_portal',
			method: 'POST',
		});

		if (!res.ok) {
			if (isRedirect) {
				goto('/login');
			}
			return false;
		}

		const props = (await res.json()) as TokenResponseProps;
		this._access_token = props.access_token;
		this._id_token = props.id_token;

		return true;
	}

	public async fetchWithBearerAsync<T>(
		relativeUrl: string,
		body: object | null = null,
		mediaType: MediaType | null = null,
	): Promise<ResponseData<T>> {
		const resData: ResponseData<T> = {
			data: null,
			error: null,
		};

		if (!this._access_token) {
			resData.error = 'access denied';
			return resData;
		}

		let res: Response | null = null;

		res = await this._fetchAsync(relativeUrl, body, mediaType);
		if (res.status == StatusCodes.UNAUTHORIZED) {
			await this.signInSilentWithCookieAsync();
			res = await this._fetchAsync(relativeUrl, body, mediaType);
		}

		if (!res.ok) {
			resData.error = 'error when fetch data';
			return resData;
		}

		resData.data = (await res.json()) as T;
		return resData;
	}

	private async _fetchAsync(
		relativeUrl: string,
		body: object | null = null,
		mediaType: MediaType | null = null,
	): Promise<Response> {
		const headers = new Headers({
			authorization: `Bearer ${this._access_token}`,
		});
		if (mediaType) {
			headers.append(CONTENT_TYPE_HEADER_KEY, mediaType);
		} else {
			headers.append(CONTENT_TYPE_HEADER_KEY, getMediaType('application/json'));
		}

		const res = await fetch(this._client.settings.authority + relativeUrl, {
			method: body ? 'POST' : 'GET',
			body: body ? JSON.stringify(body) : null,
			headers,
		});

		return res;
	}
}

const authManager = new AuthManager({
	authority: ClientConfiguration.auth.AUTHORITY,
	client_id: ClientConfiguration.auth.CLIENT_ID,
	redirect_uri: ClientConfiguration.auth.REDIRECT_URI,
	scope: 'openid offline_access',
	fetchRequestCredentials: 'include',
	userStore: new WebStorageStateStore({ store: new InMemoryWebStorage() }),
});

export default authManager;
