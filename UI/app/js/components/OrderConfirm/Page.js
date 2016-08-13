import React, {Component} from 'react';
import Page from '../common/Page.js';
import OrderDetail from './Detail.js';
import OrderList from './List.js';
import Comments from './Comments.js';
import Button from '../common/Button';
import OrderStatus from '../common/OrderStatus.js';
export default class OrderConfirmPage extends Component {
    render() {
         const footer = {
                    button: {
                        label: '立即支付',
                        onClick: () => console.log('pay'),
                        disabled: false
                    },
                     left: (<Button label='返回修改'
                                    onClick={this.props.prePage}>
                        </Button>)
                };
        const chart = this.props.chart;
        return (
            <Page footer={footer} className='order-confirm'>
                <OrderStatus />
                <OrderDetail {...chart.info}/>
                <OrderList list={chart.goods} total={chart.total}/>
                <Comments comments={chart.info.comments}/>
            </Page>
        );
    }
}