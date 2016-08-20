import React, {Component} from 'react';
import * as actions from '../../actions/order';
import * as shopActions from '../../actions/shopOrders';
import Page from '../common/Page';
import Footer from '../common/Footer';
import {ListItem} from '../common/GoodsList';
import  * as Constants from '../../constants/system';
import {Section, Line, Label, SmallButton} from '../common/Widgets';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            orderMap: {'1': [], '2': [], '3': []},
            status: 1,
            total: 0,
            prepare: {},
            showAll: false
        };
        this.orders = [];
        actions.getOrderList(this.getUserId()).then((orderList)=>{
            if (orderList) {
                const orderMap = this.state.orderMap;
                orderList.map(order => {
                    this.orders[order.OrderId] = {checked: false};
                    if (orderMap[order.StatusCode]) {
                        orderMap[order.StatusCode].push(order);
                    }
                });
                this.setState({
                    orderMap
                });
            }
        });

        
    }

    getUserId() {
        return '124123'
    }

    summary() {
        const prepare = {};
        this.state.orderMap[2].map(order => {
            order.SubOrders.map(subOrder => {
                prepare[subOrder.GoodsId] = {
                    Name: subOrder.Name,
                    Count: prepare[subOrder.Id] ? prepare[subOrder.Id].count + subOrder.Count: subOrder.Count
                }
            });    
        });
        let total = 0;
        Object.keys(prepare).map(key => {
            total +=prepare[key].Count
        })
        
        this.setState({
            total,
            prepare
        });
    }

    render() {
        const {status, orderMap, showAll} = this.state;
        let count = 0;
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态

        const kindList = Object.keys(this.state.prepare);
        return (
            <Page flex={true} direction='col' className='order-list'>
                <div className='tabs'>
                    <div className={status == 1 ? 'active' : ''}
                         onClick={()=> this.setState({status: 1})}>可接受订单</div>
                    <div className={status == 2 ? 'active' : ''}
                         onClick={()=> {
                             this.summary();
                             this.setState({status: 2})
                            }}>待配货</div>
                    <div className={status == 3 ? 'active' : ''}
                         onClick={()=> this.setState({status: 3})}>待取货</div>
                </div>
                <div className='content'>
                {status == 2 && <Section>
                    <Label>{`还需制作${kindList.length}道菜，（${this.state.total}种菜）`}</Label>
                    <SmallButton label={this.state.showAll ? '收起' : '展开详情'} onClick={() => this.setState({showAll: !this.state.showAll})}></SmallButton>
                    {
                        this.state.showAll && kindList.map(kind => {
                            const item = this.state.prepare[kind]
                            return (<Line key={kind}>
                                    <Label flex={true}>{item.Name}</Label>
                                    <Label size='small'>{`x${item.Count}`}</Label>
                                </Line>) 
                        })
                    }
                </Section>}
                {orderMap[status].map((order, index) => {
                    return (
                     <Section list={true} key={index} >
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
                        <Line align='end'>
                            {order.StatusCode == 1 && 
                                <SmallButton label='接受订单' onClick={() => {
                                   shopActions.takeOrder().then(() => {
                                       this.summary();
                                   });
                                }}/>}
                            {order.StatusCode == 2 && 
                                <SmallButton label='货已备好' onClick={() => {
                                    shopActions.orderReady().then(() => {
                                       this.summary();
                                   });                                    
                                }}/>}
                            {order.StatusCode == 3 && 
                                <SmallButton label='货已取走' onClick={() => {
                                    console.log('取货完成')                                
                                }}/>}
                        </Line>
                    </Section>)
                })}
                </div>
            </Page>
            );
        
    }
}