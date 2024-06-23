<template>
  <AuthComponent :authdata="authProps" />
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import AuthComponent from '@/components/AuthComponent.vue'
import '@/assets/index.css'
import type { Auth, User } from '@/types/interfaces'
import { useUsersStore } from '@/stores/user'

const usersStore = useUsersStore()
usersStore.fetchUsers()

const username = ref('')
const password = ref('')
const router = useRouter()
const handleRegister = async (): Promise<void> => {
  // Implement check existing user logic here
  const status = await usersStore.postUser(username.value, password.value)
  // Implement add account logic here
  if (status) {
    console.log('Register successful')
    console.log('Username:', username.value)
    console.log('Password:', password.value)
    router.push('/login')
  } else {
    console.log('Register failed')
  }
}
const authProps = <Auth>{
  title: 'Register',
  username,
  password,
  handleAuth: handleRegister,
  loginState: false
}
</script>
