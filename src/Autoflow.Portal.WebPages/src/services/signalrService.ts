// src/services/signalrService.ts

import type { Message, User } from '$lib/types/interfaces';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
// import { v4 as uuidv4 } from 'uuid';

let hubConnection: HubConnection | null = null;

// Initialize and start SignalR connection
export const startSignalRConnection = async () => {
    hubConnection = new HubConnectionBuilder()
        .withUrl('https://localhost:7198/chatHub')
        .configureLogging(LogLevel.Information)
        .build();

    try {
        await hubConnection.start();
        console.log('SignalR Connected');
    } catch (error) {
        console.error('Error starting SignalR connection: ', error);
    }
};

// Stop SignalR connection
export const stopSignalRConnection = async () => {
    if (hubConnection) {
        await hubConnection.stop();
        console.log('SignalR Disconnected');
    }
};

// Send a message using SignalR
export const sendMessage = async (
    message: Message
) => {
    if (!hubConnection) {
        throw new Error('SignalR connection is not established.');
    }

    console.log('Sending message:', message);

    try {
        await hubConnection.invoke('SendMessage', message);
    } catch (error) {
        console.error('Error sending message:', error);
    }
};

// Delete a message using SignalR
export const deleteMessage = async (
    message: Message
) => {
    if (!hubConnection) {
        throw new Error('SignalR connection is not established.');
    }

    console.log('Deleting message:', message);

    try {
        await hubConnection.invoke('DeleteMessage', message);
    } catch (error) {
        console.error('Error deleting message:', error);
    }
};

// Send message using SignalR everytime user created
export const postUserMessage = async (
    user: User
) => {
    if (!hubConnection) {
        throw new Error('SignalR connection is not established.');
    }

    console.log('Posting user:', user);

    try {
        await hubConnection.invoke('PostUserMessage', user);
    } catch (error) {
        console.error('Error sending message:', error);
    }
}


// Listen for incoming messages
export const addReceiveMessageListener = (callback: (message: Message) => void) => {
    if (!hubConnection) {
        throw new Error('SignalR connection is not established.');
    }

    hubConnection.on('ReceiveMessage', (message: Message) => {
        callback(message);
    });
};

// Listen for incoming delete message
export const deleteMessageListener = (callback: (message: Message) => void) => {
    if (!hubConnection) {
        throw new Error('SignalR connection is not established.');
    }
    hubConnection.on('ReceiveDeleteMessage', (message: Message) => {
        callback(message);
    });
}

export const addUserListener = (callback: (user: User) => void) => {
    if (!hubConnection) {
        throw new Error('SignalR connection is not established.');
    }

    hubConnection.on('ReceivePostUserMessage', (user: User) => {
        callback(user);
    });
}