import Rest from '../utils/Rest';
import request from 'superagent';
import  * as Constants from '../Constants';

const basicUrl = `${Constants.basicUrl}/api/`;
const typeURL = 'tags';
const goodURL = 'goods/郑州东/0';
const submmitURL = 'Orders/add';
const orderListURL = 'orders/Query/All/test_Open_Id';
const orderURL = 'orders/Query/Order/';

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

export const submmitOrder = (data) => {
    return request.post(basicUrl + submmitURL, data)
        .then((result) => {
            console.log('order committed!');
        }).catch(err => {
            console.log('submmit failed!');
            console.log(err.message);
        });
}

export const getOrderList = () => {
    return request.get(basicUrl + orderListURL)
        .then(res => res.body && res.body.Orders)
        .catch(err => {
            console.log('Can not get order history!');
            console.log(err.message);
        });
}

export const getOrderDetail = (orderId) => {
    return request.get(basicUrl + orderURL + orderId)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get order detail!');
            console.log(err.message);
        });
}