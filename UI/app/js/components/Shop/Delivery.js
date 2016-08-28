import React, {Component} from 'react';
import * as actions from '../../actions/order';
import * as shopActions from '../../actions/shopOrders';
import Page from '../common/Page';
import Footer from '../common/Footer';
import {ListItem, NumberLine, SummaryLine} from '../common/GoodsList';
import  * as Constants from '../../constants/system';
import {Section, Line, Label, SmallButton} from '../common/Widgets';
import Detail from '../common/Detail';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            showAll: true,
            orders: [],
            status: 1,
            openId: this.props.openId || 124123
        };
    }

    componentDidMount() {        
        this.updateOrder();
    }

    componentWillReceiveProps(nextProps) {
        if(nextProps.openId && nextProps.openId != this.state.openId) {
            this.setState({openId: nextProps.openId}, this.updateOrder);
        }
    }
    
    updateOrder() {
        if (!this.state.openId) {
            return;
        }

        actions.getOrderList(this.state.openId).then((orderList)=>{
            if (orderList) {
                const orders = orderList.filter(order => order.StatusCode >= 3 && order.StatusCode < 5);
                this.setState({
                    orders
                });
            }
        });
    }

    render() {
        const {orders} = this.state;
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
        // const footer = {
        //     button: {
        //         label: this.state.showAll ? '只显示需取货订单' : '显示所有订单',
        //         onClick: () => this.setState({showAll: !this.state.showAll})
        //     }
        // };
        return (
            <Page flex={true} direction='col' className='order-list'>
               { // <div className='tabs'>
                //     <div className={status == 1 ? 'active' : ''}
                //          onClick={()=> this.setState({status: 1})}>未完成订单</div>
                //     <div className={status == 2 ? 'active' : ''}
                //          onClick={()=> this.setState({status: 2})}>已完成配送</div>
                // </div>
               }
                <div className='content'>
                {orders.map((order, index) => {
                    if (!this.state.showAll && order.StatusCode != 3) {
                        return;
                    }
 
                    return (
                     <Section key={index}>
                        <Line><Label>{order.ProviderName}</Label></Line>
                        <Detail {...order}/>
                        {order.showDetail &&
                            <Section list={true} title='商品信息'>                        
                                {order.SubOrders.map((item, index) => <NumberLine item={item} key={index}/>)}
                                <SummaryLine price={order.Amount} left={order.OrderDate}/>
                            </Section>
                        }
                        
                        <Line align='end'>                        
                            {order.StatusCode == 3 && 
                                <SmallButton label='已取餐' onClick={() => {
                                    shopActions.expressOrder(order.OrderId).then(() => {
                                       this.updateOrder();
                                   });                            
                                }}/>}
                            {order.StatusCode == 4 && 
                                <SmallButton label='货已送到' onClick={() => {
                                    shopActions.doneDeliver(order.OrderId).then(() => {
                                       this.updateOrder();
                                    });                                
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