<script lang="ts">
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import IconBroadcast from '@tabler/icons-svelte/IconBroadcast.svelte';
	import BotSummary from './BotSummary.svelte';

	export let data: RunnerBotModel;
</script>

<BotSummary {data}>
	<p slot="header">Since 10:00AM 01/01/24</p>
	<div slot="information" class="flex flex-row gap-2">
		<IconBroadcast class="text-primary" />
		<p>Ready to be used</p>
	</div>

	<p slot="cause">Registered By "Person Name"</p>
</BotSummary>
