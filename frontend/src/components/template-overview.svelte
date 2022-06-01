<script>
	import { templateStore } from '../store';
	import { getCategoryName } from '../cashflow-utilities';
	import { getTemplates } from '../budget-api-service';

	getTemplates();
</script>

<table class="table-auto">
	<thead>
		<th>Category</th>
		<th>Amount</th>
		<th>Note</th>
		<th>Actions</th>
	</thead>
	<tbody>
		{#each $templateStore as template}
			<tr>
				<td> {getCategoryName(template.categoryId)}</td>
				<td>{template.amount}</td>
				<td>{template.note}</td>
				<td>
					<!--
						<button
						class="rounded p-1 bg-slate-200 bg-transparent"
						on:click={() => {
							if (confirm("Do you really wan't to delete this template?"))
								deleteTemplate(template.id);
						}}>🗑️</button>
					-->
					</td
				>
			</tr>
		{/each}
	</tbody>
</table>
