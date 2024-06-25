// src/stores/userStore.ts

import { writable } from 'svelte/store';
import { v4 as uuidv4 } from 'uuid';
import apiService from '../services/apiService';
import type { User } from '$lib/types/interfaces';
import type { UUID } from 'crypto';
import { setAuthState } from './authStore';

export function createUserStore() {
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
	const postUser = async (username: string, password: string): Promise<boolean> => {
		const newUser: User = {
			id: uuidv4() as unknown as UUID,
			username,
			password
		};
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
