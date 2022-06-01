<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import { getAllData } from './budget-api-service';
	import MoneyMovementsOverview from './components/MoneyMovementsOverview.svelte';
	import NewMoneyMovement from './components/new-money-movement.svelte';
	import UseTemplate from './components/use-template.svelte';
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
