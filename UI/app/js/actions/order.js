export const getOrders = () => {
    return new Promise((resolve) => {
        //get orders from server

        const foodList = [{
            name: "hot",
            id: 't01',
            detail : [{
                id: 'f01',
                name: "hot food1",
                price: 10,
                sale: 20,
                rate: '89',
                img: './url'
            },{
                id: 'f02',
                name: "hot food2",
                price: 30,
                sale: 1,
                rate: '60',
                img: './url'
            }]
        },
        {
            name: "meet",
            id: 't02',
            detail : [{
                id: 'f03',
                name: "meet food1",
                price: 40,
                sale: 14,
                rate: '78',
                img: './url'
            },{
                id: 'f04',
                name: "meet food2",
                price: 50,
                sale: 1,
                rate: '60',
                img: './url'
            }]
        }];
        resolve(foodList);
    });
}