<script>
	import { categoryStore, moneyMovementStore } from '../store';
	import { calcSumByCategory, getCategoryName } from '../cashflow-utilities';

	let moneyMovements = [];
	let sumByCategory = {};
	moneyMovementStore.subscribe((value) => {
		moneyMovements = value;
		sumByCategory = calcSumByCategory(moneyMovements);
	});

	let sum = 0;

	$: {
		Object.values(sumByCategory).forEach((element) => {
			sum += element;
		});
	}

	//console.log(data);
</script>

<h1>📈 Reports</h1>
<table>
	<thead>
		<th>Category</th>
		<th>Sum</th>
	</thead>
	<tbody>
		{#each $categoryStore as category}
			<tr>
				<td>{getCategoryName(category.id)}</td>
				<td> {sumByCategory[category.id]}</td>
			</tr>
		{/each}
	</tbody>
</table>
Sum: {sum}
