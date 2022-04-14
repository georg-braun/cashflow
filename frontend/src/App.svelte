<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import {
		getAccounts,
		addAccount,
		addIncome,
		deleteAccount,
		getAllData
	} from './budget-api-service';
	import { accountStore, accountEntryStore } from './store';
	import NewAccount from './components/new-account.svelte';

	onMount(async () => {
		await auth.createClient();
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
					<span>&nbsp;&nbsp;{$user.name} ({$user.email})</span>
				{:else}<span>&nbsp;</span>{/if}
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

			<button on:click={async () => await addIncome(account.id, new Date(), 50.25)}
				>Add income</button
			>
		{/each}
		<div>
			<h1>Account Entries</h1>
			{#each $accountEntryStore as accountEntry}
				<div>
					{accountEntry.accountId} - {accountEntry.date} - {accountEntry.amount}
				</div>
			{/each}
		</div>
	</div>
</main>
