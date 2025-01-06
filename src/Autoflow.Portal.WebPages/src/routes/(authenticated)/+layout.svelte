<script lang="ts">
	import authManager from '$lib/auth/auth-manager';
	import { onMount } from 'svelte';

	let isAuthenticated = false;

	onMount(() => {
		(async () => {
			isAuthenticated = await authManager.signInSilentWithCookieAsync();
		})();
	});
</script>

<div class="flex flex-col h-screen">
	{#if isAuthenticated}
		<slot />
	{:else}
		<div class="flex size-full items-center justify-center">
            <h1 class="text-3xl font-bold text-primary">Xin vui lòng đợi, đang tiến hành đăng nhập Authent</h1>
		</div>
	{/if}
</div>
