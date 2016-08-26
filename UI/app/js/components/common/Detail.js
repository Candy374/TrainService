import React, {Component} from 'react';
import {Section, Line, Label} from './Widgets';

const OrderDetail = ({Contact, ContactTel, TrainNumber, CarriageNumber, title, OrderId, Comment, OrderTime }) => {
  const time = new Date(OrderTime);

  return (
    <Section title={title} list={true}>
        <Line align='start'>
          <Label>订单详情：</Label>
        </Line>
          {[{label: '订单号', value: OrderId}
            ,{label: '下单时间', value: `${time.toLocaleDateString()} ${time.toLocaleTimeString()}`}
            ,{label: '联系人', value: Contact}
            ,{label: '联系方式', value: ContactTel}
            ,{label: '收货地址', value: `${TrainNumber} ${CarriageNumber}号餐车`}
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

export default OrderDetail;
