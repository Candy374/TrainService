export const getOrders = () => {
    return new Promise((resolve) => {
        //get orders from server

        const foodList = [{
            name: "hot",
            detail : [{
                name: "hot food1",
                price: 18,
                sale: 20,
                rate: '89',
                img: './url'
            },{
                name: "hot food2",
                price: 23,
                sale: 1,
                rate: '60',
                img: './url'
            }]
        },
        {
            name: "meet",
            detail : [{
                name: "meet food1",
                price: 20,
                sale: 14,
                rate: '78',
                img: './url'
            },{
                name: "meet food2",
                price: 23,
                sale: 1,
                rate: '60',
                img: './url'
            }]
        }];
        resolve(foodList);
    });
}