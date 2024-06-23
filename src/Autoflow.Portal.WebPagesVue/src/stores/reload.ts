import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useReloadStore = defineStore('reload', () => {
  const reload = ref(true)

  const triggerReload = () => {
    reload.value = !reload.value
  }
  return {
    reload,
    triggerReload
  }
})
