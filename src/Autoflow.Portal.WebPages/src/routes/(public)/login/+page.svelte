<script lang="ts">
	import authManager from '$lib/auth/auth-manager';

    let username: string = '';
    let password: string = '';

	const authPromise = authManager.signInSilentWithCookieAsync(false);

	const onLoginSubmit = (
		event: SubmitEvent & {
			currentTarget: EventTarget & HTMLFormElement;
		},
	) => {
		event.preventDefault();
		authManager.signInWithUsernamePasswordAsync(username, password);
	};
</script>

<form>
	{#await authPromise}
		<div class="h-screen w-screen flex flex-col gap-5 justify-center items-center">
			<div class="flex flex-col gap-4 items-center justify-center h-screen w-screen">
				<h1 class="text-3xl font-bold text-primary">Xin vui lòng đợi</h1>
			</div>
		</div>
	{:then isLoggedIn}
		{#if isLoggedIn}
			<div class="flex flex-col gap-3 mt-28 items-center h-screen w-screen">
				<h1 class="text-3xl font-bold">Bạn đã đăng nhập</h1>
				<a href="/" class="text-xl text-blue-600 hover:text-blue-800 hover:underline transition-colors duration-300">
					Trở về trang chủ
				</a>
			</div>	
		{:else}
		<div class="min-h-screen flex items-center justify-center bg-gray-100">
			<div class="bg-white shadow-md rounded-lg p-8 w-full max-w-md">
				<h2 class="text-3xl font-semibold text-center mb-4">Login</h2>
		
				<form on:submit={onLoginSubmit}>
					<div class="mb-4">
						<label for="username" class="block text-gray-700">Username</label>
						<input
							type="text"
							id="username"
							name="username"
                            bind:value={username}
							class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-400 focus:border-blue-400"
							required
						/>
					</div>
		
					<div class="mb-6">
						<label for="password" class="block text-gray-700">Password</label>
						<div class="relative">
							<input
								type="password"
								id="password"
								name="password"
                                bind:value={password}
								class="block w-full px-3 py-2 mt-1 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-400 focus:border-blue-400"
								required
							/>
						</div>
					</div>
		
					<button
						type="submit"
						class="w-full bg-blue-500 text-white font-semibold py-2 rounded-md shadow-md hover:bg-blue-400 active:bg-blue-600 transition duration-200"
					>
						Log in
					</button>
				</form>
			</div>
		</div>
		{/if}
	{/await}
</form>
