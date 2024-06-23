import { ref } from 'vue'
import { defineStore } from 'pinia'
import { v4 as uuidv4 } from 'uuid' // Importing the UUID generation function
import apiService from '@/services/apiService' // Import the API service
import type { User } from '@/types/interfaces'
import { AlertTriangle } from 'lucide-vue-next'

export const useUsersStore = defineStore('user', () => {
  const users = ref<User[]>([])

  // Fetch users from the API
  const fetchUsers = async () => {
    try {
      const fetchedUsers = await apiService.getUsers()
      console.log('Fetched users 1:', fetchedUsers)
      users.value = fetchedUsers
    } catch (error) {
      console.error('Error fetching users:', error)
    }
  }
  const extractErrors = (errorData: any) => {
    const errors = []
    for (const key in errorData) {
      if (Object.prototype.hasOwnProperty.call(errorData, key)) {
        errors.push(...errorData[key])
      }
    }
    return errors
  }
  // Add a new user via API
  const postUser = async (username: string, password: string): Promise<boolean> => {
    try {
      const createdUser = await apiService.createUser(<User>{
        id: uuidv4(),
        username: username,
        password: password
      })
      console.log('Created user:', createdUser)
      return true
    } catch (error: any) {
      if (error.response && error.response.data.errors) {
        const errors = extractErrors(error.response.data.errors)
        const stringErrors = errors.join('\n')
        window.alert(`Error creating user:\n${stringErrors}`)
      } else {
        console.error('An unexpected error occurred:', error)
      }
      return false
    }
  }

  // Check if a user exists via API
  const checkUser = async (username: string, password: string): Promise<boolean> => {
    try {
      console.log('Checking user', username, password)

      // Fetch all users from the API
      await fetchUsers()
      console.log('Fetched users 2:', users.value)
      const user = users.value.find(
        (user) => user.username === username && user.password === password
      )
      console.log('User found:', user)

      return user ? true : false
    } catch (error) {
      console.error('Error checking user:', error)
      return false
    }
  }

  return {
    users,
    fetchUsers,
    postUser,
    checkUser
  }
})
