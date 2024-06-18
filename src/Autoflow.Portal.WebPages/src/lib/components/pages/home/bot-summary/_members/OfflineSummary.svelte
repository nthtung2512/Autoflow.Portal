<script lang="ts">
	import LineClampParagraph from '$lib/components/headlesses/paragraph/LineClampParagraph.svelte';
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import IconBroadcast from '@tabler/icons-svelte/IconBroadcast.svelte';
	import BotSummary from './BotSummary.svelte';

	export let data: RunnerBotModel;
</script>

<BotSummary {data}>
	<p slot="header">Since 10:00AM 01/01/24</p>

	<div slot="information" class="flex flex-row gap-2">
		<IconBroadcast class="text-neutral" />
		<p>Need manual setup to turn bot online</p>
	</div>

	<LineClampParagraph slot="cause">Unregistered (Registered by ...)</LineClampParagraph>
</BotSummary>
