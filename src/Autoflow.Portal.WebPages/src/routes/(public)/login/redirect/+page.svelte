<script lang="ts">
	import { goto } from '$app/navigation';
	import { page } from '$app/stores';
	import authManager from '$lib/auth/auth-manager';
	import { onMount } from 'svelte';

	let hasCode = false;
	let isLogging: boolean = false;

	onMount(() => {
		const code = $page.url.searchParams.get('code');
		if (code) {
			hasCode = true;
			isLogging = true;
			authManager.signinCallback().then((user) => {
				if (user) goto('/');
			});
		}
	});
</script>

<div class="flex items-center justify-center">
	{#if hasCode}
		{#if isLogging}
			<div class="flex h-screen w-screen flex-col items-center justify-center gap-4">
				<h1 class="text-3xl font-bold text-primary">Xin vui lòng đợi, đang tiến hành đăng nhập Login-Redirect</h1>
			</div>
		{:else}
			<div>
				<h1>Trang này dành cho đăng nhập</h1>
				<a href="/">Trở về trang chủ</a>
			</div>
		{/if}
	{:else}
		<div>There's nothing in here</div>
	{/if}
</div>
