import React, {Component} from 'react';
import Page from '../common/Page.js';
import OrderDetail from './Detail.js';
import OrderList from './List.js';
import Comments from './Comments.js';
import OrderStatus from '../common/OrderStatus.js';
export default class OrderConfirmPage extends Component {
    render() {
         const footer = {
                    button: {
                        label: '立即支付',
                        onClick: () => console.log('pay'),
                        className: 'active'
                    }
                };

        return (
            <Page footer={footer}>
                <div className='order-confirm'>
                    <OrderStatus />
                    <OrderDetail />
                    <OrderList />
                    <Comments />
                </div>
            </Page>
        );
    }
}