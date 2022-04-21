import axios from 'axios';
import { get } from 'svelte/store';

import auth from './auth-service';
import { accountStore, accountEntryStore, budgetaryItemStore } from './store';

const serverUrl = import.meta.env.VITE_BUDGET_API_SERVER;

async function makeRequest(config) {
	try {
		const token = await auth.getAccessToken();
		config.headers = {
			...config.headers,
			Authorization: `Bearer ${token}`
		};

		const response = axios.request(config);
		return response;
	} catch (error) {
		console.log(error);
	}
}

export async function sendPost(endpoint, data) {
	try {
		const token = await auth.getAccessToken();
		const config = {
			url: `${serverUrl}/api/${endpoint}`,
			method: 'POST',
			headers: {
				'content-type': 'application/json',
				Authorization: `Bearer ${token}`
			}
		};

		try {
			const response = await axios.post(`${serverUrl}/api/${endpoint}`, data, config);
		return response;
		} catch (error) {
			console.log(`${error.response.status}: ${error.response.data}`);
			return error.response;
		}

	} catch (error) {
		console.log(error);
	}
}

export async function getAllData() {
	const config = {
		url: `${serverUrl}/api/GetAll`,
		method: 'GET',
		headers: {
			'content-type': 'application/json'
		}
	};
	const response = await makeRequest(config);

	updateStore(accountStore, response.data.accounts, accountExtractId, []);
	updateStore(accountEntryStore, response.data.accountEntries, accountEntriesExtractId, []);
	updateStore(budgetaryItemStore, response.data.budgetaryItems, budgetaryItemExtractId, []);
}

const accountExtractId = (account) => account.id;
const accountEntriesExtractId = (accountEntry) => accountEntry.id;
const budgetaryItemExtractId = (budgetaryItem) => budgetaryItem.id;

export async function getAccounts() {
	const config = {
		url: `${serverUrl}/api/GetAllAccounts`,
		method: 'GET',
		headers: {
			'content-type': 'application/json'
		}
	};

	const response = await makeRequest(config);
	accountStore.set(response.data);
}

export async function addAccount(name) {
	const response = await sendPost('AddAccount', {
		Name: name
	});
	applyDataChanges(response.data);
}

export async function deleteAccount(accountId) {
	const response = await sendPost('DeleteAccount', {
		AccountId: accountId
	});

	applyDataChanges(response.data);
}

export async function addBudgetaryItem(name) {
	const response = await sendPost('AddBudgetaryItem', {
		Name: name
	});
	applyDataChanges(response.data);
}

export async function deleteBudgetaryItem(budgetaryItemId) {
	const response = await sendPost('DeleteBudgetaryItem', {
		BudgetaryItemId: budgetaryItemId
	});

	applyDataChanges(response.data);
}

export async function deleteAccountEntry(accountEntryId) {
	const response = await sendPost('DeleteAccountEntry', {
		AccountEntryId: accountEntryId
	});

	applyDataChanges(response.data);
}

export async function addAccountEntry(accountId, budgetaryItemId, date, amount, note) {
	const data = {
		AccountId: accountId,
		BudgetaryItemId: budgetaryItemId,
		Date: date,
		Amount: amount,
		Note: note
	};
	const response = await sendPost('AddAccountEntry', data);
	if (response.status === 201)
		applyDataChanges(response.data);
}

function applyDataChanges(changes) {
	updateStore(accountStore, changes.accounts, accountExtractId, changes.deletedAccountIds);
	updateStore(
		accountEntryStore,
		changes.accountEntries,
		accountEntriesExtractId,
		changes.deletedAccountEntryIds
	);
	updateStore(budgetaryItemStore, changes.budgetaryItems, budgetaryItemExtractId, changes.deletedBudgetaryItemIds);
}

function updateStore(store, newItems, getNewItemFunc, deletedItemIds) {
	try {
		// handle existing data
		const storeItems = get(store);
		const itemsById = {};
		storeItems.forEach((_) => {
			let id = getNewItemFunc(_);
			itemsById[id] = _;
		});

		// update with new items
		newItems.forEach((_) => {
			let id = getNewItemFunc(_);
			itemsById[id] = _;
		});

		// remove deletedItems
		deletedItemIds.forEach((_) => {
			delete itemsById[_];
		});

		store.set(Object.values(itemsById));
	} catch (error) {
		console.log(error);
	}
}
