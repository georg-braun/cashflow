<script>
	import '../app.css';
	import { onMount } from 'svelte';
	import { getAllData } from '../budget-api-service';
	import auth from '../auth-service';

	onMount(async () => {
		console.log('Mounting app');
		await auth.createClient();
		await getAllData();
	});

	function login() {
		auth.loginWithPopup();
	}

	function logout() {
		auth.logout();
	}

	let { isAuthenticated, user } = auth;
</script>

<!-- App Bar -->

<div>
	<div class="text-2xl mt-2 mb-2 ml-2">🪙 Cashflow</div>
</div>
<nav class="bg-gray-200">
	<div class="flex">
		<div class="my-auto ml-2">
			{#if $isAuthenticated}
				<span>{$user.name} ({$user.email})</span>
			{:else}<span>Not logged in</span>{/if}

			<span>
				{#if $isAuthenticated}
					<a href="/#" on:click={logout}>Log Out</a>
				{:else}
					<a href="/#" on:click={login}>Log In</a>
				{/if}
			</span>
			<div>
				<a href="/">🧾 Log</a>
				<a class="ml-2" href="/categories">📚 Categories</a>
				<a class="ml-2" href="/reports">📈 Reports</a>
			</div>
		</div>
		<div class="ml-auto">
			{#if $isAuthenticated}
				<div>
					<button
						class="rounded p-1 bg-slate-200 text-xl"
						on:click={async () => {
							console.log('Reload data');
							await getAllData();
						}}>🔃</button
					>
				</div>
			{/if}
		</div>
	</div>
</nav>
<div class="container mx-auto mt-10">
	<slot />
</div>
