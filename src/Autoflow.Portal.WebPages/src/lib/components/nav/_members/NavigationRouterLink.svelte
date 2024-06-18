<script lang="ts">
	import { page } from '$app/stores';
	export let url: string;
	export let displayName: string;

	const isCurrentRoute = $page.url.pathname === url;
</script>

<a class="text-neutral font-medium {isCurrentRoute ? 'text-normal-text' : ''}" href={url}>
	{displayName}
</a>
