import React, {Component} from 'react';
import {RateItem, ListItem} from './common/GoodsList';
import * as actions from '../actions/order';
import Page from './common/Page';
import Detail from './OrderConfirm/Detail';
import Comments from './OrderConfirm/Comments';
import OrderStatus from './common/OrderStatus';
import {Section, Line, Label} from './common/Widgets';

export default class OrderDetail extends Component {
    componentWillMount() {
        this.state = {
            order: null
        };
        
        actions.getOrderDetail(this.props.id).then(order => {
            this.setState({
                order
            });
        });
    }

    render() {
        const order = this.state.order;
        if (!order) {
            return null;
        }

         const footer = order.StatusCode == 0 && {
                    button: {
                        label: '立即支付',
                        onClick: this.props.submmitOrder,
                        disabled: this.state.submitting
                    }
                };

        return (
            <Page footer={footer}>
                <OrderStatus status={order.StatusCode}/>
                <Section title='已点菜品' list={true}> 
                    {
                        order.SubOrders.map((item, index) => (
                            order.StatusCode == 6 ? (
                                <RateItem key={index}
                                    url={item.PicUrl}
                                    name={item.Name}
                                    count={item.Count}
                                    price={item.Price}/>)
                            : (<ListItem key={index}
                                    url={item.PicUrl}
                                    name={item.Name}
                                    count={item.Count}
                                    price={item.Price}/>))      
                        )
                    }
                    <Line>
                        <Label flex={true}/>
                        <Label size='small'>共计</Label>
                        <Label size='small' className='price'>{`￥${order.Amount}`}</Label>
                    </Line>
                </Section>
                <Detail {...order}/>
                <Comments Comment={order.Comment}/>
                {order.StatusCode < 4 && <button className='small' onClick={() => {
                    if (order.StatusCode != 0) {
                        alert('请联系xxxxxx取消订单');
                    } else {
                        actions.cancelOrder(order.OrderId).then((result) => {
                            if (result) {
                                this.props.setCurrentOrderId(order.OrderId);
                            }
                        });
                    }                         
                }}>取消订单</button>
                }
            </Page>
        );
    }
}