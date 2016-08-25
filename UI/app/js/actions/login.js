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

