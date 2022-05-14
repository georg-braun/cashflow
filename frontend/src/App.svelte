<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import { getAllData } from './budget-api-service';	
	import MoneyMovementsOverview from './components/MoneyMovementsOverview.svelte';
	import NewMoneyMovement from './components/new-money-movement.svelte';
	import NewCategory from './components/new-category.svelte';
	import CategoryOverview from './components/CategoryOverview.svelte';

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

<main>
	<!-- App Bar -->
	<nav>
		<div>
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
		</div>
	</nav>
	<div>
		{#if $isAuthenticated}
			<div>
				<button
					class="rounded p-1 bg-slate-200"
					on:click={async () => {
						await getAllData();
					}}>Force refresh</button
				>
			</div>

			<h1>Money movements</h1>
			<NewMoneyMovement />
			<MoneyMovementsOverview />

			<h1>Categories</h1>
			<NewCategory />
			<CategoryOverview />
		{/if}
	</div>
</main>
