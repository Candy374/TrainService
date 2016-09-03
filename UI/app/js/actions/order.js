import request from 'superagent';
import  {basicUrl, typeURL, goodsURL, userURL, cancelURL, rateURL, log
  submitURL,orderListURL, orderURL, stationsURL, deleteURL} from '../constants/actions';

export const getTypes = (goodsType = 1) => {
    return request.get(basicUrl + typeURL + goodsType)
        .then(res => res.body)
        .catch(err => {
            log('Can not get tags');
            log(err.message);
        });
};

export const getGoodsList = () => {
    return request.get(encodeURI(basicUrl + goodsURL))
        .then(res => res.body)
        .catch(err => {
            log('Can not get goods list');
            log(err.message);
        });
};

export const redirect = (orderId) => {
    location.href = `https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxaf1fff843c641aba&redirect_uri=http%3A%2F%2Ftrainservice.techotaku.net%2F%23Login%2F&response_type=code&scope=snsapi_userinfo&state=ReLogin_${orderId}#wechat_redirect`        
};

const pay = ({appId, timeStamp, nonceStr, _package, signType, paySign}, OrderId, callback) => {
    function onBridgeReady() {
        WeixinJSBridge.invoke(
            'getBrandWCPayRequest', {
                appId,     //公众号名称，由商户传入     
                timeStamp,    //时间戳，自1970年以来的秒数     
                nonceStr, //随机串     
                package: _package,     
                signType,         //微信签名方式：     
                paySign //微信签名 
            },
            function (res) {
                log('订单支付完成' + res.err_msg);
                callback(OrderId, res.err_msg == "get_brand_wcpay_request：ok"); // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
            }
        );
    }
    if (typeof WeixinJSBridge == "undefined") {
        if ( document.addEventListener ) {
            document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
        } else if (document.attachEvent) {
            document.attachEvent('WeixinJSBridgeReady', onBridgeReady); 
            document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
        }
    } else {
        onBridgeReady();
    }
};

export const getPayArgs = (OrderId, callback) => {
    const Ip = returnCitySN.cip.replace(/\./g, '_');
    return request.post(basicUrl + `Pay/Order/${OrderId}/IP/${Ip}`)
        .then((res) => {
            log(res.body);
            const args = JSON.parse(res.body);
            args._package = args.package;
            pay(args, OrderId, callback);
        })
        .catch(err => {
            log('can not get pay args');
            log(err.message);
        });
};

export const submitOrder = (data, callback) => {
    if (!data.OpenId) {
      data.OpenId = 'TBD';
    }
    return request.post(basicUrl + submitURL, data)
        .then((res) => {
            return getPayArgs(res.body, callback);
        })
        .catch(err => {
            log('submit failed!');
            log(err.message);
        });
};

export const getOrderList = (userId) => {
    return request.get(basicUrl + orderListURL + userId)
        .then(res => {
            const list = res.body && res.body.Orders;
            log('get order list: ' + list);
            const desc = list.map(item => `${item.OrderId} : ${item.StatusCode}`).join('\n');
            log(desc);
            return list;
        })
        .catch(err => {
            log('Can not get order history!');
            log(err.message);
        });
};

export const getOrderDetail = (orderId) => {
    return request.get(basicUrl + orderURL + orderId)
        .then(res => {
            log('get order detail' + res.body && res.body.StatusCode);
            return res.body
        })
        .catch(err => {
            log('Can not get order detail!');
            log(err.message);
        });
};

export const getStations = () => {
    return request.get(basicUrl + stationsURL)
        .then(res => res.body)
        .catch(err => {
            log('Can not get station list!');
            log(err.message);
        });
};

export const getUserInfo = (userId) => {
    return request.get(basicUrl + userURL + userId)
        .then(res => res.body)
        .catch(err => {
            log('Can not get user info!');
            log(err.message);
        });
};

export const cancelOrder = (orderId, openId) => {
     return request.post(basicUrl + cancelURL + openId + '/' + orderId)
        .then(res => {
            log('cancel order: ' + res.body && res.body.StatusCode);
            return res.body;
        })
        .catch(err => {
            log('Cancel order failed!');
            log(err.message);
        });
};

export const deleteOrder = (orderId, openId) => {
     return request.post(basicUrl + deleteURL + openId + '/' + orderId)
        .then(res =>{
            log('Delete order: ' + res.body);
            return res.body;
        })
        .catch(err => {
            log('Delete order failed!');
            log(err.message);
        });
};

export const submitRates = (data) => {
     return request.post(basicUrl + rateURL, data)
        .then(res => {
            log('submit rate: ' + res.body);
            return res.body;
        })
        .catch(err => {
            log('submit rate failed!');
            log(err.message);
        });
};

export const getTrainTime = (station_code, trainNumber) => {
    // Stations/{station_code}/TrainSchedule/{trainNumber}/ArriveTime
    return request.post(basicUrl + `Stations/${station_code}/TrainSchedule/${trainNumber}/TimeCheck`)
        .then(res => {
            log('train time: ' + res.body);
            return res.body;
        })
        .catch(err => {
            log('get train time failed!');
            log(err.message);
        });
};
