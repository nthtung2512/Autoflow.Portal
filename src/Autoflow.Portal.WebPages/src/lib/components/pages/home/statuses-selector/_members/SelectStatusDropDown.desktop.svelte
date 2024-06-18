<script lang="ts">
	import type { BotStatus } from '$lib/models/BotStatus';
	import { STATUS_BACKGORUND } from '$lib/styles/map-status-style';
	import Extensions from '$lib/utils/extensions';
	import IconCheck from '@tabler/icons-svelte/IconCheck.svelte';

	export let isOpened: boolean;
	export let states: { [id in BotStatus]: boolean };
	export let changeSelectedStatuses: (status: BotStatus) => void;
</script>

<div
	class="absolute bottom-0 left-0 hidden w-52 translate-y-full flex-col bg-background transition-opacity duration-300 md:flex
	{isOpened ? 'h-auto opacity-100' : 'pointer-events-none overflow-y-hidden opacity-0'}"
>
	<hr class="h-2" />
	<ul class="rounded-lg border-1 p-2">
		{#each Extensions.castObjKeys(states) as key}
			<li class="flex w-full">
				<button
					class="flex w-full flex-row items-center gap-2 rounded-md p-1 hover:bg-neutral/20"
					on:click={() => changeSelectedStatuses(key)}
				>
					<span class="size-2 rounded-full {STATUS_BACKGORUND[key]}" />
					<p class="flex-1 text-start">{Extensions.capitalizeFirstLetter(key)}</p>
					<IconCheck class={states[key] ? 'opacity-100' : 'opacity-0'} />
				</button>
			</li>
		{/each}
	</ul>
</div>
