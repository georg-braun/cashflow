import { categoryStore } from './store.js';
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

export function calcSumByMonthByCategory(moneyMovements){
    let sum = {};

    moneyMovements.forEach(_ => {
        const date = new Date(_.date);
        const month= `${date.getMonth()+1}-${date.getFullYear()}`
        if (sum[_.categoryId] == undefined) 
            sum[_.categoryId] = {};
        if (sum[_.categoryId][month] == undefined)
            sum[_.categoryId][month] = _.amount;
        else
            sum[_.categoryId][month] += _.amount;
    });

    return sum;
}