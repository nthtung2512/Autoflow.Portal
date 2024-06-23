import type { Conversation, Message, User, UserConversationMap } from '@/types/interfaces'
import axios from 'axios'

const apiClient = axios.create({
  baseURL: 'https://localhost:7198/api/chatbox', // Your API base URL
  headers: {
    'Content-Type': 'application/json'
  }
})

// API Service with CRUD operations
export default {
  // Users API
  async getUsers(): Promise<User[]> {
    const response = await apiClient.get<User[]>('/users')
    return response.data
  },
  async getUserById(userId: string): Promise<User> {
    const response = await apiClient.get<User>(`/users/${userId}`)
    return response.data
  },
  async createUser(user: User): Promise<User> {
    const response = await apiClient.post<User>('/users', user)
    return response.data
  },
  async updateUser(userId: string, user: User): Promise<void> {
    await apiClient.put(`/users/${userId}`, user)
  },
  async deleteUser(userId: string): Promise<void> {
    await apiClient.delete(`/users/${userId}`)
  },

  // Messages API
  async getMessageById(messageId: string): Promise<Message> {
    const response = await apiClient.get<Message>(`/messages/${messageId}`)
    return response.data
  },

  async getMessagesForConversation(conversationId: string): Promise<Message[]> {
    const response = await apiClient.get<Message[]>(`/messages/conversation/${conversationId}`)
    return response.data
  },

  // Conversations API
  async getConversations(): Promise<Conversation[]> {
    const response = await apiClient.get<Conversation[]>('/conversations')
    return response.data
  },
  async getConversationById(conversationId: string): Promise<Conversation> {
    const response = await apiClient.get<Conversation>(`/conversations/${conversationId}`)
    return response.data
  },
  async createConversation(conversation: Conversation): Promise<Conversation> {
    const response = await apiClient.post<Conversation>('/conversations', conversation)
    return response.data
  },
  async updateConversation(conversationId: string, conversation: Conversation): Promise<void> {
    await apiClient.put(`/conversations/${conversationId}`, conversation)
  },

  // UserConversationMap API
  async getAllUserConversationMaps(): Promise<UserConversationMap[]> {
    const response = await apiClient.get<UserConversationMap[]>('/userConversationMaps')
    return response.data
  },
  async getUserConversationMapsByUserId(userId: string): Promise<UserConversationMap[]> {
    const response = await apiClient.get<UserConversationMap[]>(
      `/userConversationMaps/user/${userId}`
    )
    return response.data
  },
  async getUserConversationMapsByConversationId(
    conversationId: string
  ): Promise<UserConversationMap[]> {
    const response = await apiClient.get<UserConversationMap[]>(
      `/userConversationMaps/conversation/${conversationId}`
    )
    return response.data
  },
  async addUserConversationMap(map: UserConversationMap): Promise<UserConversationMap> {
    const response = await apiClient.post<UserConversationMap>('/userConversationMaps', map)
    return response.data
  }
}
