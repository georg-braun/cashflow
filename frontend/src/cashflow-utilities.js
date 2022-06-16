import { categoryStore } from './store.js';
import { get } from 'svelte/store';

export function getCategoryName(id){
    const categories = get(categoryStore);
    
    const result = categories.find(_ => _.id == id);
    return result.name ?? "";
}

export function showMoneyValue(value){
    if (value === undefined)
        return "~";
    return `${value.toFixed(2)} €`;
}