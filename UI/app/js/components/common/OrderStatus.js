import React, {Component} from 'react';
import {Section, Line, Label} from '../common/Widgets';

export default class OrderStatus extends Component {
    render() {
        const status = this.props.status;

        return (
            <Section title='订单状态'>
                <div className='status'>
                    <div className={status == 0 ? 'active' : ''}>待支付</div>
                    <div className={status == 0 ? 'active' : ''}>></div>
                    <div className={status == 1 ? 'active' : ''}>商家接单</div>
                    <div className={status == 1 ? 'active' : ''}>></div>
                    <div className={status == 2 ? 'active' : ''}>送餐中</div>
                    <div className={status == 2 ? 'active' : ''}>></div>
                    <div className={status == 3 ? 'active' : ''}>完成送达</div>
               </div>
            </Section>
        );
    }
}