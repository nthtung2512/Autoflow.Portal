<script lang="ts">
    import type { Auth, User } from '$lib/types/interfaces';
    import AuthComponent from '../../components/AuthComponent.svelte';
    import { goto } from '$app/navigation';
    import { createUserStore } from '../../stores/userStore';
    import {postUserMessage} from '../../services/signalrService';
    import { v4 as uuidv4 } from 'uuid';
    import '$lib/app.css';

    const userStore = createUserStore();
    const handleRegister = async () => {
        const newUser = <User>{
            id: uuidv4(),
            username: authdata.username,
            password: authdata.password
        };
        console.log("Check", newUser)
        const status = await userStore.postUser(newUser);
        console.log('Post status: ', status);
        if (status) {
            await postUserMessage(newUser);
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
