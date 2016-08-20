import React, {Component} from 'react';
import * as actions from '../../actions/order';
import Page from '../common/Page';
import Footer from '../common/Footer';
import {ListItem} from '../common/GoodsList';
import  * as Constants from '../../constants/system';
import {Section, Line, Label, SmallButton} from '../common/Widgets';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            orderMap: {'1': [], '2': [], '3': [], '4':[], '5':[]},
            status: 1,
            showAll: false
        };
        actions.getOrderList(this.getUserId()).then((orderList)=>{
            if (orderList) {
                const orderMap = this.state.orderMap;
                orderList.map(order => {
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

    render() {
        const {status, orderMap, showAll} = this.state;
        let count = 0;
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态

        const footer = status == 1 && {
            button: {
                label: '显示需要做的菜',
                onClick: () => console.log('show lis item')
            }
        }
        return (
            <Page flex={true} direction='col' className='order-list' footer={footer}>
                <div className='tabs'>
                    <div className={status == 1 ? 'active' : ''}
                         onClick={()=> this.setState({status: 1})}>待接单</div>
                    <div className={status == 2 ? 'active' : ''}
                         onClick={()=> this.setState({status: 2})}>待配货</div>
                    <div className={status == 3 ? 'active' : ''}
                         onClick={()=> this.setState({status: 3})}>待取货</div>
                    <div className={status == 4 ? 'active' : ''}
                         onClick={()=> this.setState({status: 4})}>待收货</div>
                    <div className={status == 5 ? 'active' : ''}
                         onClick={()=> this.setState({status: 5})}>已收货</div>
                </div>
                <div className='content'>
                {orderMap[status].map((order, index) => {
                    return (
                     <Section list={true} key={index} checked={order.checked} checkbox={true}>
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
                                   console.log('接受订单')                                      
                                }}/>}
                            {order.StatusCode == 2 && 
                                <SmallButton label='备货完成' onClick={() => {
                                    console.log('备货完成')                                     
                                }}/>}
                            {order.StatusCode == 3 && 
                                <SmallButton label='取货完成' onClick={() => {
                                    console.log('取货完成')                                
                                }}/>}
                            {order.StatusCode == 4 && 
                                <SmallButton label='收货完成' onClick={() => {
                                    console.log('收货完成')                             
                                }}/>}
                        </Line>
                    </Section>)
                })}
                {orderMap[status].length > this.showNum &&
                    <Footer button={{
                        onClick: ()=> this.setState({showAll: !this.state.showAll}),
                        label: showAll ? '收起' : '显示全部订单'
                    }}/>
                }
                </div>
            </Page>
            );
        
    }
}