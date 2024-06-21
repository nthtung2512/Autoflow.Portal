<template>
  <div class="w-3/4">
    <div v-if="refProps.selectedConversation" class="relative w-full box-border h-screen p-10 flex flex-col">
      <div class="flex gap-10">
        <h2 class="text-2xl font-semibold">{{ refProps.selectedUser.username }}</h2>
        <button @click="emit('close')" class="text-blue-500">Close</button>
      </div>
      <div class="mt-4 flex flex-1 flex-col gap-4">
        <div
          v-for="message in allMessages"
          :key="message.messageId"
          :class="message.sendUserId === refProps.senderUserId ? 'text-right ' : 'text-left'"
        >
          <div
            class="inline-block p-2 rounded-lg mb-2 max-w-3/4 w-fit"
            :class="
              message.sendUserId === refProps.senderUserId
                ? 'bg-blue-500 text-white text-right'
                : 'bg-gray-200 text-black text-left'
            "
          >
            {{ message.content }}
          </div>
        </div>
      </div>
      <div class="sticky bottom-5 flex items-center w-full">
        <input
          v-model="newMessage"
          placeholder="Type your message..."
          class="px-4 py-2 flex flex-1 border border-gray-400 rounded"
        />
        <button
          @click="handleSendMessage"
          :disabled="newMessage.trim() === ''"
          class="bg-blue-500 text-white px-4 py-2 rounded ml-4"
        >
          Send
        </button>
      </div>
    </div>
    <div v-else>
      <p class="text-center text-gray-500 p-10">Select a receiver to start chatting</p>
    </div>
  </div>
</template>

<script setup lang="ts">
// import { useConversationsStore } from '@/stores/conversation'
import { useMessagesStore } from '@/stores/message'
import type { Conversation, SecondColumnData } from '@/types/interfaces'
import { computed, onMounted, ref, type Ref } from 'vue'

const messagesStore = useMessagesStore()
messagesStore.fetchMessages()
const messages = messagesStore.messages

const props = defineProps<{
  secondColumnData: SecondColumnData
}>()

const refProps = ref(props.secondColumnData)
console.log('Second Column Data:', refProps.value)

const messagesId = computed(() => refProps.value.selectedConversation?.messagesId)
console.log('Messages Id:', messagesId.value)

const allMessages = messages.filter((message) => messagesId.value?.includes(message.messageId))
console.log('All Messages:', allMessages)

// Define emits
const emit = defineEmits(['close', 'send']);

const newMessage = ref('')

// Function to emit the 'send' event with the message
function handleSendMessage() {
  if (newMessage.value.trim() !== '') {
    console.log("HERE")
    // Emit the 'send' event with the message
    emit('send', refProps.value.senderUserId, refProps.value.selectedUser.userId, newMessage.value, null);
    // Clear the input field after sending
    newMessage.value = '';
  }

// Add message id to mockdata

}

</script>
