import Rest from '../utils/Rest';
import request from 'superagent';
import  {basicUrl, typeURL, goodsURL, userURL,
    submmitURL,orderListURL, orderURL, stationsURL} from '../constants/actions';

export const getTypes = () => {
    return request.get(basicUrl + typeURL)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get tags');
            console.log(err.message);
        });
}

export const getGoodsList = (type) => {
    return request.get(encodeURI(basicUrl + goodsURL))
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

export const getOrderList = (userId) => {
    userId = 'test_Open_Id';
    return request.get(basicUrl + orderListURL + userId)
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

export const getStations = () => {
    return request.get(basicUrl + stationsURL)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get station list!');
            console.log(err.message);
        });
}

export const getUserInfo = (userId) => {
    userId = 124123;
    return request.get(basicUrl + userURL + userId)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get user info!');
            console.log(err.message);
        });
}