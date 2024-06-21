<template>
    <div class="login-page min-h-screen flex items-center justify-center bg-gray-100">
      <div class="bg-white shadow-md rounded-lg p-8 w-full max-w-md">
        <h2 class="text-3xl font-semibold text-center mb-4">{{authdata.title}}</h2>
  
        <form @submit.prevent="authdata.handleAuth">
          <div class="mb-4">
            <label for="username" class="block text-gray-700">Username</label>
            <input
              type="text"
              id="username"
              v-model="authdata.username.value"
              class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-400 focus:border-blue-400"
              required
            />
          </div>
  
          <div class="mb-6">
            <label for="password" class="block text-gray-700">Password</label>
            <div class="relative">
              <input
                :type="showPassword ? 'text' : 'password'"
                id="password"
                v-model="authdata.password.value"
                class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-400 focus:border-blue-400"
                required
              />
              <button
                type="button"
                @click="togglePasswordVisibility"
                class="absolute top-1/2 transform -translate-y-1/2 right-2 leading-none p-2 rounded-md bg-gray-200 hover:bg-gray-300 focus:outline-none focus:bg-gray-300"
              >
                X
              </button>
            </div>
          </div>
  
          <button
            type="submit"
            class="w-full bg-blue-500 text-white font-semibold py-2 rounded-md shadow-md hover:bg-blue-400 active:bg-blue-600 transition duration-200"
          >
            {{ authdata.title }}
          </button>
        </form>
  
        <div v-if="authdata.loginState" class="mt-4 flex justify-between items-center">
          <RouterLink to="/register" class="text-sm text-lightpink hover:underline"
            >Register</RouterLink
          >
        </div>

        <div v-else class="mt-4 flex justify-between items-center">
          <RouterLink to="/login" class="text-sm text-lightpink hover:underline"
            >Login</RouterLink
          >
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, type Ref } from 'vue'
  import { type Auth } from '@/types/interfaces';
  import '@/assets/index.css'

  const props = defineProps<{
    authdata: Auth
  }>();
  
  const showPassword = ref(false)

  const togglePasswordVisibility = () => {
    showPassword.value = !showPassword.value
  }
  </script>
  