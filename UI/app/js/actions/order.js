import Rest from '../utils/Rest';
import request from 'superagent';
import  {basicUrl, typeURL, goodsURL, userURL, cancelURL,
    submmitURL,orderListURL, orderURL, stationsURL} from '../constants/actions';

export const getTypes = () => {
    return request.get(basicUrl + typeURL)
        .then(res => res.body)
        .catch(err => {
            alert('Can not get tags');
            console.log(err.message);
        });
}

export const getGoodsList = (type) => {
    return request.get(encodeURI(basicUrl + goodsURL))
        .then(res => res.body)
        .catch(err => {
            alert('Can not get goods list');
            console.log(err.message);
        });
}

export const submmitOrder = (data) => {
    return request.post(basicUrl + submmitURL, data)
        .then((res) => {
            return res.body;
        }).catch(err => {
            alert('submmit failed!');
            console.log(err.message);
        });
}

export const getOrderList = (userId) => {
    return request.get(basicUrl + orderListURL + userId)
        .then(res => res.body && res.body.Orders)
        .catch(err => {
            alert('Can not get order history!');
            console.log(err.message);
        });
}

export const getOrderDetail = (orderId) => {
    return request.get(basicUrl + orderURL + orderId)
        .then(res => res.body)
        .catch(err => {
            alert('Can not get order detail!');
            console.log(err.message);
        });
}

export const getStations = () => {
    return request.get(basicUrl + stationsURL)
        .then(res => res.body)
        .catch(err => {
            alert('Can not get station list!');
            console.log(err.message);
        });
}

export const getUserInfo = (userId) => {
    userId = 124123;
    return request.get(basicUrl + userURL + userId)
        .then(res => res.body)
        .catch(err => {
            alert('Can not get user info!');
            console.log(err.message);
        });
}

export const cancelOrder = (orderId) => {
     return request.post(basicUrl + cancelURL + orderId)
        .then(res => res.body)
        .catch(err => {
            alert('Cancel order failed!');
            console.log(err.message);
        });
}