<script>
	import { deleteCategory, getAllData, deleteMoneyMovement } from '../budget-api-service';
	import { moneyMovementStore, categoryStore } from '../store';
	import { getCategoryName } from '../cashflow-utilities';

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
		{#each $moneyMovementStore.sort(sortByDateDesc) as moneyMovement}
			<tr>
				<td>{new Date(moneyMovement.date).toLocaleDateString()}</td>
				<td> {getCategoryName(moneyMovement.categoryId)}</td>
				<td>{moneyMovement.amount}</td>
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
