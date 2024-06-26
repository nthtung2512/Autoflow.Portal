<script lang="ts">
    import type { Auth, User } from '$lib/types/interfaces';
    import AuthComponent from '../../components/AuthComponent.svelte';
    import { goto } from '$app/navigation';
    import { usersStore } from '../../stores/userStore';
    import { v4 as uuidv4 } from 'uuid';
    import '$lib/app.css';
	import { onDestroy, onMount } from 'svelte';
    import {startHubConnection, stopHubConnection} from '../../services/signalrService';
    import {postUserMessage} from '../../services/signalrService';
    onMount(() => {
		startHubConnection();
	});

	onDestroy(async () => {
		// Clean up or unsubscribe from SignalR events if needed
        await stopHubConnection()
	});
    const handleRegister = async () => {
        const newUser = <User>{
            id: uuidv4(),
            username: authdata.username,
            password: authdata.password
        };
        console.log("Check", newUser)
        const status = await usersStore.postUser(newUser);
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
