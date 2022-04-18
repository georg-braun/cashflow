<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import {
		getAccounts,
		addAccount,
		addIncome,
		deleteAccount,
		getAllData,
		deleteAccountEntry
	} from './budget-api-service';
	import { accountStore, accountEntryStore } from './store';
	import NewAccount from './components/new-account.svelte';
	import NewIncome from './components/new-income.svelte';

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
					<button on:click={async () => await deleteAccount(account.id)}>Delete account</button>
				</div>
			{/each}
			<div>
				<h1>Account Entries</h1>
				<NewIncome />
				{#each $accountEntryStore as accountEntry}
					<div>
						{accountEntry.accountId} - {accountEntry.date} - {accountEntry.amount}
						<button on:click={async () => await deleteAccountEntry(accountEntry.id)}>Delete</button>
					</div>
				{/each}
			</div>
		{/if}
	</div>
</main>
