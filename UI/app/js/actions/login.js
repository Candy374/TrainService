import request from 'superagent';
import  {basicUrl} from '../constants/actions';

export const login = (code, state) => {
    // alert('get open Id');
    return request.get(basicUrl + `/User/Info/Code/${code}/State/${state}`)
        .then(res => res.body)
        .catch(err => {
            throw err;
        });
};


export const updateOpenId = (orderId) => {
    return request.post(basicUrl + 'Update/Order/${orderId}/OpenId/{newOpenId}')
        .then((res) => {
            return res.body
        })
        .catch(err => {
            console.log('can not update openId');
            console.log(err.message);
        });
};

export const getUserIp = (url) => {
    var xmlhttp;
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.open("GET", url || 'http://pv.sohu.com/cityjson?ie=utf-8', false);
    xmlhttp.send();
    console.log(xmlhttp.responseText);

    // return request.get(url || 'http://pv.sohu.com/cityjson?ie=utf-8')
    //     .then((res) => {
    //         return res.body;
    //     })
}
