import React, {Component} from 'react';
import * as actions from '../actions/order';
import Page from './common/Page';
import Footer from './common/Footer';
import {ListItem, SummaryLine} from './common/GoodsList';
import  * as Constants from '../constants/system';
import {Section, Line, ImgLine, Label, SmallButton} from './common/Widgets';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            orderMap: {'-1': [], '0':[], '1': [], '2': [], '3': []},
            status: -1,
            showAll: false
        };
        actions.getOrderList(this.props.openId).then((orderList)=>{
            if (orderList) {
                const orderMap = this.state.orderMap;
                let defaultStatus = -1;
                orderList.map(order => {               
                    orderMap['-1'].push(order);
                    const status = this.getStatus(order.StatusCode);
                    if (status == 0) {
                        defaultStatus = 0;
                    }
                    orderMap[status].push(order);
                });
                this.setState({
                    orderMap,
                    status: defaultStatus
                });
            }
        });
        this.showNum = 5;
    }

    getStatus(StatusCode) {
        if (StatusCode >= 2 && StatusCode < 6 ) {
            StatusCode = 1;
        } else if (StatusCode >= 6 && StatusCode < 7) {
            StatusCode = 2;
        } else if (StatusCode >= 7) {
            StatusCode = 3;
        }
        return StatusCode;
    }

    render() {
        const {status, orderMap, showAll} = this.state;
        let count = 0;
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
        return (
            <Page flex={true} direction='col' className='order-list'>
                <div className='tabs'>
                    <div className={status == -1 ? 'active' : ''} 
                         onClick={()=> this.setState({status: -1})}>全部</div>
                    <div className={status == 0 ? 'active' : ''}
                         onClick={()=> this.setState({status: 0})}>待付款</div>
                    <div className={status == 1 ? 'active' : ''}
                         onClick={()=> this.setState({status: 1})}>待收货</div>
                    <div className={status == 2 ? 'active' : ''}
                         onClick={()=> this.setState({status: 2})}>已完成</div>
                    <div className={status == 3 ? 'active' : ''}
                         onClick={()=> this.setState({status: 3})}>已取消</div>
                </div>
                <div className='content'>
                {orderMap[status].map((order, index) => {
                    if (!showAll && count++ > this.showNum) {
                        return null;
                    }
                    return (
                     <Section list={true} key={index}> 
                        <Line>
                            <Label flex={true}>{order.TrainNumber}</Label>
                            <label>{order.OrderStatus}</label>
                        </Line>
                            {order.SubOrders.map((item, index) => (
                                <ListItem key={index}
                                        url={item.PicUrl}
                                        name={item.Name}
                                        count={item.Count}
                                        price={item.Price}/>)                                
                            )}
                        <SummaryLine left={order.OrderDate} price={order.Amount} />
                        <Line align='end'>
                            <SmallButton label='订单详情' onClick={() => {
                                this.props.setCurrentOrderId(order.OrderId)
                            }}/>
                            {this.getStatus(order.StatusCode) == 0 && 
                                <SmallButton label='取消订单' onClick={() => {
                                    actions.cancelOrder(order.OrderId).then((result) => {
                                        if (result) {
                                            this.props.setCurrentOrderId(order.OrderId);
                                        }
                                    })                               
                                }}/>}
                            {/*this.getStatus(order.StatusCode) == 2 && 
                             <SmallButton label='追加评价' onClick={() => {
                                    this.props.setCurrentOrderId(order.OrderId)
                                }}/>*/}
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