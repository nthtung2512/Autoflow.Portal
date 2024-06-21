<template>
  <div class="fixed inset-0 bg-gray-900 bg-opacity-50 z-50 flex items-center justify-center">
    <div class="bg-white p-6 rounded shadow-lg w-1/3">
      <button @click="$emit('close')" class="text-right mb-4">
        <img :src="CloseIcon" alt="Close" class="w-4 h-4" />
      </button>
      <h2 class="text-xl font-bold mb-4">Send Message to New User</h2>
      <input
        v-model="username"
        @input="searchUsername"
        placeholder="Search username..."
        class="w-full mb-4 p-2 border rounded"
      />

      <div class="mb-4 h-24 overflow-auto">
        <ul>
          <li
            v-for="user in searchResults"
            :key="user.userId"
            @click="selectUser(user)"
            class="cursor-pointer hover:bg-gray-200 p-2 rounded"
          >
            {{ user.username }}
          </li>
        </ul>
      </div>

      <textarea
        v-model="message"
        placeholder="Type your message..."
        class="w-full p-2 border rounded mb-4"
      ></textarea>
      <button
        @click="sendMessage"
        :disabled="!selectedUser || !message"
        class="bg-blue-500 text-white px-4 py-2 rounded w-full disabled:bg-blue-200"
      >
        Send
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, type Ref } from 'vue'
import type { Auth, Conversation, FirstColumnData, User } from '@/types/interfaces'
import CloseIcon from '@/assets/close.png'
import { useUsersStore } from '@/stores/user';
import { useReloadStore } from '@/stores/reload';
import type { UUID } from 'crypto'
import { v4 as uuidv4 } from 'uuid';
const usersStore = useUsersStore()
console.log('Users Store: ', usersStore.users)

const reloadStore = useReloadStore()

const username = ref('')
const message = ref('')
const props = defineProps<{ pastReceivers: FirstColumnData }>()
console.log('Past Receivers: ', props.pastReceivers)

// Filter out users that are already receivers
const filteredUsers = usersStore.users.filter((user) => {
  return !props.pastReceivers.receiverUsers.value.some((receiver) => receiver.userId === user.userId) && !(props.pastReceivers.senderUserId === user.userId)
})
console.log('Filtered Users: ', filteredUsers)
const stateProps = ref(filteredUsers)
const searchResults = ref(filteredUsers)
const selectedUser = ref<null | User>(null)
const emit = defineEmits(['close', 'send'])
const searchUsername = () => {
  // Add your search logic here
  searchResults.value = stateProps.value.filter((user) => user.username.toLowerCase().includes(username.value.toLowerCase()))
}

const selectUser = (user: User) => {
  selectedUser.value = user
  searchResults.value = [user] // Only display the selected user
}

const sendMessage = () => {
  if (selectedUser.value) {
    // Emit the selected user ID and the message to the parent
    const newId = <UUID>uuidv4()
    emit('send', props.pastReceivers.senderUserId, selectedUser.value.userId, message.value, newId)
    reloadStore.triggerReload()
    emit('close') // Close the modal after sending
  }
}
</script>

<style scoped>
/* Add any custom styles here */
</style>
