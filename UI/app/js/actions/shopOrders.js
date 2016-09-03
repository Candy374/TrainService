import request from 'superagent';
import  {basicUrl, updateOrderURL, goodsURL, log} from '../constants/actions';

const updateOrder = (order, data) => {
    data.SubOrderIds = order.SubOrders.map(subOrder => subOrder.Id);
    return request.post(basicUrl + updateOrderURL, data)
        .then(res => res.body);
};

export const takeOrder = (order, openId) => {
    const data = { NewStatus:2 , OldStatus:1, OpenId: openId};
    return updateOrder(order, data)
        .catch(err => {
            log('take order failed!');
            log(err.message);
        });
};

export const orderReady = (order, openId) => {
    const data = { "NewStatus":3 , "OldStatus":2, OpenId: openId};
    return updateOrder(order, data)
        .catch(err => {
            log('take order failed!');
            log(err.message);
        });
};

export const expressOrder = (order, openId) => {
    const data = { "NewStatus":4 , "OldStatus":3, OpenId: openId};
    return updateOrder(order, data)
        .catch(err => {
            log('express order failed!');
            log(err.message);
        });
};

export const doneDeliver = (order, openId) => {
    const data = { "NewStatus":5 , "OldStatus":4, OpenId: openId};
    return updateOrder(order, data)
        .catch(err => {
            log('done deliver failed!');
            log(err.message);
        });
};

export const doneOrder = (order, openId) => {
  const data = { "NewStatus":6 , "OldStatus":5, OpenId: openId};
  return updateOrder(order, data)
      .catch(err => {
            log('done order failed!');
            log(err.message);
        });
};

export const getProviderId = (code) => {
  return request.get(basicUrl + `Provider/Find/Code/${code}`)
    .then(res => res.body)
    .catch(err => {
      alert('服务器出现错误， 请重新打开');
    });
};

export const getWaitingOrder = (providerId) => {
  return request.get(basicUrl + `Orders/Query/ProviderId/${providerId}/Status/1`)
    .then(res => res.body)
    .catch(err => {
      console.log('get waiting order failed');
    });
};

export const getReadyOrder = (providerId) => {
  return request.get(basicUrl + `Orders/Query/ProviderId/${providerId}/Status/2`)
    .then(res => res.body)
    .catch(err => {
      console.log('get ready order failed');
    });
};

export const getDeliverReadyOrder = () => {
  return request.get(basicUrl + `Orders/Query/ProviderId/ALL/Status/3`)
    .then(res => res.body)
    .catch(err => {
      console.log('get deliver ready order failed');
    });
};

export const getDeliverDoneOrders = () => {
  return request.get(basicUrl + `Orders/Query/ProviderId/ALL/Status/4`)
    .then(res => res.body)
    .catch(err => {
      console.log('get deliver done order failed');
    });
};

// :
// 第一个是http://123.207.164.202:8080/trainservice/api/goods/%E9%83%91%E5%B7%9E%E4%B8%9C/0
// :
// 第二个是 http://123.207.164.202:8080/TrainService/api/orders/Query/All/test_Open_Id
// :
// 另外按照ProviderId查询商家信息的API是
// :
// http://123.207.164.202:8080/TrainService/api/Provider/Find/ID/2
// http://123.207.164.202:8080/TrainService/api/Provider/Find/Code/2
// 使用login 界面的 code， 如果返回值大于0 ， 就是商家的id， 小于0 就是没有权限
// :
// 也可以按名字查询 http://123.207.164.202:8080/TrainService/api/Provider/Find/Name/%E7%A7%A6%E6%B1%89%E8%80%81%E7%A2%97
// :
// api/Orders/Update/SubOrder/{subOrderId}
// Post的requestBody是{"NewStatus":2,"OldStatus":1}


// api/Orders/Update/Order/{orderId}/OpenId/{openId}

// Update/Order/{orderId}/OpenId/{newOpenId}