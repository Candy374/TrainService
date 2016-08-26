import request from 'superagent';
import  {basicUrl} from '../constants/actions';

export const login = (code, state) => {
    alert('get open Id')
    return request.get(basicUrl + `/User/Info/Code/${code}/State/${state}`)
        .then(res => res.body)
        .catch(err => {
            throw err;
        });
}


export const renewOpenId = (orderId) => {
    return request.post(basicUrl + 'Update/Order/${orderId}/OpenId/{newOpenId}')
        .then((res) => {

        })
        .catch(err => {
            console.log('can not get openId');
            console.log(err.message);
        });
};

