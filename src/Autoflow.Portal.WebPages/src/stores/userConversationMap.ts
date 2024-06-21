import { ref } from 'vue'
import { defineStore } from 'pinia'
import { mockData } from '@/mocks/mockData'
import type { Conversation, UserConversationMap } from '@/types/interfaces'

export const useConversationMapsStore = defineStore('userConversation', () => {
  const userConversations = ref<UserConversationMap[]>([])

  const fetchConversationsMap = () => {
    userConversations.value = mockData.userConversationMaps
  }

  const postUserConversationMap = (newUserConversationMap: UserConversationMap) => {
    // Add the new user to the array
    userConversations.value.push(newUserConversationMap)
    console.log('After post ', userConversations.value)
  }

  return {
    userConversations,
    fetchConversationsMap,
    postUserConversationMap
  }
})
