<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import { getAccounts, addAccount, addIncome } from './budget-api-service';

	onMount(async () => {
		await auth.createClient();

		console.log(await auth.getAccessToken());
	});

	function login() {
		auth.loginWithPopup();
	}

	function logout() {
		auth.logout();
	}

	let { isAuthenticated, user } = auth;
  let accounts = [];
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
    <button on:click={async () => {
      let response = await getAccounts();
      accounts = response.data;
      console.log(accounts);
      
      
    } } >Get accounts</button>
    <button on:click={async () => await addAccount()} >Add account</button>

    {#each accounts as account}
      <p>{account.name} ({account.id})</p>
	  <button on:click={async () => await addIncome(account.id, new Date(), 50.25)}>Add income</button>
    {/each}
  </div>
</main>
