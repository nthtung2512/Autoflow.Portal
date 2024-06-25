<script lang="ts">
	import type {
		User,
		Message,
		Conversation,
		FirstColumnData,
		SecondColumnData,
		UserConversationMap
	} from '$lib/types/interfaces';
	import FirstColumn from '../components/FirstColumn.svelte';
	import SecondColumn from '../components/SecondColumn.svelte';
	import { onMount, onDestroy } from 'svelte';
    import { startSignalRConnection, stopSignalRConnection, sendMessage,  addReceiveMessageListener, deleteMessageListener, deleteMessage, postUserMessage, addUserListener } from '../services/signalrService';
	import { createUserStore } from '../stores/userStore';
	import { authStore } from '../stores/authStore';
	import { createUCMapStore } from '../stores/userConversationMap';
	import { createConversationStore } from '../stores/conversationStore';
	import { createMessageStore } from '../stores/messageStore';
	import type { UUID } from 'crypto';
	import { v4 as uuidv4 } from 'uuid';

	const usersStore = createUserStore();
	const userConversationsMap = createUCMapStore();
	const conversationStore = createConversationStore();
	const messagesStore = createMessageStore();

	// Get current sender user
	$: sender = $authStore.user;
	$: senderUserId = sender.id;
	console.log('Sender: ', sender);
	$: {
		console.log('Check sender', sender);
	}

	// Get all users
	let users: User[] = [];
	$: usersStore.users.subscribe((maps) => {
		users = maps;
	});
	$: {
		console.log('Check users', users);
	}

	// Get all conversationMaps
	let userConversationMaps: UserConversationMap[] = [];
	$: userConversationsMap.userConversationMaps.subscribe((maps) => {
		userConversationMaps = maps;
	});
	$: {
		console.log('Check userConversationMaps', userConversationMaps);
	}

	// Get all conversations of sender
	let userConversationMapsByUID: UserConversationMap[] = [];
	$: userConversationsMap.userConversationMapsByUID.subscribe((maps) => {
		userConversationMapsByUID = maps;
	});

	$: {
		console.log('Check ucmap', userConversationMapsByUID);
	}


	// $: linkedReceiverConversationIds =
	// $: {
	// 	console.log('Check linkedReceiverConversationIs', linkedReceiverConversationIs);
	// }
	// GEt conversation ids of sender
	$: senderConversationsId = userConversationMapsByUID.map((uc) => uc.conversationId);
	$: {
		console.log('Check senderConversationsId', senderConversationsId);
	}
	// Receiver for these conversations
	$: filteredUserConversationsByUId = userConversationMaps.filter(
		(uc) => senderConversationsId.includes(uc.conversationId) && uc.userId !== senderUserId
	);
	$: {
		console.log('Check filteredUserConversationsByUId', filteredUserConversationsByUId);
	}
	filteredUserConversationsByUId = filteredUserConversationsByUId;
	$: receivers = users.filter((user) =>
		filteredUserConversationsByUId.some((uc) => uc.userId === user.id)
	);
	$: {
		console.log('Check receivers', receivers);
	}

	// Get conversation by id
	let conversationById: Conversation | null = null;
	$: conversationStore.conversationById.subscribe((maps) => {
		conversationById = maps;
	});

	// Get conversation of selected sender - receiver
	$: selectedConversation = conversationById;
	$: {
		console.log('Check selectedConversation', selectedConversation);
	}

	// Search query to find receiver
	let selectedReceiver: User | null = null;
	$: {
		console.log('Check selectedReceiver', selectedReceiver);
	}

	// When receiver selected, choose the conversation
	function watchSelectedReceiver(user: User | null) {
		if (user === null) {
			selectedConversation = null;
			return;
		} else {
			console.log("Watch CP1")
			filteredUserConversationsByUId.find((uc) => {
				if (uc.userId === user.id) {
					console.log("Watch CP2")
					const selectedConversationId = uc.conversationId;
					conversationStore.fetchConversationById(selectedConversationId);
				}
			});
		}
	}

	// When selected receiver changes, trigger function
	$: selectedReceiver, watchSelectedReceiver(selectedReceiver);

	// async function sendMessage(
	// 	sendUserId: UUID,
	// 	receiveUserId: UUID,
	// 	content: string,
	// 	conversationId: UUID
	// ) {
	// 	if (conversationId === null) {
	// 		const newConversationId = uuidv4();
	// 		const newMessage = <Message>{
	// 			id: uuidv4(),
	// 			content,
	// 			sendUserId,
	// 			receiveUserId,
	// 			conversationId: newConversationId
	// 		};
			
	// 		console.log('New message null: ', newMessage);
	// 		const newConversation = <Conversation>{
	// 			id: newConversationId
	// 		};

	// 		const newConversationMap1 = <UserConversationMap>{
	// 			userId: sendUserId,
	// 			conversationId: newConversationId
	// 		};

	// 		const newConversationMap2 = <UserConversationMap>{
	// 			userId: receiveUserId,
	// 			conversationId: newConversationId
	// 		};

	// 		await conversationStore.postConversation(newConversation);
	// 		await userConversationsMap.postUserConversationMap(newConversationMap1);
	// 		await userConversationsMap.postUserConversationMap(newConversationMap2);
	// 		await messagesStore.postMessage(newMessage);
	// 		await conversationStore.fetchConversationById(newConversationId);
	// 		await userConversationsMap.fetchAllUserConversationMaps();
	// 		await userConversationsMap.fetchConversationMapByUserId(sendUserId);
	// 		// selectedReceiver = users.find((user) => user.id === receiveUserId);
	// 	} else {
	// 		const newMessage = <Message>{
	// 			id: uuidv4(),
	// 			content,
	// 			sendUserId,
	// 			receiveUserId,
	// 			conversationId
	// 		};
	// 		console.log('New message: ', newMessage);
	// 		await messagesStore.postMessage(newMessage);
	// 		selectedConversation = selectedConversation;
	// 	}
	// }

	// Get all messages by selected conversation id
    let messagesForConversation: Message[] = [];
    $: messagesStore.messagesForConversation.subscribe((maps) => {
        messagesForConversation = maps;
    });
    $: selectedConversation && messagesStore.getMessagesForConversationId(selectedConversation.id);
    $: {
        console.log('Check messagesForConversation', messagesForConversation);
    }
    $: {
        console.log('Check SOS', messagesStore.messagesForConversation);
    }

	async function handleDeleteMessage(message: Message) {
		// Delete message from store and db
		await messagesStore.deleteMessage(message.id);
		await deleteMessage(message);
		// Delete message from local
		messagesForConversation = messagesForConversation.filter((msg) => msg.id !== message.id);
	}

	async function handleReceiveDeleteMessage(message: Message) {
		if (message.receiveUserId === senderUserId && message.sendUserId === selectedReceiver?.id) {
			messagesForConversation = messagesForConversation.filter((msg) => msg.id !== message.id);
		}
		// else if (message.receiveUserId === senderUserId) {
		// 	await messagesStore.getMessagesForConversationId(message.conversationId);
		// }
		else {
			return;
		}
	}

	function handleReceivePostUserMessage(user : User) {
		users
	}

	onMount(async () => {
		await startSignalRConnection();
        addReceiveMessageListener(handleReceiveMessage);
		deleteMessageListener(handleReceiveDeleteMessage);
		addUserListener(handleReceivePostUserMessage);
		usersStore.fetchUsers();
		userConversationsMap.fetchConversationMapByUserId(sender.id);
		userConversationsMap.fetchAllUserConversationMaps();
	});

	

	// Handle received messages
    async function handleReceiveMessage (message: Message) {
		console.log('Received message: ', message);
		if (message.receiveUserId === senderUserId && message.sendUserId === selectedReceiver?.id) {
			messagesForConversation = [...messagesForConversation, message];
		}
		else if (message.receiveUserId === senderUserId) {
			await userConversationsMap.fetchAllUserConversationMaps();
			await userConversationsMap.fetchConversationMapByUserId(senderUserId);
		}
		else {
			return;
		}
    }

	// Handle sending a new message
    async function handleSendMessage(
		sendUserId: UUID,
		receiveUserId: UUID,
		content: string,
		conversationId: UUID | null
	) {
		if (conversationId === null) {
			const newConversationId = <UUID>uuidv4();
			const newMessage = <Message>{
				id: uuidv4(),
				content,
				sendUserId,
				receiveUserId,
				conversationId: newConversationId
			};
			
			const newConversation = <Conversation>{
				id: newConversationId
			};

			const newConversationMap1 = <UserConversationMap>{
				userId: sendUserId,
				conversationId: newConversationId
			};

			const newConversationMap2 = <UserConversationMap>{
				userId: receiveUserId,
				conversationId: newConversationId
			};

			await conversationStore.postConversation(newConversation);
			await userConversationsMap.postUserConversationMap(newConversationMap1);
			await userConversationsMap.postUserConversationMap(newConversationMap2);
			await messagesStore.postMessage(newMessage);
			await sendMessage(newMessage);
			await conversationStore.fetchConversationById(newConversationId);
			await userConversationsMap.fetchAllUserConversationMaps();
			await userConversationsMap.fetchConversationMapByUserId(sendUserId);
			// selectedReceiver = users.find((user) => user.id === receiveUserId);
		}
		else {
			const newMessage = <Message>{
				id: uuidv4(),
				content,
				sendUserId,
				receiveUserId,
				conversationId
			};
			await sendMessage(newMessage);
			await messagesStore.postMessage(newMessage);
			selectedConversation = selectedConversation;
		}
	}

	// Clean up SignalR connection on component destroy
    onDestroy(async () => {
        await stopSignalRConnection();
    });

</script>

<svelte:head>
	<title>Home</title>
	<meta name="description" content="Svelte demo app" />
</svelte:head>

<div class="flex h-screen bg-white">
	<FirstColumn {senderUserId} bind:selectedReceiver {receivers} {handleSendMessage} />
	<SecondColumn {senderUserId} bind:selectedReceiver {selectedConversation} {handleSendMessage} {messagesForConversation} {handleDeleteMessage}/>
</div>
