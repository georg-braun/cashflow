<script>
	import { onMount } from 'svelte';
	import auth from './authService';
	import { isAuthenticated, user } from './store';

	let auth0Client;

	onMount(async () => {
		auth0Client = await auth.createClient();

		isAuthenticated.set(await auth0Client.isAuthenticated());
		user.set(await auth0Client.getUser());
	});

	function login() {
		auth.loginWithPopup(auth0Client);
	}

	function logout() {
		auth.logout(auth0Client);
	}
</script>

<main>
	<!-- App Bar -->
	<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
		<a class="navbar-brand" href="/#">Task Manager</a>
		<button
			class="navbar-toggler"
			type="button"
			data-toggle="collapse"
			data-target="#navbarText"
			aria-controls="navbarText"
			aria-expanded="false"
			aria-label="Toggle navigation"
		>
			<span class="navbar-toggler-icon" />
		</button>
		<div class="collapse navbar-collapse" id="navbarText">
			<div class="navbar-nav mr-auto user-details">
				{#if $isAuthenticated}
					<span class="text-white">&nbsp;&nbsp;{$user.name} ({$user.email})</span>
				{:else}<span>&nbsp;</span>{/if}
			</div>
			<span class="navbar-text">
				<ul class="navbar-nav float-right">
					{#if $isAuthenticated}
						<li class="nav-item">
							<a class="nav-link" href="/#" on:click={logout}>Log Out</a>
						</li>
					{:else}
						<li class="nav-item">
							<a class="nav-link" href="/#" on:click={login}>Log In</a>
						</li>
					{/if}
				</ul>
			</span>
		</div>
	</nav>

	<!-- Application -->
	{#if !$isAuthenticated}
		<a class="btn btn-primary btn-lg mr-auto ml-auto" href="/#" role="button" on:click={login}
			>Log In</a
		>
	{/if}
</main>
