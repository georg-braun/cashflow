import axios from 'axios';
import { get } from 'svelte/store';

import auth from './auth-service';
import { accountStore, accountEntryStore } from './store';

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

		const response = axios.post(`${serverUrl}/api/${endpoint}`, data, config);
		return response;
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
	accountStore.set(response.data.accounts);
	accountEntryStore.set(response.data.accountEntries);
	updateStore(accountStore, [], accountExtractAccountId, []);
}

const accountExtractAccountId = (account) => account.id;

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
	console.log(response.data.accounts);
	updateStore(accountStore, response.data.accounts, accountExtractAccountId, []);
}

export async function deleteAccount(accountId) {
	const response = await sendPost('DeleteAccount', {
		AccountId: accountId
	});

	const deletedItemIds = response.data.deletedAccountIds;
	updateStore(accountStore, [], accountExtractAccountId, deletedItemIds);
}

export async function addIncome(accountId, date, amount) {
	const data = {
		AccountId: accountId,
		Date: date,
		Amount: amount
	};
	console.log(`AddIncome: ${data}`);
	await sendPost('AddIncome', data);
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

		console.log(Object.values(itemsById));
		accountStore.set(Object.values(itemsById));
	} catch (error) {
		console.log(error);
	}
}
