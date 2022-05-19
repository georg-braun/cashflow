import axios from 'axios';
import { get } from 'svelte/store';

import auth from './auth-service';
import { moneyMovementStore, categoryStore } from './store';

const serverUrl = import.meta.env.VITE_BUDGET_API_SERVER;

const moneyMovementExtractId = (accountEntry) => accountEntry.id;
const categoryExtractId = (budgetaryItem) => budgetaryItem.id;


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

	applyDataChanges(response.data);
}

export async function addCategory(name) {
	const response = await sendPost('AddCategory', {
		Name: name
	});
	applyDataChanges(response.data);
}

export async function deleteCategory(categoryItemId) {
	const response = await sendPost('DeleteCategory', {
		categoryId: categoryItemId
	});

	applyDataChanges(response.data);
}

export async function deleteMoneyMovement(moneyMovementId) {
	const response = await sendPost('DeleteMoneyMovement', {
		moneyMovementId: moneyMovementId
	});

	applyDataChanges(response.data);
}

export async function addMoneyMovement(categoryId, date, amount, note) {
	const data = {
		CategoryId: categoryId,
		Date: date,
		Amount: amount,
		Note: note
	};
	const response = await sendPost('AddMoneyMovement', data);
	if (response.status === 201 || response.status === 200) 
		applyDataChanges(response.data);
}

function applyDataChanges(changes) {
	updateStore(categoryStore, changes.categories, categoryExtractId, changes.deletedCategoryIds);
	updateStore(
		moneyMovementStore,
		changes.moneyMovements,
		moneyMovementExtractId,
		changes.deletedMoneyMovementIds
	);
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
