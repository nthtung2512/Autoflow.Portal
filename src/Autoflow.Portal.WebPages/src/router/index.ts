import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'root',
      component: HomeView, // This will be dynamically redirected
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    }, 
    {
      path: '/register',
      name: 'register',
      component: RegisterView
    }
  ]
})

// Navigation Guard
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()

  if (to.name === 'root' && !authStore.isLogin) {
    // Redirect to login if trying to access root and not logged in
    next({ name: 'login' })
  } else {
    next()
  }
})

export default router
