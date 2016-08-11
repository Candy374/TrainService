import React, {Component} from 'react';

export default class OrderDetail extends Component {
    render() {
        const orderNum = 'DC201608110753',
        userName = "张珊",
        userPhone = "13454322356",
        userAddress = "G103 动车 7号餐车";
        return (
            <div className='section'>
                <div className='head line'>
                    <div className='title'>订单详情：</div>
                    <div className='order-no'>
                    {orderNum}
                    </div>
                </div>
                <div className='user-info'>
                    <div className='line'>
                    <span className='label'>联系人：</span>{userName}</div>
                    <div className='line'>
                    <span className='label'>联系方式：</span>{userPhone}</div>
                    <div className='line'>
                    <span className='label'>收货地址：</span>{userAddress}</div>
                </div>
            </div>
        );
    }
}