import type {
	Conversation,
	Message,
	User,
	UserConversationMap,
} from '$lib/types/interfaces';
import axios from 'axios';

const apiClient = axios.create({
	baseURL: 'https://localhost:7198/api/chatbox', // Your API base URL
	headers: {
		'Content-Type': 'application/json'
	}
});

// Utility function to handle errors
const handleError = (error: unknown): string => {
	if (axios.isAxiosError(error)) {
		// Check if error has a response
		if (error.response) {
			// Server responded with a status code other than 2xx
			return error.response.data?.message || error.response.statusText;
		} else if (error.request) {
			// Request was made but no response was received
			return 'No response received from the server.';
		} else {
			// Something happened in setting up the request
			return error.message;
		}
	}
	return 'An unknown error occurred.';
};

// API Service with CRUD operations
export default {
	// Users API
	async getUsers(): Promise<User[] | string> {
		try {
			const response = await apiClient.get<User[]>('/users');
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async getUserById(userId: string): Promise<User | string> {
		try {
			const response = await apiClient.get<User>(`/users/${userId}`);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async createUser(user: User): Promise<User | string> {
		try {
			const response = await apiClient.post<User>('/users', user);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async updateUser(userId: string, user: User): Promise<void | string> {
		try {
			await apiClient.put(`/users/${userId}`, user);
		} catch (error) {
			return handleError(error);
		}
	},
	async deleteUser(userId: string): Promise<void | string> {
		try {
			await apiClient.delete(`/users/${userId}`);
		} catch (error) {
			return handleError(error);
		}
	},

	// Messages API
	async getMessageById(messageId: string): Promise<Message | string> {
		try {
			const response = await apiClient.get<Message>(`/messages/${messageId}`);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},

	async getMessagesForConversation(conversationId: string): Promise<Message[] | string> {
		try {
			const response = await apiClient.get<Message[]>(`/messages/conversation/${conversationId}`);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},

	// Conversations API
	async getConversations(): Promise<Conversation[] | string> {
		try {
			const response = await apiClient.get<Conversation[]>('/conversations');
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async getConversationById(conversationId: string): Promise<Conversation | string> {
		try {
			const response = await apiClient.get<Conversation>(`/conversations/${conversationId}`);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async createConversation(conversation: Conversation): Promise<Conversation | string> {
		try {
			const response = await apiClient.post<Conversation>('/conversations', conversation);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async updateConversation(
		conversationId: string,
		conversation: Conversation
	): Promise<void | string> {
		try {
			await apiClient.put(`/conversations/${conversationId}`, conversation);
		} catch (error) {
			return handleError(error);
		}
	},

	// UserConversationMap API
	async getAllUserConversationMaps(): Promise<UserConversationMap[] | string> {
		try {
			const response = await apiClient.get<UserConversationMap[]>('/userConversationMaps');
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async getUserConversationMapsByUserId(userId: string): Promise<UserConversationMap[] | string> {
		try {
			const response = await apiClient.get<UserConversationMap[]>(
				`/userConversationMaps/user/${userId}`
			);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async getUserConversationMapsByConversationId(
		conversationId: string
	): Promise<UserConversationMap[] | string> {
		try {
			const response = await apiClient.get<UserConversationMap[]>(
				`/userConversationMaps/conversation/${conversationId}`
			);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},
	async addUserConversationMap(map: UserConversationMap): Promise<UserConversationMap | string> {
		try {
			const response = await apiClient.post<UserConversationMap>('/userConversationMaps', map);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	}
};
