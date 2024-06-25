// messageStore.ts

import { writable } from 'svelte/store';
import apiService from '../services/apiService'; // Adjust path to your apiService
import type { Message } from '$lib/types/interfaces';

export function createMessageStore() {
	// Define the writable stores
	const messagesForConversation = writable<Message[]>([]);
	const messagesById = writable<Message>();

	// Fetch all messages by Id
	const getMessagesByMessageId = async (messageId: string): Promise<boolean> => {
		const fetchedMessages = await apiService.getMessageById(messageId);
		if (typeof fetchedMessages === 'string') {
			window.alert(`Error fetching message: ${fetchedMessages}`);
			return false;
		} else {
			messagesById.set(fetchedMessages);
			return true;
		}
	};

	// Fetch all messages by conversation Id
	const getMessagesForConversationId = async (conversationId: string): Promise<boolean> => {
		const fetchedMessages = await apiService.getMessagesForConversation(conversationId);
		if (typeof fetchedMessages === 'string') {
			window.alert(`Error fetching messages: ${fetchedMessages}`);
			return false;
		} else {
			messagesForConversation.set(fetchedMessages);
			return true;
		}
	};

	// Add a new message
	const postMessage = async (newMessage: Message): Promise<boolean> => {
		const response = await apiService.createMessage(newMessage);
		if (typeof response === 'string') {
			window.alert(`Error creating message: ${response}`);
			return false;
		} else {
			messagesForConversation.update((currentMessages) => [...currentMessages, response]);
			return true;
		}
	};
	
	const deleteMessage = async (messageId: string): Promise<void> => {
		const response = await apiService.deleteMessage(messageId);
		if (typeof response === 'string') {
			window.alert(`Error deleting message: ${response}`);
		} else {
			messagesForConversation.update((currentMessages) => currentMessages.filter((message) => message.id !== messageId));
		}
	}


	return {
		messagesForConversation,
		messagesById,
		getMessagesByMessageId,
		getMessagesForConversationId,
		postMessage,
		deleteMessage
	};
}
