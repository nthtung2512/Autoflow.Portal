<script lang="ts">
	import type { RunnerBotModel } from '$lib/models/RunnerBotModel';
	import CanceledSummary from './_members/CanceledSummary.svelte';
	import ErrorSummary from './_members/ErrorSummary.svelte';
	import OfflineSummary from './_members/OfflineSummary.svelte';
	import OnlineSummary from './_members/OnlineSummary.svelte';
	import RunningSummary from './_members/RunningSummary.svelte';

	export let data: RunnerBotModel;
	$: ({ id, name, status } = data);
</script>

{#if status === 'running'}
	<RunningSummary {data} />
{:else if status === 'error'}
	<ErrorSummary {data} />
{:else if status === 'online'}
	<OnlineSummary {data} />
{:else if status === 'offine'}
	<OfflineSummary {data} />
{:else if status === 'canceled'}
	<CanceledSummary {data} />
{/if}
