<template>
  <div class="w-1/4 border-r border-gray-200 p-4 overflow-auto">
    <!-- Button to open modal -->
    <button @click="openNewMessageModal" class="bg-blue-500 text-white px-4 py-2 rounded mb-4">
      Send Message to New User
    </button>

    <button @click="logout" class="bg-red-500 text-white px-4 py-2 rounded mb-4 ml-4">
      Logout
    </button>

    <!-- Search Button -->
    <div class="mb-4">
      <input
        v-model="searchQuery"
        @input="searchReceiver"
        placeholder="Search receiver..."
        class="w-full px-4 py-2 border border-gray-400 rounded"
      />
    </div>

    <!-- Search Results / Past Receivers List -->
    <div class="overflow-auto">
      <div v-if="searchResults.length > 0">
        <ul>
          <li
            v-for="result in searchResults"
            :key="result.userId"
            @click="selectReceiver(result)"
            class="mb-2 cursor-pointer"
          >
            {{ result.username }}
          </li>
        </ul>
      </div>
      <div v-else>
        <h3 class="font-bold mb-2">No results</h3>
      </div>
    </div>
  </div>

  <!-- Modal for sending a message to a new user -->
  <NewMessageModal
    v-if="showNewMessageModal"
    :pastReceivers="firstColumnData"
    @close="showNewMessageModal = false"
    @send="props.firstColumnData.sendMessage"
  />
</template>

<script setup lang="ts">
import { ref, toRefs, type Ref } from 'vue'
import NewMessageModal from '@/components/MessageModalComponent.vue'
import type { FirstColumnData, User } from '@/types/interfaces'
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore()
console.log('AuthStore: ', authStore.isLogin, authStore.currentUser)
const props = defineProps<{
  firstColumnData: FirstColumnData
}>()

console.log('FirstColumnData: ', props.firstColumnData)

const showNewMessageModal = ref(false)

const openNewMessageModal = () => {
  showNewMessageModal.value = true
}

const searchQuery = ref('')

const stateProps = ref(props.firstColumnData.receiverUsers)
const searchResults = ref(props.firstColumnData.receiverUsers)

const searchReceiver = () => {
  searchResults.value = stateProps.value.filter((user) =>
    user.username.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
}

const selectReceiver = (receiver: User) => {
  props.firstColumnData.selectedReceiver.value = receiver
}

const logout = () => {
  authStore.logout()
}
</script>
