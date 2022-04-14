<script>
	import { addAccount } from '../budget-api-service';

	let name = 'Cash';
</script>

<input bind:value={name} />
<button on:click={async () => await addAccount(name)}>New Account</button>
