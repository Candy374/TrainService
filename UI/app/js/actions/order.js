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

export const getTypes = () => {

}

export const getGoodsList = (type) => {
    return new Promise((resolve) => {
        const foodList =[
            {
                "GoodsId": 3,
                "Name": "鱼香肉丝盖浇饭",
                "PurchasePrice": 15,
                "SellPrice": 18,
                "ProviderId": 1,
                "CanChangeFlavor": false,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/yuxiangrousi.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 50,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 4,
                "Name": "猪肉水饺",
                "PurchasePrice": 18,
                "SellPrice": 21,
                "ProviderId": 1,
                "CanChangeFlavor": false,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/jiaozi.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 80,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 5,
                "Name": "荠菜水饺 （废弃）",
                "PurchasePrice": 14,
                "SellPrice": 17,
                "ProviderId": 1,
                "CanChangeFlavor": false,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/jiaozi.jpg",
                "IsObsolete": true,
                "IsAvailable": false,
                "Rating": 30,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2,3",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 6,
                "Name": "黄焖鸡米饭（中分）",
                "PurchasePrice": 16,
                "SellPrice": 20,
                "ProviderId": 3,
                "CanChangeFlavor": false,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/huangmenji.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 60,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2,4",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 7,
                "Name": "黄焖鸡米饭（大份）",
                "PurchasePrice": 22,
                "SellPrice": 26,
                "ProviderId": 3,
                "CanChangeFlavor": false,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/huangmenji.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 55,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2,5",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 8,
                "Name": "白米饭",
                "PurchasePrice": 1,
                "SellPrice": 2,
                "ProviderId": 1,
                "CanChangeFlavor": false,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/mifan.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 80,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2,3,4",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 9,
                "Name": "陕西凉皮",
                "PurchasePrice": 8,
                "SellPrice": 10,
                "ProviderId": 2,
                "CanChangeFlavor": true,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/liangpi.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 90,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2,3",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 10,
                "Name": "河南烩面",
                "PurchasePrice": 15,
                "SellPrice": 18,
                "ProviderId": 4,
                "CanChangeFlavor": true,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/huimian.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 80,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2,3,5",
                "StationCode": "郑州东"
            },
            {
                "GoodsId": 11,
                "Name": "川味回锅肉",
                "PurchasePrice": 16,
                "SellPrice": 19,
                "ProviderId": 4,
                "CanChangeFlavor": true,
                "PictureUrl": "http://123.207.164.202:8080/trainservice/imgs/huiguorou.jpg",
                "IsObsolete": false,
                "IsAvailable": true,
                "Rating": 72,
                "LinkedGoodsId": null,
                "OrderCount": 0,
                "GoodsType": 0,
                "Tags": "0,1,2,3",
                "StationCode": "郑州东"
            }
        ];

        let a = [{"GoodsId":3,"Name":"鱼香肉丝盖浇饭","SellPrice":18.0,"ProviderId":1,"CanChangeFlavor":false,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/yuxiangrousi.jpg","Rating":50,"OrderCount":0,"Tags":[0]},{"GoodsId":4,"Name":"猪肉水饺","SellPrice":21.0,"ProviderId":1,"CanChangeFlavor":false,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/jiaozi.jpg","Rating":80,"OrderCount":0,"Tags":[0,1,2]},{"GoodsId":6,"Name":"黄焖鸡米饭（中分）","SellPrice":20.0,"ProviderId":3,"CanChangeFlavor":false,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/huangmenji.jpg","Rating":60,"OrderCount":0,"Tags":[0,1,2,4]},{"GoodsId":7,"Name":"黄焖鸡米饭（大份）","SellPrice":26.0,"ProviderId":3,"CanChangeFlavor":false,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/huangmenji.jpg","Rating":55,"OrderCount":0,"Tags":[0,1,2,5]},{"GoodsId":8,"Name":"白米饭","SellPrice":2.0,"ProviderId":1,"CanChangeFlavor":false,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/mifan.jpg","Rating":80,"OrderCount":0,"Tags":[0,1,2,3,4]},{"GoodsId":9,"Name":"陕西凉皮","SellPrice":10.0,"ProviderId":2,"CanChangeFlavor":true,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/liangpi.jpg","Rating":90,"OrderCount":0,"Tags":[0,1,2,3]},{"GoodsId":10,"Name":"河南烩面","SellPrice":18.0,"ProviderId":4,"CanChangeFlavor":true,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/huimian.jpg","Rating":80,"OrderCount":0,"Tags":[0,1,2,3,5]},{"GoodsId":11,"Name":"川味回锅肉","SellPrice":19.0,"ProviderId":4,"CanChangeFlavor":true,"PictureUrl":"http://123.207.164.202:8080/trainservice/imgs/huiguorou.jpg","Rating":72,"OrderCount":0,"Tags":[0,1,2,3]}]
        resolve(foodList);
    });
}