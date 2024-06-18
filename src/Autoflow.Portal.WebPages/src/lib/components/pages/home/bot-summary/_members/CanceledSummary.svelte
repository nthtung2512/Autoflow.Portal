<script lang="ts">
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import IconLoader2 from '@tabler/icons-svelte/IconLoader2.svelte';
	import BotSummary from './BotSummary.svelte';

	export let data: RunnerBotModel;
</script>

<BotSummary {data}>
	<p slot="header">At 12:00PM 20/01/24</p>
	<div slot="information" class="flex flex-row gap-2">
		<IconLoader2 class="animate-spin" />
		<p>Cancelling</p>
	</div>

	<p slot="cause">Canceled By "Person Name"</p>
</BotSummary>
