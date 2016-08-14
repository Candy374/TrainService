import React, {Component} from 'react';
import {Section, Line, Label} from '../common/Widgets';

export default class OrderDetail extends Component {    
    render() {
        const {Contact, ContactTel, TrainNumber, CarriageNumber } = this.props;
        
        const userAddress = `${TrainNumber} ${CarriageNumber}号餐车`;
        return (
            <Section title='联系人信息'>
                <Line><Label>联系人：</Label>{Contact}</Line>
                <Line><Label>联系方式：</Label>{ContactTel}</Line>
                <Line><Label>收货地址：</Label>{userAddress}</Line>
            </Section>
        );
    }
}