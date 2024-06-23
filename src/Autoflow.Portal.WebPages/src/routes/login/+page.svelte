<script lang="ts">
    import type { Auth } from "$lib/types/interfaces";
    import AuthComponent from "../../components/AuthComponent.svelte";
    import { goto } from '$app/navigation';

	export let authProps: Auth;

	// Updating authProps.username and authProps.password directly
	let username = authProps.username;
	let password = authProps.password;

	// Reactive store for local updates
	$: authProps.username = username;
	$: authProps.password = password;

const handleLogin = () => {
    goto('/');
}

	// Call the handleAuth function
	const handleAuth = () => {
		authProps.handleAuth();
	};
</script>

<AuthComponent />