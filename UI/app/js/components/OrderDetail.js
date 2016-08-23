import React, {Component} from 'react';
import {RateItem, ListItem, SummaryLine} from './common/GoodsList';
import * as actions from '../actions/order';
import Page from './common/Page';
import Detail from './OrderConfirm/Detail';
import Comments from './OrderConfirm/Comments';
import OrderStatus from './common/OrderStatus';
import {Section, Line, Label, SmallButton} from './common/Widgets';

export default class OrderDetail extends Component {
    componentWillMount() {
        this.state = {
            order: null
        };
        this.updateOrder = this.updateOrder.bind(this);
        this.updateOrder();
    }

    updateOrder() {
        actions.getOrderDetail(this.props.id).then(order => {
            this.setState({
                order
            });
        });
    }

    cancelOrder() {
        const order = this.state.order;
        if (order.StatusCode != 0) {
            alert('请联系xxxxxx取消订单');
        } else {
            actions.cancelOrder(order.OrderId).then(this.updateOrder);
        }  
    }

    scroe() {
        const order = this.state.order;
        const data = {
            OrderId: order.OrderId,
            Rates: []
        }
        order.SubOrders.map(item => {
            data.Rates.push({
                GoodsId: item.GoodsId,
                Rate: item.Rate,
                SubId: item.Id
            });
        });
        actions.submitRates(data).then(this.updateOrder);
    }

    orderAgain() {
        const chart = {goods: {}};
        this.state.order.SubOrders.map(item => {
            chart.goods[item.GoodsId] = item
        });
        this.props.updateChart(chart, this.props.nextPage);
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
                                    rate={item.Rate}
                                    updateRate={(Rate) =>{
                                        if (!order.IsRated) {
                                            item.Rate = Rate;
                                            this.setState({order});
                                        }
                                    }}
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
                    <SummaryLine price={order.Amount}/>
                </Section>
                <Detail {...order}/>
                <Comments Comment={order.Comment}/>
                <Section className='align-end'>
                {order.StatusCode < 4 && 
                    <SmallButton label='取消订单'
                        onClick={this.cancelOrder.bind(this)}/>
                }
                {order.StatusCode > 4 && 
                    <SmallButton label='重新下单'
                            onClick={this.orderAgain.bind(this)}/>
                }
                {order.StatusCode == 6 && !order.IsRated &&
                    <SmallButton label='提交评价'
                            onClick={this.scroe.bind(this)}/>}
                </Section>
            </Page>
        );
    }
}