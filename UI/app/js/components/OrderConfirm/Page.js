import React, {Component} from 'react';
import Page from '../common/Page.js';
import OrderDetail from './Detail.js';
import OrderList from './List.js';
import Comments from './Comments.js';
import Button from '../common/Button';
import OrderStatus from '../common/OrderStatus.js';
import actions from '../../actions/order.js';
export default class OrderConfirmPage extends Component {
    submmitOrder() {
        const info = this.props.chart.info;
        const list = this.props.chart.goods.map(item => ({Id: item.Id, Count: item.count}));
        const data = {
            "OpenId": "这个ID通过调用微信JS-SDK来获取",
            TrainNumber: info.TrainNumber,
            CarriageNumber: '' + info.CarriageNumber,
            IsDelay: info.IsDelay,
            OrderType: 0, //订单类型，0是餐饮订单
            PayWay: 0, //支付途径，0是微信支付
            Comment: info.Comment,
            Contact: info.Contact,
            ContactTel: info.ContactTel,
            TotalPrice: this.props.chart.total,
            List: list
        }
        actions.submmitOrder(data);
    }

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
                <OrderStatus status={0}/>
                <OrderDetail {...chart.info}/>
                <OrderList list={chart.goods} total={chart.total}/>
                <Comments Comment={chart.info.Comment}/>
            </Page>
        );
    }
}