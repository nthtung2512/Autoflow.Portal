<script lang="ts">
	const LINE_CLAMP_STYLES = {
		1: 'line-clamp-1 ',
		2: 'line-clamp-2',
		3: 'line-clamp-3',
	};

	type LineClampStyle = keyof typeof LINE_CLAMP_STYLES;

	export let line: LineClampStyle = 1;
	export let customClass: string = '';
</script>

<p class="text-ellipsis {LINE_CLAMP_STYLES[line]} {customClass}">
	<slot />
</p>
