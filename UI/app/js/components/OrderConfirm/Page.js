import React, {Component} from 'react';
import Page from '../common/Page.js';
import OrderDetail from './Detail.js';
import OrderList from './List.js';
import Comments from './Comments.js';
import Button from '../common/Button';
import OrderStatus from '../common/OrderStatus.js';
import * as actions from '../../actions/order.js';
export default class OrderConfirmPage extends Component {
    componentWillMount() {
        this.state = {
            submitting: false
        }
    }
    submmitOrder() {
        const {info, goods} = this.props.chart;
        const list = Object.keys(goods).map(key => {
            
            return {Id: goods[key].GoodsId, Count: goods[key].count};
        });
        const data = {
            OpenId: 124123,
            TrainNumber: info.TrainNumber,
            CarriageNumber: '' + info.CarriageNumber,
            IsDelay: info.IsDelay == 'on',
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
                        onClick: this.submmitOrder.bind(this),
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