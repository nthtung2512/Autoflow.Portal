// src/stores/userStore.ts

import { writable } from 'svelte/store';
import apiService from '../services/apiService';
import type { User } from '$lib/types/interfaces';
import type { UUID } from 'crypto';
import { setAuthState } from './authStore';

function createUserStore() {
	const users = writable<User[]>([]);
	const fetchUsers = async () => {
		const fetchedUsers: User[] | string = await apiService.getUsers();
		if (typeof fetchedUsers === 'string') {
			window.alert(`Error fetching users: ${fetchedUsers}`);
		} else {
			console.log('Fetched users:', fetchedUsers);
			users.set(fetchedUsers);
		}
	};

	// Change this to trigger reload
	const postUser = async (newUser: User): Promise<boolean> => {
		const response = await apiService.createUser(newUser);
		if (typeof response === 'string') {
			window.alert(`Error creating user: ${response}`);
			return false;
		} else {
			console.log('Created user:', response);
			users.update((users) => [...users, response]);
			return true;
		}
	};
	const checkUser = async (username: string, password: string): Promise<boolean> => {
		console.log('Checking user store:', username, password);
		const response = await apiService.login(username, password);
		if (typeof response === 'string') {
			window.alert(`Error login: ${response}`);
			return false;
		} else {
			console.log('Logged in user:', response);
			setAuthState(true, response);
			return true;
		}
	};
	const deleteUser = async (id: UUID) => {
		const response = await apiService.deleteUser(id);
		if (typeof response === 'string') {
			window.alert(`Error deleting user: ${response}`);
		} else {
			console.log('Deleted user:', response);
			users.update((users) => users.filter((user) => user.id !== id));
		}
	};

	return {
		users,
		fetchUsers,
		postUser,
		checkUser,
		deleteUser
	};
}

export const usersStore = createUserStore();