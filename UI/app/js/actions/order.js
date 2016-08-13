import Rest from '../utils/Rest';
import request from 'superagent';

const hostname = '123.207.164.202';
const basicUrl = `http://${hostname}/TrainService/api/`
const typeURL = 'tags'
const goodURL = 'goods/郑州东/0'

export const getTypes = () => {
    return request.get(basicUrl + typeURL)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get tags');
            console.log(err.message);
        });
}

export const getGoodsList = (type) => {
    return request.get(encodeURI(basicUrl + goodURL))
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get goods list');
            console.log(err.message);
        });
}

export const getOrderList = () => {
    return new Promise((resolve) => {
        const orderList = [{
            shopName: '饭店',
            type: '外卖',
            totalPrice: 30,
            orderTime: '2016-01-11 22:54:12',
            status: '待付款',
            fee: 0,
            list: [{
                "GoodsId": 3,
                "Name": "鱼香肉丝盖浇饭",
                "SellPrice": 15,
                count: 1,
                PictureUrl: 'url'
            }, {
                "GoodsId": 3,
                "Name": "猪肉水饺",
                "SellPrice": 15,
                count: 1,
                PictureUrl: 'url'
            }]
        }, {
            shopName: '饭店',
            type: '外卖',
            totalPrice: 20,
            orderTime: '2016-01-11 22:54:12',
            status: '已付款',
            fee: 5,
            list: [{
                "GoodsId": 3,
                "Name": "鱼香肉丝盖浇饭",
                "SellPrice": 15,
                count: 1,
                PictureUrl: 'url'
            }]
        },{
            shopName: '饭店',
            type: '外卖',
            totalPrice: 30,
            orderTime: '2016-01-11 22:54:12',
            status: '待付款',
            fee: 0,
            list: [{
                "GoodsId": 3,
                "Name": "鱼香肉丝盖浇饭",
                "SellPrice": 15,
                count: 1,
                PictureUrl: 'url'
            }, {
                "GoodsId": 3,
                "Name": "猪肉水饺",
                "SellPrice": 15,
                count: 1,
                PictureUrl: 'url'
            }]
        }, {
            shopName: '饭店',
            type: '外卖',
            totalPrice: 20,
            orderTime: '2016-01-11 22:54:12',
            status: '已付款',
            fee: 5,
            list: [{
                "GoodsId": 3,
                "Name": "鱼香肉丝盖浇饭",
                "SellPrice": 15,
                count: 1,
                PictureUrl: 'url'
            }]
        }]
        resolve(orderList);
    });
}