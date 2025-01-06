import { json } from '@sveltejs/kit';

export function GET({ cookies }) {
	cookies.set('refresh_token', '', {
		path: '/connect',
		maxAge: Number.MIN_SAFE_INTEGER,
	});

	return json(null, {
		status: 200,
	});
}
