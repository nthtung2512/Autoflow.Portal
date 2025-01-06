// conversationMapsStore.js (or .ts for TypeScript)

import { writable } from 'svelte/store';
import type { UserConversationMap } from '$lib/types/interfaces';
import apiService from '../services/apiService';
import type { UUID } from 'crypto';

export function createUCMapStore() {
	  const userConversationMaps = writable<UserConversationMap[]>([]);
	  const userConversationMapsByUID = writable<UserConversationMap[]>([]);
	  const userConversationMapsByCID = writable<UserConversationMap[]>([]);

  const fetchAllUserConversationMaps = async () => {
	const fetchedUserConversationMaps = await apiService.getAllUserConversationMaps();
	if (typeof fetchedUserConversationMaps === 'string') {
	  window.alert(`Error fetching user conversation maps: ${fetchedUserConversationMaps}`);
	} else {
	  userConversationMaps.set(fetchedUserConversationMaps);
	}
  };
  // Fetch user conversation map by user id
	const fetchConversationMapByUserId = async (userId: UUID | undefined): Promise<boolean> => {
		if (userId === undefined) {
			window.alert('No user ID provided to fetch user conversation map.');
			return false;
		}
        console.log('fetchConversationMapByUserId:', userId);
		const fetchedMap = await apiService.getUserConversationMapsByUserId(userId);
		if (typeof fetchedMap === 'string') {
			window.alert(`Error fetching user conversation map by user id: ${fetchedMap}`);
			return false;
		} else {
			userConversationMapsByUID.set(fetchedMap);
			return true;
		}
	};

	// Fetch user conversation map by conversation id
	const fetchConversationMapByConversationId = async (conversationId: string): Promise<boolean> => {
		const fetchedMap = await apiService.getUserConversationMapsByConversationId(conversationId);
		if (typeof fetchedMap === 'string') {
			window.alert(`Error fetching user conversation map by conversation id: ${fetchedMap}`);
			return false;
		} else {
			userConversationMapsByCID.set(fetchedMap);
			return true;
		}
	};
  const postUserConversationMap = async (newUserConversationMap: UserConversationMap): Promise<boolean> => {
	const response = await apiService.addUserConversationMap(newUserConversationMap);
	if (typeof response === 'string') {
	  window.alert(`Error creating user conversation map: ${response}`);
	  return false;
	} else {
	  userConversationMaps.update((userConversationMaps) => [...userConversationMaps, response]);
	  return true;
	}
  };
  return {
	userConversationMaps,
	userConversationMapsByUID,
	userConversationMapsByCID,
	fetchAllUserConversationMaps,
	fetchConversationMapByUserId,
	fetchConversationMapByConversationId,
	postUserConversationMap,
  };
}