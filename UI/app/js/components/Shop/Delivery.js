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
            dishReadyList: [],
            diliveringList: [],
            openId: this.props.openId,
            status: 1,
            deliveryStatus: localStorage.getItem('delivery') ? JSON.parse(localStorage.getItem('delivery')) : {}
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

    formatList(list) {
        list.map(order => {
            const providers = {};
            let count = 0;
            order.SubOrders.map(subOrder => {
                const provider = subOrder.Provider;
                if (!providers[provider.ProviderId]) {
                    providers[provider.ProviderId] = provider;
                    providers[provider.ProviderId].goods = [];
                }
                count += subOrder.Count;
                providers[provider.ProviderId].goods.push({
                    Name: subOrder.Name,
                    Count: subOrder.Count,
                    Id: subOrder.GoodsId,
                    Checked: this.getItem(order.OrderId, provider.ProviderId, subOrder.GoodsId)
                });
            });
            order.providers = Object.keys(providers).map(key => providers[key]);
            order.Count = count;
        })
    
        return list;
    }

    getItem(orderId, providerId, goodsId) {
        const delivery = this.state.deliveryStatus;
        const provider = delivery[orderId] && delivery[orderId][providerId];
        return provider && (goodsId ? provider[goodsId] : provider.Checked);  
    }

    setItem(orderId, providerId, goodsId, checked) {
        const delivery = this.state.deliveryStatus
        delivery[orderId] = delivery[orderId] || {};
        const provider = delivery[orderId][providerId] || {};
        provider[goodsId] = checked;
        const checkedGoods = Object.keys(provider).filter(key => !provider[key]);
        provider.Checked = checkedGoods.length == 1 && checkedGoods[0] == 'Checked';
        //delivery[orderId][providerId] = provider;
        
        this.setState({deliveryStatus: delivery}, () => {
            localStorage.setItem('delivery', JSON.stringify(this.state.deliveryStatus));
        });
    }

    updateOrder() {
        if (!this.state.openId) {
            return;
        }

        shopActions.getDeliverReadyOrder().then((dishReadyList) => {
            shopActions.getDeliverDoneOrders().then((diliveringList)=>{
                const status = dishReadyList.length > 0 ? 1 : 2;
                this.setState({
                    dishReadyList: this.formatList(dishReadyList), 
                    diliveringList: this.formatList(diliveringList), 
                    status
                });
            });
        });
    }

    render() {
        const {dishReadyList, diliveringList, status} = this.state;
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
        // const footer = {
        //     button: {
        //         label: this.state.showAll ? '只显示需取货订单' : '显示所有订单',
        //         onClick: () => this.setState({showAll: !this.state.showAll})
        //     }
        // };
        const list = status == 1 ? dishReadyList : diliveringList;
        return (
            <Page flex={true} direction='col' className='order-list'>
               <div className='tabs'>
                    <div className={status == 1 ? 'active' : ''}
                            onClick={()=> this.setState({status: 1})}>新订单</div>
                    <div className={status == 2  ? 'active' : ''}
                        onClick={()=> this.setState({status: 2})}>正在配送</div>
               </div>
               <div className='content'>               
                {list.map((order, index) => {
                    return (
                     <Section key={index}>
                        <Line>
                            <Label flex={true} >{`单号：${order.OrderId}`}</Label>
                            <Label align='end'>{`共${order.Count}道菜`}</Label>
                        </Line>                        
                        <Line>     
                            <Label flex={true}>{`收货地址：${order.TrainNumber}`}</Label>                            
                            <Label align='end' size='auto'>{`送达时间：${order.ExpectTime.substr(order.ExpectTime.indexOf('T') + 1)}`}</Label>
                        </Line>  
                        {
                            order.providers.map((provider, index) => (
                                <Section key={index}>
                                    <Line>
                                        <Label done={this.getItem(order.OrderId, provider.ProviderId)}>{provider.Name}</Label>
                                        <Label>{provider.TelphoneNumber}</Label>
                                        <Label align='end'>{provider.Location}</Label>
                                    </Line>
                                    
                                    {
                                        provider.goods.map((dish, index) => {
                                            const checked = this.getItem(order.OrderId, provider.ProviderId, dish.Id);
                                            return (
                                            <Line key={index} className='short'>
                                                <input type='checkbox' value={checked} checked={checked}
                                                       onChange={(event) => this.setItem(order.OrderId, provider.ProviderId, dish.Id, event.target.checked)}/>
                                                <Label done={checked}>{dish.Name}</Label>
                                                <Label align='end'>{dish.Count}</Label>
                                            </Line>);
                                        })
                                    }
                                </Section>
                            ))
                        }
                        <Line align='end'>
                        {status == 1 ? 
                            <SmallButton label='已取餐' onClick={() => {
                                    shopActions.expressOrder(order, this.state.openId).then(() => {
                                       this.updateOrder();
                                   });                            
                                }}/>
                                : 
                            <SmallButton label='货已送到' onClick={() => {
                                    shopActions.doneDeliver(order.OrderId, this.state.openId).then(() => {
                                    this.updateOrder();
                                    });                                
                                }}/>
                        }
                        </Line>  
                    </Section>); 
                    })  
                }
               </div>
            </Page>
        );        
    }
}