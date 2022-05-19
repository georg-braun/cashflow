<script>
	import { categoryStore, moneyMovementStore } from '../store';
	import {
		calcSumByMonthByCategory,
		calcSumByCategory,
		getCategoryName
	} from '../cashflow-utilities';

	let moneyMovements = [];
	let sumByCategory = {};
	let sumByMonthByCategory = {};
	moneyMovementStore.subscribe((value) => {
		moneyMovements = value;
		sumByCategory = calcSumByCategory(moneyMovements);
		sumByMonthByCategory = calcSumByMonthByCategory(moneyMovements);
	});

	let sum = 0;

	$: {
		Object.values(sumByCategory).forEach((element) => {
			sum += element;
		});
	}

	let currentYear = new Date().getFullYear();

	function getColor(index) {
		return index % 2 == 0 ? 'bg-slate-300' : 'white';
	}
	//console.log(data);
</script>

<h1>📈 Reports</h1>

<h2>Per month ({currentYear})</h2>
<table>
	<thead>
		<th class="bg-slate-300" />
		{#each Array.from(Array(12).keys()) as month}
			<th class="w-40 text-right bg-slate-300"> {month + 1}</th>
		{/each}
	</thead>
	<tbody>
		{#each $categoryStore as category}
			<tr>
				<td class="bg-slate-300">{getCategoryName(category.id)}</td>
				{#each Array.from(Array(12).keys()) as month}
					<td class="w-30 text-right">
						{sumByMonthByCategory[category.id][`${month + 1}-${currentYear}`] ?? ''}</td
					>
				{/each}
			</tr>
		{/each}
	</tbody>
</table>

<h2 class="mt-8">Total</h2>
<table>
	<thead>
		<th>Category</th>
		<th>Sum</th>
	</thead>
	<tbody>
		{#each $categoryStore as category, i}
			<tr class={getColor(i)}>
				<td>{getCategoryName(category.id)}</td>
				<td class="w-40 text-right">{sumByCategory[category.id]}</td>
			</tr>
		{/each}
	</tbody>
</table>
<b class="mt-8">Total: </b>{sum}
