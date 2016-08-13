import Rest from '../utils/Rest';
import request from 'superagent';

const hostname = '123.207.164.202';
const basicUrl = `http://${hostname}/TrainService/api/`;
const typeURL = 'tags';
const goodURL = 'goods/郑州东/0';
const orderURL = 'Orders/add';

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
    return request.post(basicUrl + orderURL, data).then((result) => {
        console.log('order committed!');
    });
}