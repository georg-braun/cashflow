<script>
	import { categoryStore } from '../../store';
	import { deleteCategory } from '../../budget-api-service';

	function sortyByNameAsc(a, b) {
		return a.name.localeCompare(b.name);
	}
</script>

{#each $categoryStore.sort(sortyByNameAsc) as category}
	<div>
		{category.name}
		<button
			class="rounded p-1 bg-transparent"
			on:click={async () => {
				if (
					confirm(
						`Do you really wan't to delete this category? All associated entries get deleted.`
					)
				)
					await deleteCategory(category.id);
			}}>🗑️</button
		>
	</div>
{/each}
