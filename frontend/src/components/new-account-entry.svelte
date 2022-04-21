<script>
	import { addAccountEntry } from '../budget-api-service';
	import { accountStore, budgetaryItemStore } from '../store';

	let selectedAccount;
	let selectedBudgetaryItem;
	let date = '2022-04-15';
	let amount = 0;
	let note = '';
</script>

<div>
	Account:
	<select bind:value={selectedAccount}>
		{#each $accountStore as account}
			<option value={account}>
				{account.name}
			</option>
		{/each}
	</select>
</div>
<div>
	Budget:
	<select bind:value={selectedBudgetaryItem}>
		{#each $budgetaryItemStore as budgetaryItem}
			<option value={budgetaryItem}>
				{budgetaryItem.name}
			</option>
		{/each}
	</select>
	<button on:click={() => selectedBudgetaryItem = undefined}>Reset</button>
</div>
<div>
	Amount:
	<input type="number" bind:value={amount} />
</div>
<div>
	Date:
	<input type="date" bind:value={date} />
</div>
<div>
	Note:
	<input bind:value={note} />
</div>


<button
	on:click={async () =>
	{
		const defaultGuid = "00000000-0000-0000-0000-000000000000";
		await addAccountEntry(selectedAccount.id, defaultGuid, date, amount)
	}
	}
		
	>Add income</button
>

<button
	on:click={async () => {
		if (selectedAccount === undefined){
			console.log("Select an account.");
			return;
		}

		if (selectedBudgetaryItem === undefined){
			console.log("Select an budget.");
			return;
		}
		
		await addAccountEntry(selectedAccount.id, selectedBudgetaryItem.id, date, amount)}
	}
		
	>Add spending</button
>