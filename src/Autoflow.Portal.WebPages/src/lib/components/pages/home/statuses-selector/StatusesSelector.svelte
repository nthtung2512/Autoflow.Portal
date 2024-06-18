<script setup lang="ts">
	import * as Drawer from '$lib/components/headlesses/ui/drawer';
	import type { BotStatus } from '$lib/models/BotStatus';
	import { STATUS_BACKGORUND } from '$lib/styles/map-status-style';
	import Extensions from '$lib/utils/extensions';
	import { clickOutside } from '$lib/utils/use-element-directives';
	import IconChevronDown from '@tabler/icons-svelte/IconChevronDown.svelte';
	import SelectStatusDropDown from './_members/SelectStatusDropDown.desktop.svelte';

	export let statusStates: { [id in BotStatus]: boolean };

	let isOpened = false;

	const changeSelectedStatuses = (status: BotStatus) => {
		statusStates[status] = !statusStates[status];
		statusStates = statusStates;
	};

	const onClick = () => (isOpened = !isOpened);

	const onClickOutside = () => {
		if (isOpened) isOpened = false;
	};
</script>

<div
	class="relative flex justify-center py-2 sm:p-0"
	use:clickOutside
	on:clickoutside={onClickOutside}
>
	<Drawer.Root>
		<button
			on:click={onClick}
			class="relative flex w-52 flex-row items-center justify-center gap-1 px-5"
		>
			<ul class="flex w-[44px] flex-row">
				{#each Extensions.castObjKeys(statusStates) as key}
					<li class="relative w-[8px]">
						<span
							class="absolute -top-[6px] flex size-[12px] shrink-0 rounded-full {statusStates[key]
								? STATUS_BACKGORUND[key]
								: 'border-1 border-neutral bg-transparent'}"
						/>
					</li>
				{/each}
			</ul>

			<p>Status</p>
			<p class="rounded-2xl bg-neutral px-2 py-0.5 text-background">
				{Object.values(statusStates).filter((item) => item).length}/5
			</p>
			<IconChevronDown />

			<Drawer.Trigger>
				<button type="button" class="absolute inset-0 md:pointer-events-none" />
			</Drawer.Trigger>
		</button>

		<SelectStatusDropDown {isOpened} states={statusStates} {changeSelectedStatuses} />

		<Drawer.Content>
			<!-- Todo: Create new Components like {SelectStatusDropDown} for drawer content -->
		</Drawer.Content>
	</Drawer.Root>
</div>
