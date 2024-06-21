<template>
  <AuthComponent :authdata="authProps" />
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import AuthComponent from '@/components/AuthComponent.vue'
import '@/assets/index.css'
import type { Auth, User } from '@/types/interfaces'

const username = ref('')
const password = ref('')
const router = useRouter()

const handleLogin = () => {
  // Implement your login logic here
  const user = <User>{ username: username.value, password: password.value }
  const authStore = useAuthStore()
  const checkUser = authStore.login(user) 
  username.value = '' 
  password.value = ''
  if (checkUser) router.push('/')
}

const authProps = <Auth>{
  title: 'Login',
  username,
  password,
  handleAuth: handleLogin,
  loginState: true
}
</script>
