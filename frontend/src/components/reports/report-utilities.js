export function getAlternatingColor(index) {
    return index % 2 == 0 ? 'bg-slate-300' : 'white';
}

// returns green color if the balance is positive and red if negative
export function getBalanceColor(balance){
    if (balance == undefined)
        return 'bg-white'
    return balance >= 0 ? 'bg-green-300' : 'bg-red-300';
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

export function calcSumByMonth(moneyMovements){
    let sum = {};

    moneyMovements.forEach(_ => {
        const date = new Date(_.date);
        const month= `${date.getMonth()+1}-${date.getFullYear()}`
        if (sum[month] == undefined)
            sum[month] = _.amount;
        else
            sum[month] += _.amount;
    });

    return sum;
}