import React, {Component} from 'react';
import {Section, Line, Label, SmallButton} from './Widgets';
import {ListItem, SummaryLine, OrderListNoImg} from './GoodsList';

const displayTime = (OrderTime) => OrderTime.replace('T', ' ');

const displayAddress = (TrainNumber, CarriageNumber) => `${TrainNumber} ${CarriageNumber}号餐车`;
const OrderDetail = ({Contact, ContactTel, TrainNumber, CarriageNumber, title, OrderId, Comment, OrderTime }) => {

  return (
    <Section title={title} list={true}>
        <Line align='start'>
          <Label>订单详情：</Label>
        </Line>
          {[{label: '订单号', value: OrderId}
            ,{label: '下单时间', value: displayTime(OrderTime)}
            ,{label: '联系人', value: Contact}
            ,{label: '联系方式', value: ContactTel}
            ,{label: '收货地址', value: displayAddress(TrainNumber, CarriageNumber)}
            ,{label: '备注', value: Comment}].map((item, index) => (
              <Line className='short' key={index}>
                <Label>{item.label + ':'}</Label>
                <Label size='auto'>{item.value}</Label>
              </Line>
            ))
          }

    </Section>
  );
};

export const Detail = ({OrderStatus, Amount, SubOrders, TrainNumber, CarriageNumber, button, OrderTime }) => {
  const time = new Date(OrderTime);

  return (
        <Section list={true} className='short'>
          <Line>
              <Label flex={true}>
                <span>{OrderStatus}</span>
                <span className='time'>{displayTime(OrderTime)}</span>
              </Label>     
            <Label align='end'>{displayAddress(TrainNumber, CarriageNumber)} </Label>
          </Line>
          <OrderListNoImg total={Amount} short={true}
              list={SubOrders} totalLabel='合计'/>
          {button}
      </Section>
  );
};

export default OrderDetail;
