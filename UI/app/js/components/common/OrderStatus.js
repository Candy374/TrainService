import React, {Component} from 'react';


export default class OrderStatus extends Component {
    render() {
        return (
            <div className='section'>
               <div className='line'>订单状态</div>
               <div className='status img'>
                    <div className='start' /> 
                    <div className='line' />
                    <div className='line' />
                    <div className='end' />
                </div>
                <div className='status'>
                    <div >待支付</div>
                    <div >商家接单</div>
                    <div >送餐中</div>
                    <div >完成送达</div>
               </div>
            </div>
        );
    }
}