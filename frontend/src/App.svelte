<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import {
		deleteAccount,
		getAllData,
		deleteAccountEntry,
		deleteBudgetaryItem
	} from './budget-api-service';
	import { accountStore, accountEntryStore, budgetaryItemStore } from './store';
	import NewAccount from './components/new-account.svelte';
	import NewAccountEntry from './components/new-account-entry.svelte';
	import NewBudget from './components/new-budget.svelte';

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
	let accountMarkedForDeletion;
	let accountMarkedForDeletionNameTypedByUser;
</script>

<main>
	<!-- App Bar -->
	<nav>
		<div>
			<div>
				{#if $isAuthenticated}
					<span>{$user.name} ({$user.email})</span>
				{:else}<span>Not logged in</span>{/if}
			</div>
			<span>
				<ul>
					{#if $isAuthenticated}
						<li>
							<a href="/#" on:click={logout}>Log Out</a>
						</li>
					{:else}
						<li>
							<a href="/#" on:click={login}>Log In</a>
						</li>
					{/if}
				</ul>
			</span>
		</div>
	</nav>
	<div>
		{#if $isAuthenticated}
			<div>
				<button
					on:click={async () => {
						await getAllData();
					}}>Get all data</button
				>
			</div>

			<h1>Accounts</h1>
			<NewAccount />
			{#each $accountStore as account}
				<div>
					{account.name} ({account.id})
					<button on:click={() => accountMarkedForDeletion = account}>Delete account</button>
				</div>
			{/each}

			{#if accountMarkedForDeletion !== undefined}
				<p>Do you really wan't to delete the account "{accountMarkedForDeletion.name}"? All associated account entries get deleted.</p>
				<input type="text" placeholder="Insert account name" bind:value={accountMarkedForDeletionNameTypedByUser}/>
				<button on:click={async () => {
					if (accountMarkedForDeletionNameTypedByUser !== accountMarkedForDeletion.name){
						console.log("Wrong account name.")
						return;
					}
			
					await deleteAccount(accountMarkedForDeletion.id);
					accountMarkedForDeletion = undefined;}}>
					I know what I'm doing 😉</button>
					
			{/if}
			
			<div>
				<h1>Account Entries</h1>
				<NewAccountEntry />
				{#each $accountEntryStore as accountEntry}
					<div>
						{accountEntry.accountId} - {accountEntry.date} - {accountEntry.amount}
						<button on:click={async () => await deleteAccountEntry(accountEntry.id)}>Delete</button>
					</div>
				{/each}
			</div>
			<div>
				<h1>Budgets</h1>
				<NewBudget />
				{#each $budgetaryItemStore as budgetaryItem}
					<div>
						{budgetaryItem.id} - {budgetaryItem.name}
						<button on:click={async () => await deleteBudgetaryItem(budgetaryItem.id)}
							>Delete</button
						>
					</div>
				{/each}
			</div>
		{/if}
	</div>
</main>
