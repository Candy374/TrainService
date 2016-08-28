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

const pay = ({appId, timeStamp, nonceStr, _package, signType, paySign}, callback) => {
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
                alert(res.err_msg);
                callback(res.err_msg == "get_brand_wcpay_request：ok"); // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。 
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
}

const getPayArgs = (OrderId, callback) => {
    const Ip = returnCitySN.cip.replace(/\./g, '_');

    return request.post(basicUrl + `Pay/Order/${OrderId}/IP/${Ip}`)
        .then((res) => {
            const args = JSON.parse(res.body);
            args._package = args.package;
            pay(args, callback.bind(this, OrderId));
        })
        .catch(err => {
            console.log('can not get pay args');
            console.log(err.message);
        });
}

export const submitOrder = (data, callback) => {
    if (!data.OpenId) {
      data.OpenId = 'TBD';
    }
    return request.post(basicUrl + submitURL, data)
        .then((res) => {
            return getPayArgs(res.body, callback);
        })
        .catch(err => {
            console.log('submit failed!');
            console.log(err.message);
        });
};

export const getOrderList = (userId) => {
    // alert('openId is ' + userId);
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