<script>
    import { categoryStore } from '../store';
    import { deleteCategory } from '../budget-api-service';	
    let categoryMarkedForDeletion;
	let categoryMarkedForDeletionNameTypedByUser;
</script>

{#each $categoryStore as category}
	<div>
		{category.name}
		<button class="rounded p-1 bg-transparent" on:click={() => (categoryMarkedForDeletion = category)}
			>🗑️</button
		>
	</div>
{/each}

{#if categoryMarkedForDeletion !== undefined}
	<p>
		Do you really wan't to delete the category "{categoryMarkedForDeletion.name}"? All associated
		money movents get deleted.
	</p>
	<input
		type="text"
		placeholder="Insert category name"
		bind:value={categoryMarkedForDeletionNameTypedByUser}
	/>
	<button
		class="rounded p-1 bg-red"
		on:click={async () => {
			if (categoryMarkedForDeletionNameTypedByUser !== categoryMarkedForDeletion.name) {
				console.log('Wrong category name.');
				return;
			}

			await deleteCategory(categoryMarkedForDeletion.id);
			categoryMarkedForDeletion = undefined;
		}}
	>
		I know what I'm doing 😉</button
	>
{/if}
