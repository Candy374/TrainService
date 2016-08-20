import request from 'superagent';
import  {basicUrl, typeURL, goodsURL, userURL, cancelURL, rateURL,
    submmitURL,orderListURL, orderURL, stationsURL} from '../constants/actions';

export const takeOrder = (orderId) => {
    // return request.get(basicUrl + typeURL + orderId)
    //     .then(res => res.body)
    //     .catch(err => {
    //         alert('Can not take order');
    //         console.log(err.message);
    //     });

    return request.get(encodeURI(basicUrl + goodsURL))
        .then(res => {
            const list = res.body
            const result = list.filter(item => item.OrderId == orderId);
            result[0].StatusCode = result[0].StatusCode + 1;
            return result;
        })
        .catch(err => {
            alert('Can not get goods list');
            console.log(err.message);
        });
}

export const orderReady = (orderId) => {
    // return request.get(basicUrl + typeURL + orderId)
    //     .then(res => res.body)
    //     .catch(err => {
    //         alert('Can not take order');
    //         console.log(err.message);
    //     });

    return request.get(encodeURI(basicUrl + goodsURL))
        .then(res => {
            const list = res.body
            const result = list.filter(item => item.OrderId == orderId);
            result[0].StatusCode = result[0].StatusCode + 1;
            return result;
        })
        .catch(err => {
            alert('Can not get goods list');
            console.log(err.message);
        });
}

export const expressOrder = (orderId) => {
    // return request.get(basicUrl + typeURL + orderId)
    //     .then(res => res.body)
    //     .catch(err => {
    //         alert('Can not take order');
    //         console.log(err.message);
    //     });

    return request.get(encodeURI(basicUrl + goodsURL))
        .then(res => {
            const list = res.body
            const result = list.filter(item => item.OrderId == orderId);
            result[0].StatusCode = result[0].StatusCode + 1;
            return result;
        })
        .catch(err => {
            alert('Can not get goods list');
            console.log(err.message);
        });
}