<script lang="ts">
	import Login from './login/+page.svelte';
	import Register from './register/+page.svelte';

	import { page } from '$app/stores';
	import { authStore } from '../stores/authStore';
	let isAuthenticated = false;
	// Reactive declaration: automatically subscribes to the store
	$: isAuthenticated = $authStore.isAuthenticated;

	// Watch for changes in isAuthenticated and log a message
	$: {
		console.log('Check authState', isAuthenticated);
	}

	
</script>

<div class="app">
	<main>
		{#if !isAuthenticated}
			{#if $page.url.pathname === '/register'}
				<Register />
			{:else}
				<Login />
			{/if}
		{:else}
			<slot />
		{/if}
	</main>
</div>
