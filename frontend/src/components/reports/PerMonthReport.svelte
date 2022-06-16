<script>
	import { getCategoryName } from '../../cashflow-utilities';

	import {
		calcSumByMonth,
		calcSumByMonthByCategory,
		getColor,
		getBalanceColor
	} from './report-utilities';

	let currentYear = new Date().getFullYear();

	export let categories = [];
	export let moneyMovements = [];

	$: sumByMonthByCategory = calcSumByMonthByCategory(moneyMovements);
	$: sumByMonth = calcSumByMonth(moneyMovements);
</script>

<table>
	<thead>
		<!-- Show months -->
		<th />
		{#each Array.from(Array(12).keys()) as month}
			<th class="w-40 text-right"> {month + 1}</th>
		{/each}
	</thead>
	<tbody>
		<!-- Row per category -->
		{#each categories as category, i}
			<tr class={getColor(i)}>
				<td>{getCategoryName(category.id)}</td>
				{#each Array.from(Array(12).keys()) as month}
					<td class="w-30 text-right">
						{sumByMonthByCategory[category.id] == undefined
							? '~'
							: sumByMonthByCategory[category.id][`${month + 1}-${currentYear}`] ?? ''}</td
					>
				{/each}
			</tr>
		{/each}
		<!-- Sum by month-->
		<tr>
			<td><b>Sum</b></td>
			{#each Array.from(Array(12).keys()) as month}
				<td class="w-30 text-right {getBalanceColor(sumByMonth[`${month + 1}-${currentYear}`])}">
					<b>
						{sumByMonth[`${month + 1}-${currentYear}`] == undefined
							? '~'
							: sumByMonth[`${month + 1}-${currentYear}`] ?? ''}
					</b>
				</td>
			{/each}
		</tr>
	</tbody>
</table>
