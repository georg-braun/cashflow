<script>
	import { getCategoryName, showMoneyValue } from '../../cashflow-utilities';

	import {
		calcSumByCategory,
		getAlternatingColor,
	} from './report-utilities';

	export let categories = [];
	export let moneyMovements = [];
	let sum = 0;

	$: {
		Object.values(sumByCategory).forEach((element) => {
			sum += element;
		});
	}
	
	$: sumByCategory = calcSumByCategory(moneyMovements);
</script>

<table>
	<thead>
		<th>Category</th>
		<th>Sum</th>
	</thead>
	<tbody>
		{#each categories as category, i}
			<tr class={getAlternatingColor(i)}>
				<td>{getCategoryName(category.id)}</td>
				<td class="w-40 text-right">{showMoneyValue(sumByCategory[category.id])}</td>
			</tr>
		{/each}
	</tbody>
</table>
<b class="mt-8">Total: </b>{showMoneyValue(sum)}
