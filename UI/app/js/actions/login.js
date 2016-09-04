import request from 'superagent';
import  {basicUrl, log} from '../constants/actions';

export const login = (code, state) => {
    return request.get(basicUrl + `/User/Info/Code/${code}/State/${state}`)
        .then(res => res.body)
        .catch(err => {
            log('login failed');
            log(err)
            throw err;
        });
};


export const updateOpenId = (orderId) => {
    return request.post(basicUrl + 'Update/Order/${orderId}/OpenId/{newOpenId}')
        .then((res) => {
            return res.body
        })
        .catch(err => {
            log('can not update openId');
            log(err.message);
        });
};
