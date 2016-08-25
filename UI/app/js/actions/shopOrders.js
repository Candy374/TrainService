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

// :
// 第一个是http://123.207.164.202:8080/trainservice/api/goods/%E9%83%91%E5%B7%9E%E4%B8%9C/0
// :
// 第二个是 http://123.207.164.202:8080/TrainService/api/orders/Query/All/test_Open_Id
// :
// 另外按照ProviderId查询商家信息的API是
// :
// http://123.207.164.202:8080/TrainService/api/Provider/Find/ID/2
// :
// 也可以按名字查询 http://123.207.164.202:8080/TrainService/api/Provider/Find/Name/%E7%A7%A6%E6%B1%89%E8%80%81%E7%A2%97
// :
// api/Orders/Update/SubOrder/{subOrderId}
// Post的requestBody是{"NewStatus":2,"OldStatus":1}