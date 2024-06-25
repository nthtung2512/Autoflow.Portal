import type { UUID } from 'crypto';

export interface UserConversationMap {
	userId: UUID;
	conversationId: UUID;
}

export interface User {
	id: UUID;
	username: string;
	password: string;
}

export interface Message {
	id: UUID;
	content: string;
	sendUserId: UUID;
	receiveUserId: UUID;
	conversationId: UUID;
}

export interface Conversation {
	id: UUID;
}

export interface Auth {
	title: string;
	username: string; // Directly use string for Svelte
	password: string; // Directly use string for Svelte
	handleAuth: () => void;
	loginState: boolean;
}

export interface FirstColumnData {
	senderUserId: UUID;
	selectedReceiver: User | null; // Directly nullable User
	receiverUsers: User[]; // Array of User objects without Ref
	sendMessage: (messageId: UUID, senderUserId: UUID, receiverUserId: UUID, message: string) => void;
}

export interface SecondColumnData {
	senderUserId: UUID;
	selectedUser: User; // Directly use User type
	selectedConversation: Conversation | null; // Directly nullable Conversation
}

export interface ApiErrorResponse {
	response: {
		data: {
			errors: Record<string, string[]>;
		};
	};
}

export interface ErrorData {
	[key: string]: string[];
}