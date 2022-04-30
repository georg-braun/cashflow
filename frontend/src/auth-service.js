import initializeAuth0Client from '@auth0/auth0-spa-js';
import { writable, get } from 'svelte/store';

export const isAuthenticated = writable(false);
export const popupOpen = writable(false);
export const error = writable();
export const user = writable({});
export const tasks = writable([]);
export const client = writable();

async function createClient() {
  let auth0Client = await initializeAuth0Client({    
		domain: import.meta.env.VITE_AUTH_DOMAIN,
		client_id: import.meta.env.VITE_AUTH_CLIENT_ID,
		audience: import.meta.env.VITE_AUTH_AUDIENCE,
		redirect_uri: window.location.origin		
	});

	client.set(auth0Client);
	isAuthenticated.set(await get(client).isAuthenticated());
	user.set(await get(client).getUser());
}

async function loginWithPopup(options) {
	popupOpen.set(true);
	try {
		await get(client).loginWithPopup(options);

		user.set(await get(client).getUser());
		isAuthenticated.set(true);
	} catch (e) {
		// eslint-disable-next-line
		console.error(e);
	} finally {
		popupOpen.set(false);
	}
}

function logout() {
	return get(client).logout();
}

async function getAccessToken() {
	return await get(client).getTokenSilently();
}

const auth = {
	createClient,
	loginWithPopup,
	logout,
	getAccessToken,
	user,
	isAuthenticated
};

export default auth;
