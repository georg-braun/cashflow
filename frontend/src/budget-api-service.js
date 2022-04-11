import axios from "axios";

import auth from "./auth-service";

const serverUrl = import.meta.env.VITE_BUDGET_API_SERVER

async function makeRequest(config) {
    try {
        const token = await auth.getAccessToken();
        config.headers = {
            ...config.headers,
            Authorization: `Bearer ${token}`,
        };

        console.log(`initiate request`)
        console.log(config)

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
            method: "POST",
            headers: {
                "content-type": "application/json",
                Authorization: `Bearer ${token}`
            }
        }
        console.log(`initiate request`)
        console.log(config)

        const response = axios.post(`${serverUrl}/api/${endpoint}`, data, config);
        return response;
    } catch (error) {
        console.log(error);
    }
}



export async function getAccounts() {
    const config = {
        url: `${serverUrl}/api/GetAllAccounts`,
        method: "GET",
        headers: {
            "content-type": "application/json",
        }
    }

    console.log("Get all accounts");
    return makeRequest(config);
}

export async function addAccount() {
    await sendPost("AddAccount", {
        Name: "Cash"
    });
}

export async function deleteAccount(accountId) {
    await sendPost("DeleteAccount", {
        AccountId: accountId
    });
}

export async function addIncome(accountId, date, amount){
    const data =  {
        AccountId: accountId,
        Date: date,
        Amount: amount
    };
    console.log(`AddIncome: ${data}`)
    await sendPost("AddIncome", data);
}


