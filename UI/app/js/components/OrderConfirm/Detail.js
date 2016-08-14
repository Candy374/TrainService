import React, {Component} from 'react';
import {Section, Line, Label} from '../common/Widgets';

export default class OrderDetail extends Component {
    generateOrderNum() {
        const orderNum = 'DC201608110753';
        return orderNum;
    }
    
    render() {
        const {Contact, ContactTel, TrainNumber, CarriageNumber } = this.props;
        
        const userAddress = `${TrainNumber} ${CarriageNumber}号餐车`;
        return (
            <Section>
                <Line className='head'>
                    <div className='title'>订单详情：</div>
                    <Label size='large' align='end'>{this.generateOrderNum()}</Label>
                </Line>
                <Line><Label>联系人：</Label>{Contact}</Line>
                <Line><Label>联系方式：</Label>{ContactTel}</Line>
                <Line><Label>收货地址：</Label>{userAddress}</Line>
            </Section>
        );
    }
}