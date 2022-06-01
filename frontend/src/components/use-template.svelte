<script>
	import { get } from 'svelte/store';
	import { addMoneyMovement } from '../budget-api-service';
	import { getCategoryName } from '../cashflow-utilities';
	import { templateStore } from '../store';

	let selectedTemplate;
	let date = new Date().toISOString().split('T')[0];
</script>

<div class="flex flex-wrap  align-middle">
	<div class=" my-auto">
		<input type="date" bind:value={date} />
	</div>
	<div class="ml-4 my-auto">
		<select bind:value={selectedTemplate}>
			{#each $templateStore as template}
				<option value={template}>
					{`${getCategoryName(template.categoryId)} | ${template.amount} | ${template.note} `  }
				</option>
			{/each}
		</select>
	</div>

	<div class="my-auto">
		<button
			class="rounded px-2 ml-4  my-auto bg-slate-200"
			on:click={async () => {
				if (selectedTemplate === undefined) {
					console.log('Select a template.');
					return;
				}
				await addMoneyMovement(selectedTemplate.categoryId, date, selectedTemplate.amount, selectedTemplate.note);
			}}
			>Add
		</button>
	</div>
</div>
