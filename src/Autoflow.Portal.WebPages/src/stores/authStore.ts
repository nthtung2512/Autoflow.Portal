// src/stores/authStore.ts
// Simple Writable Store for variable and no method

import type { User } from '$lib/types/interfaces';
import { writable } from 'svelte/store';

export const authStore = writable<{ isAuthenticated: boolean, user: null | User }>({
  isAuthenticated: false,
  user: null
});

// Helper function to update the authentication state
export const setAuthState = (isAuthenticated: boolean, user: null | User) => {
  authStore.set({ isAuthenticated, user });
};
