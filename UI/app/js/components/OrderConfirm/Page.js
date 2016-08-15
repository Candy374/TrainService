import React, {Component} from 'react';
import Page from '../common/Page';
import OrderDetail from './Detail';
import OrderList from '../common/GoodsList';
import Comments from './Comments';
import Button from '../common/Button';
import OrderStatus from '../common/OrderStatus';
import * as actions from '../../actions/order';
export default class OrderConfirmPage extends Component {
    render() {
         const footer = {
                    button: {
                        label: '立即支付',
                        onClick: this.submmitOrder.bind(this),
                        disabled: this.state.submitting
                    },
                    left: (
                        <Button label='返回修改'
                                disabled={this.state.submitting}
                                onClick={this.props.prePage}>
                        </Button>)
                };
        const chart = this.props.chart;
        return (
            <Page footer={footer}>            
                <OrderStatus status={0}/>
                <OrderDetail {...chart.info}/>
                <OrderList list={chart.goods} total={chart.total}/>
                <Comments Comment={chart.info.Comment}/>
            </Page>
        );
    }
}