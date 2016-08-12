import React, {Component} from 'react';
import * as actions from '../actions/order';
import Page from './common/Page.js';
export default class MyOrders extends Component {
    componentWillMount() {
        this.state = {
            orderList: []
        };
        actions.getOrderList().then((orderList)=>{
            this.setState({
                orderList
            })
        });
    }

    render() {
        if (this.state.orderList.length == 0) {
            return null;
        }
        
            return (
            <Page className='order-list'>
                <div className='tabs'>
                    <div >全部</div>
                    <div className='acitve'>待付款</div>
                    <div >待发货</div>
                    <div >待收获</div>
                    <div >待评价</div>
                </div>
                <div className='content'>
                {this.state.orderList.map((order, index) => {
                    return (
                     <div className='section list' key={index}> 
                        <div className='line'>
                            <div className='label'>{order.shopName}
                            </div>
                            <div className='status rightToLeft'>{order.status}
                            </div>
                        </div>
                            {
                                order.list.map((food, index) => {
                                    return (
                                        <div key={index} className='item'>
                                            {food.PictureUrl && <img src= {food.PictureUrl}  className='img'/>}
                                            <div className="line">
                                                <label className="label">
                                                    {food.Name}
                                                </label>

                                                <div className="width-small">
                                                    {food.count && `x${food.count}`}
                                                </div> 
                                                <div className="width-small">
                                                    {`￥${food.SellPrice}`}
                                                </div>

                                            </div>                           
                                        </div>
                                    )
                                })
                            }
                        <div className='line'>
                            <div className='label'>
                                2016-08-11 22:54:14
                            </div>
                            <div className='rightToLeft'>共计: ￥{order.totalPrice}
                             (含外送费${order.fee})
                            </div>
                        </div>
                        <div className='line rightToLeft'>
                            <button className='detail'>订单详情</button>
                        </div>
                    </div>
                        )
                })
                }
                 </div>
            </Page>
            );
        
    }
}