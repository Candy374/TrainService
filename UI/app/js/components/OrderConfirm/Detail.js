import React, {Component} from 'react';

export default class OrderDetail extends Component {
    generateOrderNum() {
        const orderNum = 'DC201608110753';
        return orderNum;
    }
    
    render() {
        const {Contact, ContactTel, TrainNumber, CarriageNumber } = this.props;
        
        const userAddress = `${TrainNumber} ${CarriageNumber}号餐车`;
        return (
            <div className='section'>
                <div className='head'>
                    <div className='title'>订单详情：</div>
                    <div className='width-large rightToLeft'>
                    {this.generateOrderNum()}
                    </div>
                </div>
                <div className='user-info'>
                    <div className='line'>
                    <span className='width-medium'>联系人：</span>{Contact}</div>
                    <div className='line'>
                    <span className='width-medium'>联系方式：</span>{ContactTel}</div>
                    <div className='line'>
                    <span className='width-medium'>收货地址：</span>{userAddress}</div>
                </div>
            </div>
        );
    }
}