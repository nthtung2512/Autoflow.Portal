<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import type {
  User,
  Message,
  Conversation,
  FirstColumnData,
  SecondColumnData,
  UserConversationMap
} from '@/types/interfaces'
import { useConversationsStore } from '@/stores/conversation'
import { useMessagesStore } from '@/stores/message'
import { useUsersStore } from '@/stores/user'
import FirstColumnComponent from '@/components/FirstColumnComponent.vue'
import SecondColumnComponent from '@/components/SecondColumnComponent.vue'
import { renderVNode } from 'vue/server-renderer'
import { usersId } from '@/mocks/mockData'
import { useConversationMapsStore } from '@/stores/userConversationMap'
import type { UUID } from 'crypto'
import { v4 as uuidv4 } from 'uuid'
import { useAuthStore } from '@/stores/auth'
import { useReloadStore } from '@/stores/reload'
import apiService from '@/services/apiService'

// Initialize stores
const usersStore = useUsersStore()
usersStore.fetchUsers()
const users = usersStore.users

const messagesStore = useMessagesStore()

const conversationsStore = useConversationsStore()
conversationsStore.fetchConversations()
const conversations = conversationsStore.conversations

const conversationMapsStore = useConversationMapsStore()
conversationMapsStore.fetchConversationsMap()
const userConversation = conversationMapsStore.userConversations

const authStore = useAuthStore()
console.log('Current User: ', authStore.currentUser)

const reloadStore = useReloadStore()

// Get current sender user
const senderUserId = authStore.currentUser?.userId // Dummy user ID
const getSender = users.filter((user) => user.userId === senderUserId)[0]
console.log('Sender User: ', getSender)

// Get all conversations of current sender user
const getSenderConversationsEntry = ref(
  userConversation.filter((usconv) => usconv.userId === senderUserId)
)
const getSenderConversations = ref(
  getSenderConversationsEntry.value.map((item) => item.conversationId)
)
console.log('Sender Conversations: ', getSenderConversations.value)

// Get all receivers of current sending user
const getReceiverEntry = ref(
  getSenderConversationsEntry.value.map(
    (item) =>
      userConversation.filter(
        (usconv) => usconv.conversationId === item.conversationId && usconv.userId !== senderUserId
      )[0]
  )
)
console.log('Receiver Entry: ', getReceiverEntry.value)
const getReceiverUserIds = ref(getReceiverEntry.value.map((item) => item.userId))
console.log('Receiver User Ids: ', getReceiverUserIds.value)

// Get all receiver usernames
const receiverUsers = ref<User[]>(
  getReceiverUserIds.value.map(
    (receiverUserId) => users.filter((user) => user.userId === receiverUserId)[0]
  )
)
console.log('Receiver Users: ', receiverUsers.value)

// Get conversation of sender - receiver
const selectedConversation = ref<Conversation | null>(null)

// Search query to find receiver
const selectedReceiver = ref<User | null>(null)
watch(selectedReceiver, (newValue, oldValue) => {
  if (newValue === null) {
    renderKey.value++
    selectedConversation.value = null
    return
  }
  const selectedConversationId = getReceiverEntry.value.find(
    (usconv) => usconv.userId === newValue.userId
  )?.conversationId
  selectedConversation.value = conversations.find(
    (conv) => conv.conversationId === selectedConversationId
  )
  console.log('Selected Conversation:', selectedConversation.value)
  secondColumnData.selectedUser = newValue
  renderKey.value++
})
// Send message to selected receiver
const sendMessage = (
  sendUserId: UUID,
  receiveUserId: UUID,
  content: string,
  conversationId: UUID
) => {
  // Add message to messages:
  const newMessage = <Message>{
    messageId: uuidv4(),
    content,
    receiveUserId,
    sendUserId
  }

  messagesStore.postMessage(newMessage)

  if (conversationId === null) {
    console.log('Check 1', selectedConversation.value)
    conversationsStore.postMessageToConversation(
      selectedConversation.value?.conversationId,
      newMessage
    )
  } else {
    console.log('Check 2')

    const newConversation = <Conversation>(<unknown>{
      conversationId: conversationId,
      messagesId: []
    })

    const newConversationMap1 = <UserConversationMap>{
      userId: sendUserId,
      conversationId: conversationId
    }

    const newConversationMap2 = <UserConversationMap>{
      userId: receiveUserId,
      conversationId: conversationId
    }
    conversationsStore.postConversation(newConversation)
    console.log('New Conversation: ', newConversation)
    conversationsStore.postMessageToConversation(conversationId, newMessage)
    conversationMapsStore.postUserConversationMap(newConversationMap1)
    conversationMapsStore.postUserConversationMap(newConversationMap2)
  }
  console.log('New Message: ', newMessage)
  console.log(
    'Send messages from ',
    sendUserId,
    ' to receiver:',
    receiveUserId,
    ' with message: ',
    content
  )
  // selectedConversation.value?.conversationId
  // Update receiver user conversation
  // conversationMapsStore.updateUserConversation(receiveUserId, selectedConversation.value?.conversationId, newMessage.messageId)

  // messagesStore.fetchMessages()
  // conversationsStore.fetchConversations()
  renderKey.value++
}

// Props passed down to FirstColumnComponent
const firstColumnData = <FirstColumnData>(<unknown>{
  senderUserId,
  selectedReceiver,
  receiverUsers,
  sendMessage
})

const selectedUser = ref<User | null>(null)
// Data sent to second column
const secondColumnData = <SecondColumnData>{
  senderUserId,
  selectedUser: selectedUser.value,
  selectedConversation
}

const renderKey = ref(0)
const firstRenderKey = ref(0)

const close = () => {
  selectedReceiver.value = null
  renderKey.value++

  console.log('Close', renderKey.value)
}

// Watch the reload state and react to changes
watch(
  () => reloadStore.reload,
  () => {
    // Handle the reload logic here, like fetching data again
    // Get current sender user
    getSenderConversationsEntry.value = userConversation.filter(
      (usconv) => usconv.userId === senderUserId
    )
    getSenderConversations.value = getSenderConversationsEntry.value.map(
      (item) => item.conversationId
    )
    getReceiverEntry.value = getSenderConversationsEntry.value.map(
      (item) =>
        userConversation.filter(
          (usconv) =>
            usconv.conversationId === item.conversationId && usconv.userId !== senderUserId
        )[0]
    )
    getReceiverUserIds.value = getReceiverEntry.value.map((item) => item.userId)
    receiverUsers.value = getReceiverUserIds.value.map(
      (receiverUserId) => users.filter((user) => user.userId === receiverUserId)[0]
    )
    console.log('Receiver Users: ', receiverUsers.value)

    // Update firstColumnData
    firstColumnData.receiverUsers = receiverUsers

    // Signal that Home view should reload
    console.log('Home view should reload now')
    reloadStore.reload = false
    firstRenderKey.value++
  }
)
</script>

<template>
  <main class="flex h-screen bg-white">
    <FirstColumnComponent :key="firstRenderKey" :firstColumnData="firstColumnData" />
    <!-- Add key to reload the component on data change -->
    <SecondColumnComponent
      :key="renderKey"
      :secondColumnData="secondColumnData"
      @close="close"
      @send="sendMessage"
    />
  </main>
</template>
