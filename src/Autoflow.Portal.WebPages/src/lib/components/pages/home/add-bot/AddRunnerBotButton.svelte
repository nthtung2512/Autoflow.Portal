<script lang="ts">
	import * as Dialog from '$lib/components/headlesses/ui/dialog/index.js';
	import IconCodePlus from '@tabler/icons-svelte/IconCodePlus.svelte';
	import RegistrationConfigModal from '../registration-config-modal/RegistrationConfigModal.svelte';
</script>

<div class="group flex items-center justify-center">
	<Dialog.Root>
		<Dialog.Trigger>
			<div class="flex flex-row rounded-md bg-neutral/20 p-2">
				<IconCodePlus />

				<div
					class="flex w-0 items-center justify-center transition-all delay-300 duration-500 group-hover:w-52"
				>
					<p class="line-clamp-1">Register new Runner Bot</p>
				</div>
			</div>
		</Dialog.Trigger>

		<RegistrationConfigModal />
	</Dialog.Root>
</div>
