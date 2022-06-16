<script>	
	import auth from './auth-service';	
	import MoneyMovementsOverview from './components/money-movement/MoneyMovementsOverview.svelte';
	import NewMoneyMovement from './components/money-movement/AddMoneyMovement.svelte';
	import UseTemplate from './components/template/UseTemplate.svelte';
	let { isAuthenticated, user } = auth;
</script>

<main>
	<div>
		{#if $isAuthenticated}
			<h1>🧾 Log</h1>
			<h2>Manual log entry</h2>
			<NewMoneyMovement />
			<h2>Use template</h2>
			<UseTemplate />
			<div class="mt-8">
				<MoneyMovementsOverview />
			</div>

		{/if}
	</div>
</main>
