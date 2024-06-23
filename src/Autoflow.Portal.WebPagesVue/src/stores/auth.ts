import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { useUsersStore } from './user'
import { type UUID } from 'crypto'
import type { User } from '@/types/interfaces'
import { useRouter } from 'vue-router'

// Define the store
export const useAuthStore = defineStore('auth', () => {
  const isLogin = ref(false)
  const router = useRouter()
  const currentUser = ref<User | null>(null)

  async function login(username: string, password: string) {
    const usersStore = useUsersStore()

    try {
      // Check if the user exists
      const userExists = await usersStore.checkUser(username, password)
      console.log('User exists:', userExists)
      if (userExists) {
        isLogin.value = true
        return true
      } else {
        isLogin.value = false
        console.log('Login failed: User does not exist or credentials are incorrect')
        return false
      }
    } catch (error) {
      console.error('Error during login:', error)
      isLogin.value = false
      return false
    }
  }

  function logout() {
    console.log('logout')
    isLogin.value = false
    currentUser.value = null
    router.push('/login')
  }

  return { isLogin, currentUser, login, logout }
})
