<!-- MainComponent.svelte -->
<script lang="ts">
	import '$lib/app.css';
	import { onMount } from 'svelte';
	import { writable, derived } from 'svelte/store';
	import NewMessageModal from '../components/NewMessageModal.svelte';
	import type { FirstColumnData, User } from '$lib/types/interfaces';
	import { authStore, setAuthState } from '../stores/authStore';
	import { createUserStore } from '../stores/userStore';
	import type { UUID } from 'crypto';
	import { addUserListener } from '../services/signalrService';

	export let senderUserId: UUID;
	export let selectedReceiver;
	export let receivers: User[];
	export let handleSendMessage;

  $: {
    console.log("PROPS receiver", receivers)
  }

	const usersStore = createUserStore();

	// Get all users
	let users: User[] = [];
	$: usersStore.users.subscribe((maps) => {
		users = maps;
	});
	$: {
		console.log('Check users', users);
	}

	let showNewMessageModal = false;
	let searchQuery = '';

	function openNewMessageModal() {
		showNewMessageModal = true;
	}

	function closeNewMessageModal() {
		showNewMessageModal = false;
	}
  
	$: searchResults = receivers.filter((user) =>
		user.username.toLowerCase().includes(searchQuery.toLowerCase())
	);
  $: {
    console.log('Check searchResults', searchResults);
  }
	function selectReceiver(receiver: User) {
		selectedReceiver = receiver;
	};

	const logout = () => {
		setAuthState(false, null);
	};

	// Filter user that is not the sender and not alreay sent message
	$: filteredReceivers = users.filter(
    (user) => user.id !== senderUserId && !receivers.some((receiver) => receiver.id === user.id)
  );
  $: {
    console.log('Check filteredReceivers', filteredReceivers);
  }

   

	onMount(() => {
		usersStore.fetchUsers();
		
	});


	async function deleteAccount() {
		await usersStore.deleteUser(senderUserId);
		logout();
		
	}
	
</script>

<main class="w-1/4 border-r border-gray-200 p-4 overflow-auto">
	<!-- Button to open modal -->
	<button on:click={openNewMessageModal} class="bg-blue-500 text-white px-4 py-2 rounded mb-4">
		Send Message to New User
	</button>

	<button on:click={logout} class="bg-red-500 text-white px-4 py-2 rounded mb-4 ml-4">
		Logout
	</button>

	<button on:click={deleteAccount} class="bg-red-500 text-white px-4 py-2 rounded mb-4 ml-4">
		Delete Account
	</button>

	<!-- Search Button -->
	<div class="mb-4">
		<input
			bind:value={searchQuery}
			placeholder="Search receiver..."
			class="w-full px-4 py-2 border border-gray-400 rounded"
		/>
	</div>

	<!-- Search Results / Past Receivers List -->
	<div class="overflow-auto">
		{#if searchResults.length > 0}
			<ul class="flex flex-col w-fit items-start">
				{#each searchResults as result (result.id)}
					<button on:click={() => selectReceiver(result)} class="mb-2 cursor-pointer">
						{result.username}
					</button>
				{/each}
			</ul>
		{:else}
			<h3 class="font-bold mb-2">No results</h3>
		{/if}
	</div>
</main>

{#if showNewMessageModal}
	<NewMessageModal {senderUserId} {filteredReceivers} {closeNewMessageModal} {handleSendMessage}/>
{/if}
