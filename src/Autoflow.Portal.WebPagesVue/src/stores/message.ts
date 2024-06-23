import { ref } from 'vue'
import { defineStore } from 'pinia'
import { mockData } from '@/mocks/mockData'
import type { Message } from '@/types/interfaces'
import apiService from '@/services/apiService'

export const useMessagesStore = defineStore('message', () => {
  const messages = ref<Message[]>([]) // Initial fetch

  // Fetch all messages (you might call this to refresh manually)
  const fetchMessages = async (): Promise<void> => {
    // Fetch messages from the API
    const fetchedMessages = await apiService.getMessages()
    messages.value = mockData.messages
  }

  // Add a new message
  const postMessage = (newMessage: Message) => {
    messages.value.push(newMessage)
  }

  return {
    messages,
    fetchMessages,
    postMessage
  }
})
