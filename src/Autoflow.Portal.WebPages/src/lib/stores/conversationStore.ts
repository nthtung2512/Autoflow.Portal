// conversationStore.js (or .ts for TypeScript)

import { writable } from 'svelte/store';
import type { Conversation } from '$lib/types/interfaces';
import apiService from '../services/apiService';

export function createConversationStore() {
	const conversations = writable<Conversation[]>([]);
	const conversationById = writable<Conversation>();
	const fetchAllConversations = async (): Promise<boolean> => {
		const fetchedConversations = await apiService.getConversations();
		if (typeof fetchedConversations === 'string') {
			window.alert(`Error fetching conversations: ${fetchedConversations}`);
			return false;
		} else {
			conversations.set(fetchedConversations);
			return true;
		}
	};
	const fetchConversationById = async (conversationId: string): Promise<boolean> => {
		const fetchedConversation = await apiService.getConversationById(conversationId);
		if (typeof fetchedConversation === 'string') {
			window.alert(`Error fetching conversation by id: ${fetchedConversation}`);
			return false;
		} else {
			conversationById.set(fetchedConversation);
			return true;
		}
	};
	const postConversation = async (newConversation: Conversation): Promise<boolean> => {
		const response = await apiService.createConversation(newConversation);
		if (typeof response === 'string') {
			window.alert(`Error creating conversation: ${response}`);
			return false;
		} else {
			conversations.update((currentConversations) => [...currentConversations, response]);
			return true;
		}
	};
	return {
		conversations,
		conversationById,
		fetchAllConversations,
		fetchConversationById,
		postConversation
	};
}