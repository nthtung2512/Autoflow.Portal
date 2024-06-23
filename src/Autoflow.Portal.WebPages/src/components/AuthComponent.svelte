<script lang="ts">
	// Import necessary types and styles
	import { onMount } from 'svelte';
	import type { Auth } from '$lib/types/interfaces';

	// Define props
	export let authdata: Auth;

	// Reactive variables for component state
	let showPassword = false;

	// Function to toggle password visibility
	const togglePasswordVisibility = () => {
		showPassword = !showPassword;
	};
</script>

<div class="login-page min-h-screen flex items-center justify-center bg-gray-100">
	<div class="bg-white shadow-md rounded-lg p-8 w-full max-w-md">
		<h2 class="text-3xl font-semibold text-center mb-4">{authdata.title}</h2>

		<form on:submit|preventDefault={authdata.handleAuth}>
			<div class="mb-4">
				<label for="username" class="block text-gray-700">Username</label>
				<input
					type="text"
					id="username"
					bind:value={authdata.username}
					class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-400 focus:border-blue-400"
					required
				/>
			</div>

			<div class="mb-6">
				<label for="password" class="block text-gray-700">Password</label>
				<div class="relative">
					{#if showPassword}
						<input
							type="text"
							id="password"
							bind:value={authdata.password}
							class="block w-full px-3 py-2 mt-1 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-400 focus:border-blue-400"
							required
						/>
					{:else}
						<input
							type="password"
							id="password"
							bind:value={authdata.password}
							class="block w-full px-3 py-2 mt-1 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-400 focus:border-blue-400"
							required
						/>
					{/if}
					<button
						type="button"
						on:click={togglePasswordVisibility}
						class="absolute top-1/2 transform -translate-y-1/2 right-2 leading-none p-2 rounded-md bg-gray-200 hover:bg-gray-300 focus:outline-none focus:bg-gray-300"
					>
						X
					</button>
				</div>
			</div>

			<button
				type="submit"
				class="w-full bg-blue-500 text-white font-semibold py-2 rounded-md shadow-md hover:bg-blue-400 active:bg-blue-600 transition duration-200"
			>
				{authdata.title}
			</button>
		</form>

		{#if authdata.loginState}
			<div class="mt-4 flex justify-between items-center">
				<a href="/register" class="text-sm text-lightpink hover:underline">Register</a>
			</div>
		{:else}
			<div class="mt-4 flex justify-between items-center">
				<a href="/login" class="text-sm text-lightpink hover:underline">Login</a>
			</div>
		{/if}
	</div>
</div>
