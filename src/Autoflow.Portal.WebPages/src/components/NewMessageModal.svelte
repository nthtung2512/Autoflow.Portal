<!-- NewMessageModal.svelte -->
<script lang="ts">
	import type { User } from '$lib/types/interfaces';

	export let senderUserId;
	export let filteredReceivers: User[];
	export let closeNewMessageModal: () => void;
	export let handleSendMessage;
	export let selectedReceiver;

	let searchQuery = '';
	let stateProps = filteredReceivers;
	let searchResults = filteredReceivers;

	$: searchResults = stateProps.filter((user) =>
		user.username.toLowerCase().includes(searchQuery.toLowerCase())
	);

	let selectedUser: User | null = null;
	$: {
		console.log('Check selectedUser', selectedUser);
	}
	function selectUser(user: User) {
		selectedUser = user;
		searchResults = [user]; // Only display the selected user
	}

	let message = '';
</script>

<div class="fixed inset-0 bg-gray-900 bg-opacity-50 z-50 flex items-center justify-center">
	<div class="bg-white p-6 rounded shadow-lg w-1/3">
		<button on:click={closeNewMessageModal} class="text-right mb-4"> Close </button>
		<h2 class="text-xl font-bold mb-4">Send Message to New User</h2>
		<input
			bind:value={searchQuery}
			placeholder="Search username..."
			class="w-full mb-4 p-2 border rounded"
		/>

		<div class="mb-4 h-24 overflow-auto">
			<ul>
				{#each searchResults as user (user.id)}
					<button
						on:click={() => selectUser(user)}
						class="cursor-pointer hover:bg-gray-200 p-2 mr-2 rounded"
					>
						{user.username}
					</button>
				{/each}
			</ul>
		</div>

		<textarea
			bind:value={message}
			placeholder="Type your message..."
			class="w-full p-2 border rounded mb-4"
		></textarea>
		<button
			on:click={() => handleSendMessage(senderUserId, selectedUser?.id, message, null)}
			on:click={() => selectedReceiver = selectedUser}
			on:click={closeNewMessageModal}
			disabled={!selectedUser || !message}
			class="bg-blue-500 text-white px-4 py-2 rounded w-full disabled:bg-blue-200"
		>
			Send
		</button>
	</div>
</div>
