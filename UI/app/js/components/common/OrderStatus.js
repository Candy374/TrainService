import React, {Component} from 'react';


export default class OrderStatus extends Component {
    render() {
        const status = this.props.status;

        return (
            <div className='section'>
               <div className='line'>订单状态</div>
                <div className='status'>
                    <div className={status == 0 ? 'active' : ''}>待支付 ></div>
                    <div className={status == 1 ? 'active' : ''}>商家接单 ></div>
                    <div className={status == 2 ? 'active' : ''}>送餐中 ></div>
                    <div className={status == 3 ? 'active' : ''}>完成送达</div>
               </div>
            </div>
        );
    }
}