<script lang="ts">
    import Login from './login/+page.svelte';
    import Register from './register/+page.svelte';
    import { page } from '$app/stores';
    import { authStore } from '../stores/authStore';
    import apiService from '../services/apiService';
    import { onMount } from 'svelte';

    let isAuthenticated = false;
    let loading = true; // New loading state

    // Reactive declaration: automatically subscribes to the store
    $: isAuthenticated = $authStore.isAuthenticated;

    // Watch for changes in isAuthenticated and log a message
    $: {
        console.log('Check authState', isAuthenticated);
    }

    // Use onMount to trigger authentication check
    onMount(async () => {
        await apiService.ensureAuthenticated();
        loading = false; // Set loading to false after authentication check
    });
</script>

<div class="app">
    <main>
        {#if loading}
            <p>Loading...</p> <!-- Display a loading message or spinner -->
        {:else}
            {#if !isAuthenticated}
                {#if $page.url.pathname === '/register'}
                    <Register />
                {:else}
                    <Login />
                {/if}
            {:else}
                <slot />
            {/if}
        {/if}
    </main>
</div>
