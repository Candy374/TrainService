import React, {Component} from 'react';
import * as actions from '../../actions/order';
import Page from '../common/Page';
import Footer from '../common/Footer';
import {ListItem} from '../common/GoodsList';
import  * as Constants from '../../constants/system';
import {Section, Line, Label, SmallButton} from '../common/Widgets';
import Detail from '../OrderConfirm/Detail';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            showAll: false,
            orders: []
        };
        actions.getOrderList(this.getUserId()).then((orderList)=>{
            if (orderList) {
                const orders = orderList.filter(order => order.StatusCode >= 3 && order.StatusCode <= 5)
                this.setState({
                    orders
                });
            }
        });
    }

    getUserId() {
        return '124123'
    }

    render() {
        const {orders, showAll} = this.state;
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
        const footer = {
            button: {
                label: this.state.showAll ? '只显示需取货订单' : '显示所有订单',
                onClick: () => this.setState({showAll: !this.state.showAll})
            }
        }
        return (
            <Page flex={true} direction='col' className='order-list' footer={footer}>
                <div className='content'>
                {orders.map((order, index) => {
                    if (!this.state.showAll && order.StatusCode != 3) {
                        return;
                    }
 
                    return (
                     <Section key={index}>
                        <Detail {...order} title={`订单号：${order.OrderId}`}/>
                        {order.showDetail &&
                            <Section list={true} title='商品信息'>                        
                                {order.SubOrders.map((item, index) => (
                                    <Line key={index}>
                                        <Label flex={true}>{item.Name}</Label>
                                        <Label size='small'>{`x${item.Count}`}</Label>
                                    </Line>)                               
                                )}

                                <Line>
                                    <Label flex={true}>{order.OrderDate}</Label>
                                    <Label size='small'>共计: </Label>
                                    <Label size='small'>￥{order.Amount}</Label>
                                </Line>
                            </Section>
                        }
                        
                        <Line align='end'>     
                            {order.StatusCode == 4 && 
                                <SmallButton label='货已送到' onClick={() => {
                                    console.log('备货完成')                                     
                                }}/>}                   
                            {order.StatusCode == 5 && 
                                <SmallButton label='已送到' onClick={() => {
                                    console.log('取货完成')                                
                                }}/>}
                                
                            <SmallButton label={order.showDetail ? '收起' : '查看详情'} 
                                        onClick={() => {
                                          order.showDetail = !order.showDetail;
                                          this.setState({orders: this.state.orders})
                                        }}/>
                        </Line>
                    </Section>)
                })}
                </div>
            </Page>
            );
        
    }
}