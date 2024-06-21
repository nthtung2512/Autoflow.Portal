// Import necessary modules if using Node.js environment
import fs from 'fs';
import { v4 as uuidv4 } from 'uuid';

// Mock data generation
export const usersId = [uuidv4(), uuidv4(), uuidv4()]
const messagesId = [uuidv4(), uuidv4(), uuidv4(), uuidv4(), uuidv4()]
const conversationsId = [uuidv4(), uuidv4(), uuidv4()]
export const mockData = {
    users: [
      { userId: usersId[0], username: 'john_doe', password: 'password123' },
      { userId: usersId[1], username: 'jane_smith', password: 'securepass' },
      { userId: usersId[2], username: 'mike_jones', password: 'mypass123' },
      { userId: uuidv4(), username: 'peter', password: 'abc' }
    ],
    messages: [
      { messageId: messagesId[0], content: 'Hello!', receiveUserId: usersId[0], sendUserId: usersId[1] },
      { messageId: messagesId[1], content: 'Hi there!', receiveUserId: usersId[1], sendUserId: usersId[0] },
      { messageId: messagesId[2], content: 'How are you?', receiveUserId: usersId[0], sendUserId: usersId[2] },
      { messageId: messagesId[3], content: 'I\'m good, thanks!', receiveUserId: usersId[2], sendUserId: usersId[0] },
      { messageId: messagesId[4], content: 'Nice weather today!', receiveUserId: usersId[1], sendUserId: usersId[2] }
    ],
    conversations: [
      { conversationId: conversationsId[0], messagesId: [messagesId[0], messagesId[1]] },
      { conversationId: conversationsId[1], messagesId: [messagesId[2], messagesId[3]] },
      { conversationId: conversationsId[2], messagesId: [messagesId[4]] }
    ],
    userConversationMaps: [
      { userId: usersId[0], conversationId: conversationsId[0] },
      { userId: usersId[1], conversationId: conversationsId[0] },
      { userId: usersId[0], conversationId: conversationsId[1] },
      { userId: usersId[2], conversationId: conversationsId[1] },
      { userId: usersId[1], conversationId: conversationsId[2] },
      { userId: usersId[2], conversationId: conversationsId[2] }
    ]
  };
  
  // Optionally, write the mock data to a JSON file
  // const jsonData = JSON.stringify(mockData, null, 2);
  // fs.writeFileSync('mockData.json', jsonData);
  
  // Output the mock data for testing purposes
  console.log(mockData);
  