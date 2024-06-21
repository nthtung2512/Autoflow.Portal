import { ref } from 'vue';
import { defineStore } from 'pinia';
import { v4 as uuidv4 } from 'uuid'; // Importing the UUID generation function
import { mockData } from '@/mocks/mockData';
import type { User } from '@/types/interfaces';

export const useUsersStore = defineStore('user', () => {
  const users = ref<User[]>([]);
  
  // Fetch users from mock data
  const fetchUsers = () => {
    users.value = mockData.users;
  };

  // Add a new user
  const postUser = (newUser: User) => {
    const {username, password} = newUser;
    const checkUser = users.value.find((user) => user.username === username && user.password === password);
    // Add the new user to the array
    if (checkUser) {
      console.log('User already exists');
    }
    else {
      users.value.push(newUser);
    }
  };

  const checkUser = (newUser: User) => {
    console.log('Checking user', newUser, users.value);
    const checkUser = users.value.find((user) => user.username === newUser.username && user.password === newUser.password);
    return checkUser;
  }

  return {
    users,
    fetchUsers,
    postUser,
    checkUser
  };
});
