import type { UUID } from "crypto";
import type { Ref } from "vue";

export interface UserConversationMap {
    userId: UUID;
    conversationId: UUID;
}

export interface User {
    userId: UUID;
    username: string;
    password: string;
}

export interface Message {
    messageId: UUID;
    content: string;
    receiveUserId: UUID;
    sendUserId: UUID;
}

export interface Conversation {
    conversationId: UUID;
    messagesId: UUID[];
}

export interface Auth {
    title: string;
    username: Ref<string>;
    password: Ref<string>;
    handleAuth: () => void;
    loginState: boolean;
}

export interface FirstColumnData {
    senderUserId: UUID;
    selectedReceiver: Ref<User | null>; // Vue Ref for User or null
    receiverUsers: Ref<User[]>; // Array of User objects
    sendMessage: (messageId: UUID, senderUserId: UUID, receiverUserId: UUID, message: string) => void;
  }

export interface SecondColumnData {
    senderUserId: UUID;
    selectedUser: User; // Vue Ref for User or null
    selectedConversation: Ref<Conversation | null>; // Vue Ref for Conversation or null
  }

