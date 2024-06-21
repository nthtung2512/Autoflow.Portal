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
import { v4 as uuidv4 } from 'uuid'
const usersStore = useUsersStore()
usersStore.fetchUsers()

const username = ref('')
const password = ref('')
const router = useRouter()
const handleRegister = () => {
  // Implement check existing user logic here
  const newUser = <User>{ userId: uuidv4(), username: username.value, password: password.value }
  usersStore.postUser(newUser)
  // Implement add account logic here
  console.log('Register successful')
  console.log('Username:', username.value)
  console.log('Password:', password.value)
  router.push('/login')
}
const authProps = <Auth>{
  title: 'Register',
  username,
  password,
  handleAuth: handleRegister,
  loginState: false
}
</script>
