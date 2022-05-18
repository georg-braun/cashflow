import { moneyMovementStore, categoryStore } from './store.js';
import { get } from 'svelte/store';

export function getCategoryName(id){
    const categories = get(categoryStore);
    
    const result = categories.find(_ => _.id == id);
    return result.name ?? "";
}

export function calcSumByCategory(moneyMovements){
    let sumByCategory = {};

    moneyMovements.forEach(_ => {
        if (sumByCategory[_.categoryId] == undefined)
            sumByCategory[_.categoryId] = _.amount;
        else
            sumByCategory[_.categoryId] += _.amount;
    });

    return sumByCategory;
}