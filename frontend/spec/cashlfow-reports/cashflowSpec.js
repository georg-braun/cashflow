import { calcSumByCategory, calcSumByMonthByCategory } from "../../src/cashflow-utilities.js"

describe("cashflow-reports", function () {

	it("calculates correct category sum", function () {
		// arrange
		const data = getTestScenario();

		// act
		const sumByCategory = calcSumByCategory(data.moneyMovements);

		// assert
		const expectedResult = {
			'77fff9d0-0dc8-4524-bdc1-49cf1565f405': -150,
			'65073f9d-36eb-426a-9411-8ba0cc790bec': -190
		};

		expect(sumByCategory).toEqual(expectedResult);
	});

	it("calculates correct category sum per month", function () {
		// arrange
		const data = getTestScenario();

		// act
		const sumByMonthByCategory = calcSumByMonthByCategory(data.moneyMovements);

		// assert
		const expectedResult = {
			'77fff9d0-0dc8-4524-bdc1-49cf1565f405': {'5-2022': -150},
			'65073f9d-36eb-426a-9411-8ba0cc790bec': {'5-2022': -100, '4-2022': -90}
		};

		expect(sumByMonthByCategory).toEqual(expectedResult);
	});


});

function getTestScenario() {
	return {
		"moneyMovements": [
			{
				"id": "a99b23f6-a8e0-4586-b3ec-b01dcb3c3a3c",
				"amount": -50,
				"date": "2022-05-17T22:00:00Z",
				"note": "Tires",
				"categoryId": "77fff9d0-0dc8-4524-bdc1-49cf1565f405"
			},
			{
				"id": "94ea2b46-48f0-48dc-bf92-df588e89442f",
				"amount": -100,
				"date": "2022-05-17T22:00:00Z",
				"note": "Fuel",
				"categoryId": "77fff9d0-0dc8-4524-bdc1-49cf1565f405"
			},
			{
				"id": "1b38fc03-401b-4b35-b2ed-88299a40c9e9",
				"amount": -100,
				"date": "2022-05-17T22:00:00Z",
				"note": "Something to eat",
				"categoryId": "65073f9d-36eb-426a-9411-8ba0cc790bec"
			},
			{
				"id": "46b001ad-6e9c-4863-a4bc-a467d8452ea8",
				"amount": -90,
				"date": "2022-04-18T22:00:00Z",
				"note": "Something to eat",
				"categoryId": "65073f9d-36eb-426a-9411-8ba0cc790bec"
			}
		],
		"categories": [
			{
				"id": "77fff9d0-0dc8-4524-bdc1-49cf1565f405",
				"name": "Car"
			},
			{
				"id": "65073f9d-36eb-426a-9411-8ba0cc790bec",
				"name": "Groceries"
			}
		]
	}
}