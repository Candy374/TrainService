import React, {Component} from 'react';
import Footer from '../common/Footer.js';
import OrderDetail from './Detail.js';
import OrderList from './List.js';
import Comments from './Comments.js';
import OrderStatus from '../common/OrderStatus.js';
export default class OrderConfirmPage extends Component {
    render() {
         const footer = {
                    button: {
                        label: '立即支付',
                        onClick: () => console.log('pay')
                    }
                };

        return (
            <div className='order-confirm'>
                <OrderStatus />
                <OrderDetail />
                <OrderList />
                <Comments />
                <Footer {...footer} />
            </div>
        );
    }
}