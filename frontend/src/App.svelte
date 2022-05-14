<script>
	import { onMount } from 'svelte';
	import auth from './auth-service';
	import { getAllData } from './budget-api-service';	
	import MoneyMovementsOverview from './components/MoneyMovementsOverview.svelte';
	import NewMoneyMovement from './components/new-money-movement.svelte';
	import NewCategory from './components/new-category.svelte';
	import CategoryOverview from './components/CategoryOverview.svelte';





	let { isAuthenticated, user } = auth;
	
</script>

<main>

	<div>
		{#if $isAuthenticated}
		

			<h1>Money movements</h1>
			<NewMoneyMovement />
			<MoneyMovementsOverview />

			<h1>Categories</h1>
			
			<CategoryOverview />
			<NewCategory />
		{/if}
	</div>
</main>
