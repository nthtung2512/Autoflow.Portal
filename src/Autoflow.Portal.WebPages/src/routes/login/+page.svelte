<script lang="ts">
	import type { Auth } from '$lib/types/interfaces';
	import AuthComponent from '../../components/AuthComponent.svelte';
	import { goto } from '$app/navigation';
	import { createUserStore } from '../../stores/userStore';
	
	import '$lib/app.css';

	export const users = createUserStore();
	const handleLogin = async () => {
		console.log("Check", authdata.username, authdata.password)
		const checkUser = await users.checkUser(authdata.username, authdata.password);
		console.log('Check user ', checkUser);
		authdata.username = '';
		authdata.password = '';
		if (checkUser) {
			goto('/');
		}
	};

	let authdata = <Auth>{
		title: 'Login',
		username: '',
		password: '',
		handleAuth: handleLogin,
		loginState: true
	}

</script>

<AuthComponent bind:authdata/>
