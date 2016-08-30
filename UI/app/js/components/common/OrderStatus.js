import React, {Component} from 'react';
import {Section, Line, Label} from '../common/Widgets';
import {CountDown} from '../common/Detail';

const OrderStatus = ({status, ExpiredTime}) =>{
     // 0：未付款，1：已付款，2：商家已接单，3：商家已配货 
    // 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
    const getClass = (tabStatus) => {
        if (status == tabStatus) {
            return 'active';
        } else if (status < tabStatus) {
            return 'todo';
        }

        return 'done';
    };

    return (
        <Section title='订单状态'>
           {status == 7 ?  (
               <ul className='status direction-col'>
                        <li className={getClass(7)}>订单取消</li>
                </ul>)
            : (<ul className='status direction-col'>            
                <li className={getClass(0)}>待付款</li>
                <li className={getClass(1)}>已付款</li>
                <li className={getClass(2)}>商家已接单</li>
                <li className={getClass(3)}>商家已配货</li>
                <li className={getClass(4)}>快递员已取货</li>
                <li className={getClass(5)}>已经送到指定位置</li>
                <li className={getClass(8)}>异常状态</li>                
                <li className={getClass(6)}>订单结束</li>
              </ul>)
            }
            {status == 0 && <label>请在<CountDown ExpiredTime={ExpiredTime}/>之内下单，逾期订单将自动取消</label>}
        </Section>
    );
};

export default OrderStatus;