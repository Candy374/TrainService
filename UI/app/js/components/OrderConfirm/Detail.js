import React, {Component} from 'react';
import {Section, Line, Label} from '../common/Widgets';

const OrderDetail = ({Contact, ContactTel, TrainNumber, CarriageNumber, title }) => (
    <Section title={title || '联系人信息'}>
        <Line align='start'><Label>联系人：</Label>{Contact}</Line>
        <Line align='start'><Label>联系方式：</Label>{ContactTel}</Line>
        <Line align='start'><Label>收货地址：</Label>{`${TrainNumber} ${CarriageNumber}号餐车`}</Line>
    </Section>
);

export default OrderDetail;
