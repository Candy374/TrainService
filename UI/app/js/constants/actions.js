import * as Constants from './system';

export const basicUrl = `${Constants.basicUrl}/api/`;
export const typeURL = 'Tags/';
export const goodsURL = 'goods/郑州东/0';
export const submitURL = 'Orders/add';
export const orderListURL = 'orders/Query/All/';
export const orderURL = 'orders/Query/Order/';
export const stationsURL = 'stations';
export const userURL = 'user/LastInput/';
export const cancelURL = 'Orders/Cancel/';
export const deleteURL = 'Orders/Delete/';
export const rateURL = 'Orders/Rate';
export const updateSubOrderURL = 'Orders/Update/SubOrder/';
export const updateOrderURL = 'Orders/Update/Order/';
const level = 'info';
export const log = (msg) => {
    if (level == 'alert') {
        alert(msg)
    } else {
        console.log(msg)
    }
    
};
