<script lang="ts">
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import IconAlertTriangle from '@tabler/icons-svelte/IconAlertTriangle.svelte';
	import IconCircleDashedCheck from '@tabler/icons-svelte/IconCircleDashedCheck.svelte';
	import BotSummary from './BotSummary.svelte';

	export let data: RunnerBotModel;
</script>

<BotSummary {data}>
	<p slot="header">Has Run 1 day(s)</p>

	<div slot="information" class="flex flex-col">
		<div class="flex flex-row items-center gap-2">
			<IconCircleDashedCheck class="text-success" />
			<p>13 successfull run</p>
		</div>
		<div class="flex flex-row items-center gap-2">
			<IconAlertTriangle class="text-warning" />
			<p>2 faulted run</p>
		</div>
	</div>

	<p slot="cause">Started By "Person Name"</p>
</BotSummary>
