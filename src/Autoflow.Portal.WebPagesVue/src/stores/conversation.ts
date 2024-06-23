import { ref } from 'vue'
import { defineStore } from 'pinia'
import { mockData } from '@/mocks/mockData'
import type { Conversation } from '@/types/interfaces'
import type { Message } from 'postcss'
import type { UUID } from 'crypto'

export const useConversationsStore = defineStore('conversation', () => {
    const conversations = ref<Conversation[]>([])
    
    const fetchConversations = () => {
        conversations.value = mockData.conversations
    }

    // Add a new user
    const postMessageToConversation = (addedConId: UUID, newMessage: Message) => {
        console.log("addedConId", addedConId)
        // Add the new user to the array
        console.log("Before", conversations.value.find(con => con.conversationId === addedConId)?.messagesId)
        conversations.value.find(con => con.conversationId === addedConId)?.messagesId.push(newMessage.messageId)
        console.log("After", conversations.value.find(con => con.conversationId === addedConId)?.messagesId)
    }

    const postConversation = (newConversation: Conversation) => {
        // Add the new user to the array
        conversations.value.push(newConversation)
        console.log("After post ", conversations.value)
    }

    return {
        conversations,
        fetchConversations,
        postMessageToConversation,
        postConversation
    }

})
