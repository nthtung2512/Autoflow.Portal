// src/stores/userStore.ts

import { writable, type Writable, get } from 'svelte/store';
import { v4 as uuidv4 } from 'uuid';
import apiService from '../services/apiService';
import type { User } from '$lib/types/interfaces';
import type { UUID } from 'crypto';

// Define the type of the user store
interface UserStore extends Writable<User[]> {
  fetchUsers: () => Promise<void>;
  postUser: (username: string, password: string) => Promise<boolean>;
  checkUser: (username: string, password: string) => Promise<boolean>;
}

const createUserStore = (): UserStore => {
  const { subscribe, set, update } = writable<User[]>([]);

  const fetchUsers = async (): Promise<void> => {
    const fetchedUsers: (User[] | string) = await apiService.getUsers();
    if (typeof fetchedUsers === 'string') {
      window.alert(`Error fetching users: ${fetchedUsers}`);
    }
    else {
      console.log('Fetched users:', fetchedUsers);
      set(fetchedUsers);
    }
  };

  const postUser = async (username: string, password: string): Promise<boolean> => {
    const newUser: User = {
      id: uuidv4() as unknown as UUID,
      username,
      password,
    };
    const response = await apiService.createUser(newUser);
    if (typeof response === 'string') {
      window.alert(`Error creating user: ${response}`);
      return false;
    }
    else {
      console.log('Created user:', response);
      update((users) => [...users, response]);
      return true;
    }
  };

  const checkUser = async (username: string, password: string): Promise<boolean> => {
    try {
      await fetchUsers();
      const user = get(userStore).find(
        (user) => user.username === username && user.password === password
      );
      console.log('User found:', user);
      return user ? true : false;
    } catch (error) {
      console.error('Error checking user:', error);
      return false;
    }
  };

  return {
    subscribe,
    set,
    update,
    fetchUsers,
    postUser,
    checkUser,
  };
};

// Instantiate the store
export const userStore: UserStore = createUserStore();
