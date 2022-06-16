<script>
	import { deleteMoneyMovement } from '../../budget-api-service';
	import { moneyMovementStore } from '../../store';
	import { getAlternatingColor } from '../reports/report-utilities';
	import { getCategoryName, showMoneyValue } from '../../cashflow-utilities';

	function sortByDateDesc(a, b) {
		// Turn your strings into dates, and then subtract them
		// to get a value that is either negative, positive, or zero.
		return new Date(b.date) - new Date(a.date);
	}
</script>

<table class="table-auto">
	<thead>
		<th>Date</th>
		<th>Category</th>
		<th>Amount</th>
		<th>Note</th>
		<th>Actions</th>
	</thead>
	<tbody>
		{#each $moneyMovementStore.sort(sortByDateDesc) as moneyMovement, i}
			<tr class="{getAlternatingColor(i)}">
				<td>{new Date(moneyMovement.date).toLocaleDateString()}</td>
				<td> {getCategoryName(moneyMovement.categoryId)}</td>
				<td>{showMoneyValue(moneyMovement.amount)}</td>
				<td>{moneyMovement.note}</td>
				<td
					><button
						class="rounded p-1 bg-slate-200 bg-transparent"
						on:click={() => {
							if (confirm("Do you really wan't to delete this money movement?"))
								deleteMoneyMovement(moneyMovement.id);
						}}>🗑️</button
					></td
				>
			</tr>
		{/each}
	</tbody>
</table>
