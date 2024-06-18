<script lang="ts">
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import BotSummary from './BotSummary.svelte';

	export let data: RunnerBotModel;
</script>

<BotSummary {data}>
	<p slot="header">Has Stopped for 1 day(s)</p>

	<div slot="information">Error Log: PLACEHOLDER...</div>

	<p slot="cause">Stopped at: FLOW</p>
</BotSummary>
