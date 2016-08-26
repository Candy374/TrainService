import request from 'superagent';
import  {basicUrl, typeURL, goodsURL, userURL, cancelURL, rateURL,
  submitURL,orderListURL, orderURL, stationsURL} from '../constants/actions';

export const getTypes = () => {
    return request.get(basicUrl + typeURL)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get tags');
            console.log(err.message);
        });
};

export const getGoodsList = () => {
    return request.get(encodeURI(basicUrl + goodsURL))
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get goods list');
            console.log(err.message);
        });
};

export const redirect = (orderId) => {
    location.href = `https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxaf1fff843c641aba&redirect_uri=http%3A%2F%2Ftrainservice.techotaku.net%2F%23Login%2F&response_type=code&scope=snsapi_userinfo&state=ReLogin_${orderId}#wechat_redirect`        
};

export const submitOrder = (data) => {
    if (!data.OpenId) {
      data.OpenId = 'TBD';
    }
    return request.post(basicUrl + submitURL, data)
        .then((res) => {
            return res.body;
        })
        .catch(err => {
            console.log('submit failed!');
            console.log(err.message);
        });
};

export const getOrderList = (userId) => {
    alert('openId is ' + userId);
    return request.get(basicUrl + orderListURL + userId)
        .then(res => res.body && res.body.Orders)
        .catch(err => {
            console.log('Can not get order history!');
            console.log(err.message);
        });
};

export const getOrderDetail = (orderId) => {
    return request.get(basicUrl + orderURL + orderId)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get order detail!');
            console.log(err.message);
        });
};

export const getStations = () => {
    return request.get(basicUrl + stationsURL)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get station list!');
            console.log(err.message);
        });
};

export const getUserInfo = (userId) => {
    return request.get(basicUrl + userURL + userId)
        .then(res => res.body)
        .catch(err => {
            console.log('Can not get user info!');
            console.log(err.message);
        });
};

export const cancelOrder = (orderId) => {
     return request.post(basicUrl + cancelURL + orderId)
        .then(res => res.body)
        .catch(err => {
            console.log('Cancel order failed!');
            console.log(err.message);
        });
};

export const submitRates = (data) => {
     return request.post(basicUrl + rateURL, data)
        .then(res => res.body)
        .catch(err => {
            console.log('submit rate failed!');
            console.log(err.message);
        });
};