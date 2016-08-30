import React, {Component} from 'react';
import {Section, Line, Label, SmallButton} from './Widgets';
import {ListItem, SummaryLine, OrderListNoImg} from './GoodsList';

const displayAddress = (TrainNumber, CarriageNumber) => `${TrainNumber} ${CarriageNumber}号餐车`;
const OrderDetail = ({Contact, ContactTel, TrainNumber, CarriageNumber, title, OrderId, Comment, OrderTime }) => {

  return (
    <Section title={title} list={true}>
        <Line align='start'>
          <Label>订单详情：</Label>
        </Line>
          {[{label: '订单号', value: OrderId}
            ,{label: '下单时间', value: OrderTime.replace('T', ' ')}
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

export class CountDown extends Component {
  componentWillMount() {
    this.state = {
      time: ''
    }
  }

  componentDidMount() {
    this.timeout = setInterval(this.getTimeLeft.bind(this), 1000);
  }

  componentWillUnmount() {
    clearInterval(this.timeout);
  }

  getTimeLeft() {
    const expire = new Date(this.props.ExpiredTime);
    const now = new Date();
    let time = (expire - now)/1000;

    if (time > (8 * 60 * 60 - 15 * 60)) {
      time = time - 8 * 60 * 60;
    }
    const min = '0' +  Math.floor(time/60);
    const second = '0' + Math.floor(time % 60);
    this.setState({time: `${min.substr(-2, 2)}:${second.substr(-2, 2)}`});
  }

  render() {
    return <span className='time'>{this.state.time}</span>;
  }
}

export const Detail = ({OrderStatus, Amount, SubOrders, TrainNumber, CarriageNumber, button, ExpiredTime, OrderDate}) => {
  let date;
  if (OrderStatus == '待付款') {
    date = <CountDown ExpiredTime={ExpiredTime}/>;
  } else {
    date = <span className='time'>{OrderDate}</span>;
  }
  return (
        <Section list={true} className='short'>
          <Line>
              <Label flex={true}>
                <span>{OrderStatus}</span>
                {date}
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
