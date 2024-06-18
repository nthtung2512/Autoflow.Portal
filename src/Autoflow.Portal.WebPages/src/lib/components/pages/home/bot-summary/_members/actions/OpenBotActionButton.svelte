<script lang="ts">
	import * as DropdownMenu from '$lib/components/headlesses/ui/dropdown-menu';
	import IconDots from '@tabler/icons-svelte/IconDots.svelte';
	import IconPlayerPause from '@tabler/icons-svelte/IconPlayerPause.svelte';
	import IconPlayerPlay from '@tabler/icons-svelte/IconPlayerPlay.svelte';
	import IconSettings from '@tabler/icons-svelte/IconSettings.svelte';

	export let isHeader: boolean;
</script>

<DropdownMenu.Root>
	<DropdownMenu.Trigger>
		<IconDots class={isHeader ? 'flex sm:hidden' : 'hidden sm:flex'} />
	</DropdownMenu.Trigger>

	<DropdownMenu.Content class="p-2">
		<DropdownMenu.Group>
			<DropdownMenu.Item>
				<IconSettings class="mr-2 size-4" />
				Config Registration
			</DropdownMenu.Item>
			<DropdownMenu.Item>
				<IconPlayerPlay class="mr-2 size-4" />
				Start
			</DropdownMenu.Item>
			<DropdownMenu.Item>
				<IconPlayerPause class="mr-2 size-4" />
				Stop
			</DropdownMenu.Item>
		</DropdownMenu.Group>
	</DropdownMenu.Content>
</DropdownMenu.Root>
