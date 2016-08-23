import request from 'superagent';
import  {basicUrl, updateOrderURL} from '../constants/actions';

const udpateOrder = (orderId, data) => {
    return request.post(basicUrl + updateOrderURL + orderId, data)
        .then(res => res.body)
        .catch(err => {
            throw err;
        });
};

export const takeOrder = (orderId) => {
    const data = { "NewStatus":2 , "OldStatus":1}
    return udpateOrder(orderId, data);
}

export const orderReady = (orderId) => {
    const data = { "NewStatus":3 , "OldStatus":2}
    return udpateOrder(orderId, data);
}

export const expressOrder = (orderId) => {
    const data = { "NewStatus":4 , "OldStatus":3}
    return udpateOrder(orderId, data);
}

export const doneDeliver = (orderId) => {
    const data = { "NewStatus":5 , "OldStatus":4}
    return udpateOrder(orderId, data);
}