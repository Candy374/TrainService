import React, {Component} from 'react';
import * as actions from '../../actions/order';
import * as shopActions from '../../actions/shopOrders';
import Page from '../common/Page';
import Footer from '../common/Footer';
import {ListItem, NumberLine, SummaryLine} from '../common/GoodsList';
import  * as Constants from '../../constants/system';
import {Section, Line, Label, SmallButton} from '../common/Widgets';
import Detail from '../common/Detail';

const formatList = (list) => {
    const providers = {};
    list.map(order => {
        order.SubOrders.map(order => {
            const provider = order.Provider;
            if (!providers[provider.ProviderId]) {
                providers[provider.ProviderId] = provider;
                providers[provider.ProviderId].goods = [];
            }

            providers[provider.ProviderId].goods.push({
                Name: order.Name,
                Count: order.Count
            });
        });
        order.providers = Object.keys(providers).map(key => providers[key]);
    })

    
    return list;
}

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            showAll: true,
            dishReadyList: [],
            diliveringList: [],
            openId: this.props.openId,
            status: 1
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

        shopActions.getDeliverReadyOrder().then((dishReadyList) => {
            shopActions.getDeliverDoneOrders().then((diliveringList)=>{
                const status = dishReadyList.length > 0 ? 1 : 2;
                this.setState({
                    dishReadyList: formatList(dishReadyList), 
                    diliveringList: formatList(diliveringList), 
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
                            <Label flex={true}>{`单号：${order.OrderId}`}</Label>
                            <Label align='end'>{`时间：${order.ExpectTime}`}</Label>
                        </Line>
                        {
                            order.providers.map(provider => {
                                <Section>
                                    <Line>
                                        <Label>{provider.Name}</Label>
                                        <Label>{provider.TelphoneNumber}</Label>
                                        <Label align='end'>{provider.Location}</Label>
                                    </Line>
                                    
                                    {
                                        provider.goods.map(dish => {
                                            <Line>
                                                <Label>{dish.Name}</Label>
                                                <Label align='end'>{dish.Count}</Label>
                                            </Line>
                                        })
                                    }
                                </Section>
                            })
                        }

                        <Line>     
                            <Label align='end'>{`共${order.Amount}道菜`}</Label>
                        </Line>   
                        <Line>     
                            <Label >{`收货地址：${order.TrainNumber}`}</Label>
                        </Line>  
                        {status == 1 ? 
                            <SmallButton label='已取餐' onClick={() => {
                                    shopActions.expressOrder(order, this.state.openId).then(() => {
                                       this.updateOrder();
                                   });                            
                                }}/>
                                : 
                            <SmallButton label='货已送到' onClick={() => {
                                    shopActions.doneDeliver(order, this.state.openId).then(() => {
                                    this.updateOrder();
                                    });                                
                                }}/>
                        }
                    </Section>); 
                    })  
                }
               </div>
            </Page>
        );        
    }
}