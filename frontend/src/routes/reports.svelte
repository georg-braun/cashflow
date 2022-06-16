<script>
	import { categoryStore, moneyMovementStore } from '../store';
	import PerMonthReport from '../components/reports/PerMonthReport.svelte';
	import TotalPerCategoryReport from '../components/reports/TotalPerCategoryReport.svelte';

	let moneyMovements = [];
	let categories = [];

	categoryStore.subscribe((value) => {
		categories = value;
	});

	moneyMovementStore.subscribe((value) => {
		moneyMovements = value;
	});

	let currentYear = new Date().getFullYear();
</script>

<h1>📈 Reports</h1>

<h2>Per month ({currentYear})</h2>
<PerMonthReport {moneyMovements} {categories} />

<h2 class="mt-8">Total</h2>
<TotalPerCategoryReport {moneyMovements} {categories} />
