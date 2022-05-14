<script>
	import { addCategory } from '../budget-api-service';

	let name = '';
</script>

<input placeholder="category name" bind:value={name} />
<button class="rounded p-1 bg-slate-200" on:click={async () => await addCategory(name)}>Add category</button>
