<script>	
	import { addTemplate } from '../../budget-api-service';
	import { categoryStore } from '../../store';

	let selectedCategory;
	let amount = 0;
	let note = '';
</script>

<div class="flex flex-wrap  align-middle">
	<div class="ml-4 my-auto">
		<select bind:value={selectedCategory}>
			{#each $categoryStore as category}
				<option value={category}>
					{category.name}
				</option>
			{/each}
		</select>
	</div>
	<div class="ml-4 my-auto">
		<input step="0.01" class="w-32 bg-slate-50" type="number" bind:value={amount} />
	</div>

	<div class="ml-4  my-auto">
		<input placeholder="note" class="w-96 bg-slate-50" bind:value={note} />
	</div>
	<div class="my-auto">
		<button
			class="rounded px-2 ml-4  my-auto bg-slate-200"
			on:click={async () => {
				if (selectedCategory === undefined) {
					console.log('Select a category.');
					return;
				}
				await addTemplate(selectedCategory.id, amount, note);
			}}
			>Add
		</button>
	</div>
</div>
