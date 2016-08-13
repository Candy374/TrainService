import React, {Component} from 'react';
import * as actions from '../actions/order';
import Page from './common/Page';
import  * as Constants from '../Constants';

export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            orderList: [],
            status: -1
        };
        actions.getOrderList().then((orderList)=>{
            this.setState({
                orderList
            })
        });
    }

    render() {
        const {status, orderList} = this.state;
        if (orderList.length == 0) {
            return null;
        }
        // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
        // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
        return (
            <Page className='order-list'>
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
                {orderList.map((order, index) => {
                    let StatusCode = order.StatusCode;
                    if (StatusCode >= 2 && StatusCode < 5 ) {
                        StatusCode = 1;
                    } else if (StatusCode >=5 && StatusCode < 7) {
                        StatusCode = 2;
                    } else if (StatusCode >= 7) {
                        StatusCode = 3;
                    }

                    if (status != -1 && StatusCode != status) {
                        return null;
                    }
                    return (
                     <div className='section list' key={index}> 
                        <div className='line'>
                            <div className='label'>{order.TrainNumber}
                            </div>
                            <div className='status rightToLeft'>{order.OrderStatus}
                            </div>
                        </div>
                            {
                                order.SubOrders.map((food, index) => {
                                    return (
                                        <div key={index} className='item'>
                                            {food.PicUrl && <img src= {Constants.basicUrl + food.PicUrl}  className='img'/>}
                                            <div className="line">
                                                <div className='descriptions'>
                                                    <label className="name">
                                                        {food.Name}
                                                    </label>

                                                    <div className="width-small">
                                                        {food.count}
                                                    </div> 
                                                    <div className="width-small">
                                                        {`￥${food.Price}`}
                                                    </div>
                                                </div> 
                                            </div>                           
                                        </div>
                                    )
                                })
                            }
                        <div className='line'>
                            <div className='label'>
                                {order.OrderDate}
                            </div>
                            <div className='rightToLeft'>共计: ￥{order.Amount}</div>
                        </div>
                        <div className='line rightToLeft'>
                            <button className='detail'>订单详情</button>
                        </div>
                    </div>)
                })}
                </div>
            </Page>
            );
        
    }
}