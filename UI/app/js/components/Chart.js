import React, {Component} from 'react';
import Footer from './common/Footer.js';
import Page from './common/Page.js';
import {Section, Line, Label, Button} from './common/Widgets';
import {OrderListNoImg} from './common/GoodsList';

export default class OrderInfo extends Component {
    render() {
        const chart = this.props.chart;
        
        const footer = {
            button: {
                label: '去结算',
                onClick: this.props.submitOrder,
                disabled: this.props.submitting
            },
            left: 
        };
        
        return (
            <Page className='order-info' footer={footer}>         
                <OrderListNoImg total={chart.total} list={chart.goods}/>
            </Page>
        );
    }
}