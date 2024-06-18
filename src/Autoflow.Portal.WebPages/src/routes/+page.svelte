<script lang="ts">
	import ViewWrapper from '$lib/components/ViewWrapper.svelte';
	import AddRunnerBotButton from '$lib/components/pages/home/add-bot/AddRunnerBotButton.svelte';
	import BotSummaryBuilder from '$lib/components/pages/home/bot-summary/BotSummaryBuilder.svelte';
	import StatusesSelector from '$lib/components/pages/home/statuses-selector/StatusesSelector.svelte';
	import type { BotStatus } from '$lib/models/BotStatus';
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import IconSearch from '@tabler/icons-svelte/IconSearch.svelte';

	let listItems: RunnerBotModel[] = [
		{
			id: '',
			name: 'sitename/botname',
			status: 'running',
		},
		{
			id: '',
			name: 'sitename/botname1',
			status: 'error',
		},
		{
			id: '',
			name: 'sitename/botname2',
			status: 'online',
		},
		{
			id: '',
			name: 'sitename/botname3',
			status: 'offine',
		},
		{
			id: '',
			name: 'sitename/botname4',
			status: 'canceled',
		},
	];

	let statusStates: { [id in BotStatus]: boolean } = {
		running: true,
		error: true,
		online: true,
		offine: true,
		canceled: true,
	};

	let searchInputtext: string = '';

	$: filteredItem = listItems
		.filter((item) => statusStates[item.status] === true)
		.filter((item) => item.name.includes(searchInputtext));
</script>

<ViewWrapper>
	<section class="flex h-32 flex-row gap-3 border-b-1">
		<div class="flex flex-1 flex-col justify-center">
			<h1 class="text-3xl font-bold text-normal-text">Deployments</h1>
			<p class="text-neutral">
				Of Organization <span class="font-bold text-foreground">Foo Bar</span>
			</p>
		</div>

		<AddRunnerBotButton />
	</section>

	<section class="flex flex-col pt-5">
		<div class="flex flex-col gap-2 sm:flex-row [&>*]:rounded-md [&>*]:border-1">
			<div class="flex w-full flex-2 flex-row gap-2 p-2 focus-within:shadow-md">
				<IconSearch />
				<input
					bind:value={searchInputtext}
					class="w-full appearance-none bg-transparent text-foreground outline-none"
				/>
			</div>

			<StatusesSelector bind:statusStates />
		</div>

		<div class="mt-5 flex flex-col divide-y rounded-lg border-1">
			{#if filteredItem.length === 0}
				<p>You haven't added any bot</p>
			{:else}
				{#each filteredItem as item}
					<BotSummaryBuilder data={item} />
				{/each}
			{/if}
		</div>
	</section>
</ViewWrapper>
