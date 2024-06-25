<script lang="ts">
    import type { Auth } from '$lib/types/interfaces';
    import AuthComponent from '../../components/AuthComponent.svelte';
    import { goto } from '$app/navigation';
    import { createUserStore } from '../../stores/userStore';
    import '$lib/app.css';

    const userStore = createUserStore();
    const handleRegister = async () => {
        console.log("Check", authdata.username, authdata.password)
        const status = await userStore.postUser(authdata.username, authdata.password);
        console.log('Post status: ', status);
        if (status) {
            goto('/login');
        }
    };

    let authdata = <Auth>{
        title: 'Register',
        username: '',
        password: '',
        handleAuth: handleRegister,
        loginState: false
    };
</script>

<AuthComponent bind:authdata={authdata} />
