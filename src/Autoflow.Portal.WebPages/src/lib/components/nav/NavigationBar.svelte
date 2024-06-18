<script lang="ts">
	import NavigationRouterLink from './_members/NavigationRouterLink.svelte';
</script>

<nav class="flex h-14 flex-row border-b-2">
	<div class="mr-16 flex w-52 flex-row items-center justify-end gap-2">
		<img alt="autoflow logo" src="/favicon.png" class="h-12" />
		<h1 class="text-lg font-bold">Autoflow</h1>
	</div>

	<div class="flex flex-1 flex-row items-center gap-7">
		<NavigationRouterLink url="/" displayName="Panel" />
		<NavigationRouterLink url="/wiki" displayName="Wiki" />
	</div>
</nav>
