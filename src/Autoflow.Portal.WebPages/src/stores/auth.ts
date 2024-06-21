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
  
  function login(newUser: User) {
    const usersStore = useUsersStore()
    usersStore.fetchUsers()
    const checkUser = usersStore.checkUser(newUser)
    if (checkUser) {
      isLogin.value = true
      const temp = usersStore.users.find((user) => user.username === newUser.username)
      if (temp) {
        currentUser.value = temp
      }
    } else {
      window.alert('User not found')
    }
    return checkUser
  }

  function logout() {
    console.log('logout')
    isLogin.value = false
    currentUser.value = null
    router.push('/login')
  }

  return { isLogin, currentUser, login, logout }
})
