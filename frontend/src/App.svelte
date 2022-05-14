<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import { deleteCategory, getAllData, deleteMoneyMovement } from './budget-api-service';
	import { moneyMovementStore, categoryStore } from './store';
	import MoneyMovementsOverview from './components/MoneyMovementsOverview.svelte';
	import NewMoneyMovement from './components/new-money-movement.svelte';
	import NewCategory from './components/new-category.svelte';

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
	let categoryMarkedForDeletion;
	let categoryMarkedForDeletionNameTypedByUser;
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
				<button class="rounded p-1 bg-slate-200"
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
			{#each $categoryStore as category}
				<div>
					{category.name}
					<button class="rounded p-1 bg-red-200" on:click={() => (categoryMarkedForDeletion = category)}>Delete category</button>
				</div>
			{/each}

			{#if categoryMarkedForDeletion !== undefined}
				<p>
					Do you really wan't to delete the category "{categoryMarkedForDeletion.name}"? All
					associated money movents get deleted.
				</p>
				<input
					type="text"
					placeholder="Insert category name"
					bind:value={categoryMarkedForDeletionNameTypedByUser}
				/>
				<button class="rounded p-1 bg-red"
					on:click={async () => {
						if (categoryMarkedForDeletionNameTypedByUser !== categoryMarkedForDeletion.name) {
							console.log('Wrong category name.');
							return;
						}

						await deleteCategory(categoryMarkedForDeletion.id);
						categoryMarkedForDeletion = undefined;
					}}
				>
					I know what I'm doing 😉</button
				>
			{/if}
		{/if}
	</div>
</main>
