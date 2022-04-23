<script>
	import { addCategory } from '../budget-api-service';

	let name = '';
</script>

<input placeholder="category name" bind:value={name} />
<button on:click={async () => await addCategory(name)}>Add category</button>
