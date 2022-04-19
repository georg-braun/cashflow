<script>
	import { addBudgetaryItem } from '../budget-api-service';

	let name = 'Groceries';
</script>

<input bind:value={name} />
<button on:click={async () => await addBudgetaryItem(name)}>New budget</button>
