import type { UUID } from 'crypto';
import { defineStore } from 'pinia';
import { v4 as uuidv4 } from 'uuid'; // Import the UUID function from the 'uuid' package
import { ref } from 'vue';

// Define the store
export const useGuidStore = defineStore('guid', () => {
  const guids = ref<string>("")
    
    const createGuids = () => {
      guids.value = uuidv4();
    }

    return {
      guids,
      createGuids
    }
})