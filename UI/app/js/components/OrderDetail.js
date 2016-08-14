import React, {Component} from 'react';
import {ListItem} from './common/GoodsList';
import * as actions from '../actions/order';
import Page from './common/Page';
import Detail from './OrderConfirm/Detail';
import Comments from './OrderConfirm/Comments';
import OrderStatus from './common/OrderStatus';
import {Section} from './common/Widgets';

export default class OrderDetail extends Component {
    componentWillMount() {
        this.state = {
            order: null
        };
        
        actions.getOrderDetail(this.props.id).then(order => {
            this.setState({
                order
            });
        });
    }

    render() {
        const order = this.state.order;
        if (!order) {
            return null;
        }

        order.info = {};
        return (
            <Page >
                <OrderStatus status={order.StatusCode}/>
                <Section title='已点菜品' list={true}> 
                {
                    order.SubOrders.map((item, index) => (
                        <ListItem key={index}
                                  url={item.PicUrl}
                                  count={item.Count}
                                  price={item.Price}/>)      
                    )
                }
                </Section>
                <Detail {...order.info}/>
                <Comments Comment={order.info.Comment}/>
            </Page>
        );
    }
}