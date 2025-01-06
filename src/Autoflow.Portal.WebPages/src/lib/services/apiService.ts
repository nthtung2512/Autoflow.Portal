import type {
	Conversation,
	Message,
	User,
	UserConversationMap,
	ErrorData
} from '$lib/types/interfaces';
import { goto } from '$app/navigation';
import axios from 'axios';
import { setAuthState } from '../stores/authStore';
const apiClient = axios.create({
	baseURL: 'https://localhost:7198/api', // Your API base URL
	headers: {
		'Content-Type': 'application/json'
	}
});

// Add a request interceptor to include the token
apiClient.interceptors.request.use(
    config => {
        const token = localStorage.getItem('jwtToken');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

function isTokenExpired(token: string): boolean {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp * 1000 < Date.now();
}

const extractErrors = (errorData: ErrorData) => {
    const errors = []
    for (const key in errorData) {
      if (Object.prototype.hasOwnProperty.call(errorData, key)) {
        errors.push(...errorData[key])
      }
    }
    return errors
  }

// Utility function to handle errors
const handleError = (error: unknown): string => {
	if (axios.isAxiosError(error)) {
		console.log("Is Axios Error: ")
		// Check if error has a response
		if (error.response && error.response.data.errors) {
			// Server responded with a status code other than 2xx
			const errors = extractErrors(error.response.data.errors)
       	 	const stringErrors = errors.join('\n')
			return stringErrors || error.response.statusText;
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
	async ensureAuthenticated() {
		const token = localStorage.getItem('jwtToken');
		if (token && !isTokenExpired(token)) {
			// When token available
			setAuthState(true, JSON.parse(localStorage.getItem('user') || '{}'));
			goto("/")
		}
		else {
			// Token is not available or expired, redirect to login
			localStorage.removeItem('jwtToken'); // Clear the invalid token
			goto("/login");
		}
	},
	
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
			console.log("Response: ", response.data)
			return response.data;
		} catch (error) {
			console.log("Error: ", error)
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
		console.log("Delete User: ", userId)
		try {
			await apiClient.delete(`/users/${userId}`);
		} catch (error) {
			return handleError(error);
		}
	},

	// Login API
	async login(username: string, password: string): Promise<User | string> {
		try {
			const response = await apiClient.post<{ token: string, user: User }>('/login', { username, password });
			const token = response.data.token;
			console.log("login return data", response.data);
	
			// Save the token in local storage
			localStorage.setItem('jwtToken', token);
	
			// Optionally, save user data if needed
			localStorage.setItem('user', JSON.stringify(response.data.user));
	
			return response.data.user;
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

	async createMessage(message: Message): Promise<Message | string> {
		console.log("Create Message: ", message)
		try {
			const response = await apiClient.post<Message>('/messages', message);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	},

	async deleteMessage(messageId: string): Promise<void | string> {
		console.log("Delete Message: ", messageId)
		try {
			await apiClient.delete(`/messages/${messageId}`);
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
		console.log("Sent map: ", map)
		try {
			const response = await apiClient.post<UserConversationMap>('/userConversationMaps', map);
			return response.data;
		} catch (error) {
			return handleError(error);
		}
	}
};
