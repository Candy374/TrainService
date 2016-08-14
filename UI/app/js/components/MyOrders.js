import React, {Component} from 'react';
import * as actions from '../actions/order';
import Page from './common/Page';
import Footer from './common/Footer';
import  * as Constants from '../constants/system';
import {Section, Line, ImgLine, Label} from './common/Widgets';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            orderMap: {'-1': [], '0':[], '1': [], '2': [], '3': []},
            status: -1,
            showAll: false
        };
        actions.getOrderList(this.getUserId()).then((orderList)=>{
            const orderMap = this.state.orderMap;
            orderList.map(order => {               
                orderMap['-1'].push(order);
                orderMap[this.getStatus(order.StatusCode)].push(order);
            })
            this.setState({
                orderMap
            })
        });
        this.showNum = 5;
    }

    getStatus(StatusCode) {
        if (StatusCode >= 2 && StatusCode < 5 ) {
            StatusCode = 1;
        } else if (StatusCode >=5 && StatusCode < 7) {
            StatusCode = 2;
        } else if (StatusCode >= 7) {
            StatusCode = 3;
        }
        return StatusCode;
    }

    getUserId() {
        return ''
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
                         onClick={()=> this.setState({status: 2})}>待评价</div>
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
                            {
                                order.SubOrders.map((food, index) => {
                                    return (
                                        <ImgLine url={food.PicUrl} key={index}>
                                            <Label flex={true}>{food.Name}</Label>
                                            <Label size='small'>x{food.Count}</Label>
                                            <Label size='small'>{`￥${food.Price}`}</Label>                      
                                        </ImgLine>
                                    )
                                })
                            }
                        <Line>
                            <Label flex={true}>{order.OrderDate}</Label>
                            <Label size='small'>共计: </Label>
                            <Label size='small'>￥{order.Amount}</Label>
                        </Line>
                        <Line align='end'>
                            <button className='detail'>订单详情</button>
                        </Line>
                    </Section>)
                })}
                {orderMap[status].length > this.showNum &&
                    <Footer button={{
                        onClick: ()=> this.setState({showAll: !this.state.showAll}),
                        label: showAll ? '收起' : '显示全部'
                    }}/>
                }
                </div>
            </Page>
            );
        
    }
}