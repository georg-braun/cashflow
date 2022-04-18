<script>
	import { addIncome } from '../budget-api-service';
	import { accountStore } from '../store';

	let selectedAccount;
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
{#if selectedAccount !== undefined}
	<p>CLI: {selectedAccount.name};{date};{amount};{note}</p>
{/if}

<button on:click={async () => await addIncome(selectedAccount.id, date, amount)}>New Income</button>
