import React, {Component} from 'react';
import * as actions from '../../actions/order';
import * as shopActions from '../../actions/shopOrders';
import Page from '../common/Page';
import Footer from '../common/Footer';
import {ListItem, SummaryLine,NumberLine} from '../common/GoodsList';
import  * as Constants from '../../constants/system';
import {Section, Line, Label, SmallButton} from '../common/Widgets';
import {Detail} from '../common/Detail';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            orderMap: {'1': [], '2': [], '3': []},
            status: 1,
            total: 0,
            prepare: {},
            showAll: false,
            openId: this.props.openId,
            providerId: this.props.providerId
        };
        this.orders = [];
    }

    componentDidMount() {        
        this.updateOrderList();
    }

    componentWillReceiveProps(nextProps) {
        if(nextProps.openId && nextProps.openId != this.state.openId) {
            this.setState({openId: nextProps.openId}, this.updateOrderList);
        }

        if(nextProps.providerId && nextProps.providerId != this.state.providerId) {
          this.setState({providerId: nextProps.providerId}, this.updateOrderList);
        }
    }
    

    updateOrderList() {
        if (!this.state.openId || !this.state.providerId) {
            return;
        }

        shopActions.getWaitingOrder(this.state.providerId).then(waitingOrder => {
            const orderMap = this.state.orderMap;
            orderMap['1'] = waitingOrder;
            this.setState({
              orderMap
            });
        }).then(() => {
            return shopActions.getReadyOrder(this.state.providerId).then(readyOrder => {
                const orderMap = this.state.orderMap;
                orderMap['2'] = readyOrder;
                this.setState({
                  orderMap
                });
            });
        });
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
        });
        
        this.setState({
            total,
            prepare
        });
    }

    render() {
        const {status, orderMap} = this.state;
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态

        const kindList = Object.keys(this.state.prepare);
        return (
            <Page flex={true} direction='col' className='order-list'>
                <div className='tabs'>
                    <div className={status == 1 ? 'active' : ''}
                         onClick={()=> this.setState({status: 1})}>新订单</div>
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
                    <Line>
                        <Label flex={true}>{`还需制作${kindList.length}道菜，（${this.state.total}种）`}</Label>
                    {kindList.length > 0 && <SmallButton label={this.state.showAll ? '收起' : '展开详情'} 
                        onClick={() => this.setState({showAll: !this.state.showAll})}/> }                        
                    </Line>
                    {
                        this.state.showAll && 
                        kindList.map(kind => <NumberLine item={this.state.prepare[kind]} key={kind} />)
                    }
                </Section>}
                {orderMap[status].map((order, index) => {
                    const button = (
                    <Line>
                        <Label flex={true}>{`订单号：${order.OrderId}`}</Label>
                        {status == 1 && 
                            <SmallButton label='接单' onClick={() => {
                                shopActions.takeOrder(order, this.state.openId).then(() => {                                       
                                    this.summary();
                                    this.updateOrderList();
                                });
                            }}/>}
                        {status == 2 && 
                            <SmallButton label='货已备好' onClick={() => {
                                shopActions.orderReady(order, this.state.openId).then(() => {
                                    this.summary();
                                });                                    
                            }}/>}
                    </Line>);
                    return <Detail {...order} key={index} button={button} />;
                })}
                </div>
            </Page>
            );
        
    }
}