<script lang="ts">
	import * as Dialog from '$lib/components/headlesses/ui/dialog';
</script>

<Dialog.Content class="sm:max-w-[425px]">
	<Dialog.Header>
		<Dialog.Title>Runner Bot Configuration</Dialog.Title>
		<Dialog.Description>
			Get your bot configuration from this
			<a href="/" class="text-accent-foreground underline"> instruction </a>
		</Dialog.Description>
	</Dialog.Header>
	<div class="grid gap-4 py-4">
		<div class="grid grid-cols-4 items-center gap-4">
			<label for="name">Name</label>
			<input id="name" class="col-span-3 rounded-md border-1 p-1" />
		</div>
		<div class="grid grid-cols-4 items-center gap-4">
			<label for="username">Secret</label>
			<input id="username" class="col-span-3 rounded-md border-1 p-1" />
		</div>
	</div>
	<Dialog.Footer>
		<button type="submit" class="rounded-lg bg-neutral/30 p-2">Save changes</button>
	</Dialog.Footer>
</Dialog.Content>
