<script>
	import NavigationBar from '$lib/components/nav/NavigationBar.svelte';
	import '../app.css';
</script>

<div class="flex flex-col">
	<NavigationBar />

	<slot />
</div>
