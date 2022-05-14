<script>
	import { get} from "svelte/store"
	import { addMoneyMovement } from '../budget-api-service';
	import { categoryStore } from '../store';

	let selectedCategory;
	let date = new Date().toISOString().split('T')[0];
	let amount = 0;
	let note = '';
</script>

<div>
	Category:
	<select bind:value={selectedCategory}>
		{#each $categoryStore as category}
			<option value={category}>
				{category.name}
			</option>
		{/each}
	</select>
</div>
<div>
	Amount:
	<input step="0.01" type="number" bind:value={amount} />
</div>
<div>
	Date:
	<input type="date" bind:value={date} />
</div>
<div>
	Note:
	<input placeholder="note" bind:value={note} />
</div>


<button class="rounded p-1 bg-slate-200"
	on:click={async () =>
		{
			if (selectedCategory === undefined){
				console.log("Select a category.");
				return;
			}
			await addMoneyMovement(selectedCategory.id, date, amount, note)
		
		}
	}
	>Add money movement
</button>
