<script>
	import { addCategory } from '../budget-api-service';

	let name = '';
</script>

<input class="bg-slate-50" placeholder="category name" bind:value={name} />
<button class="rounded px-2 bg-slate-200" on:click={async () => await addCategory(name)}>Add</button>
