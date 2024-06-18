<script lang="ts">
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import { STATUS_BACKGORUND } from '$lib/styles/map-status-style';
	import Extensions from '$lib/utils/extensions';
	import OpenBotActionButton from './actions/OpenBotActionButton.svelte';

	export let data: RunnerBotModel;
</script>

<li class="flex justify-between px-3 pb-0 pt-3 sm:pb-3 lg:flex-row">
	<div class="flex flex-grow flex-col lg:flex-row">
		<div class="block w-full flex-2 sm:flex lg:w-auto">
			<div class="flex w-full flex-1 border-b border-b-gray-300 p-3 sm:border-none">
				<h1 class="w-full text-xl font-bold">{data.name}</h1>
				<OpenBotActionButton isHeader={true} />
			</div>

			<div
				class="flex w-full flex-1 flex-col border-b border-b-gray-300 p-3 sm:w-auto sm:border-none"
			>
				<div class="flex flex-row items-center">
					<span class="mr-2 size-3 rounded-full {STATUS_BACKGORUND[data.status]}"></span>
					<h2>{Extensions.capitalizeFirstLetter(data.status)}</h2>
				</div>

				<div class="ml-5 text-sm text-neutral">
					<slot name="header" />
				</div>
			</div>
		</div>

		<div class="block w-full flex-2 sm:flex lg:w-auto">
			<div
				class="flex w-full flex-1 flex-col justify-center border-b border-b-gray-300 p-3 sm:border-none lg:w-auto"
			>
				<slot name="information" />
			</div>

			<div class="flex w-full flex-1 flex-row items-center gap-2 p-3 lg:w-auto">
				<slot name="cause" />
			</div>
		</div>
	</div>

	<OpenBotActionButton isHeader={false} />
</li>
