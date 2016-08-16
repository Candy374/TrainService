import React, {Component} from 'react';
import Page from '../common/Page';
import OrderDetail from './Detail';
import OrderList from '../common/GoodsList';
import Comments from './Comments';
import {Button} from '../common/Widgets';
import OrderStatus from '../common/OrderStatus';
import * as actions from '../../actions/order';
const OrderConfirmPage = (props) => {
    const footer = {
        button: {
            label: '立即支付',
            onClick: props.submmitOrder,
            disabled: props.submitting
        },
        left: {
            type: 'button',
            label: '返回修改',
            disabled: props.submitting,
            onClick: props.prePage
        }
    };
    const chart = props.chart;
    return (
        <Page footer={footer}>            
            <OrderStatus status={0}/>
            <OrderDetail {...chart.info}/>
            <OrderList list={chart.goods} total={chart.total}/>
            <Comments Comment={chart.info.Comment}/>
        </Page>
    );
};

export default OrderConfirmPage;
