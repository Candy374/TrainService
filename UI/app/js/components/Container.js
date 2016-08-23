import React, {Component} from 'react';
import Footer from './common/Footer';
import OrderPage from './OrderPage';
import OrderInfo from './OrderInfo';
import MyOrders from './MyOrders';
import OrderDetail from './OrderDetail';
import ChooseStation from './ChooseStation';
import ConfirmPage from './OrderConfirm/Page';
import * as actions from '../actions/order';
import ShopOrders from './Shop/MyOrders';
import Delivery from './Shop/Delivery';


const _extend = Object.assign || function(target) {
    for (var i = 1; i < arguments.length; i++) {
        var source = arguments[i];
        for (var key in source) {
            if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } }
    return target; };
    
export default class Container extends Component {
    componentWillMount() {
        let page = 'Choose';
        if (location.hash.indexOf('#MyOrders') == 0) {
            page = 'MyOrders';
        } else if (location.hash.indexOf('#ShopOrders') == 0) {
            page = 'ShopOrders';
        } else if (location.hash.indexOf('#Delivery') == 0) {
            page = 'Delivery';
        }

        this.state = {
            page: page,
            chart: { 
                goods: {},
                total: 0,
                info: {
                    TrainNumber: '',
                    CarriageNumber: '',
                    IsDelay: false,
                    Contact: '',
                    ContactTel: '',
                    Comment: ''
                },
                station: {}
            },
            orderId: null,
            stations: [],
            submitting: false
        };
        this.updateChart = this.updateChart.bind(this);
        this.submmitOrder = this.submmitOrder.bind(this);
        this.setCurrentOrderId = this.setCurrentOrderId.bind(this);        
        actions.getStations().then((stations) => {
            this.setState({stations})
        });
        actions.getUserInfo().then((user) => {
            const info = Object.assign({}, this.state.chart.info, user);
            this.state.chart.info = info;
            this.setState({
                chart: this.state.chart
            });
        });
    }

    nextPage(page) {
        this.setState({page});
    }

    prePage(page) {
        this.setState({page});
    }

    updateChart(chart, callback) {
        const updated = _extend({}, this.state.chart, chart);
        this.setState({
            chart: updated
        }, callback);
    }

    setCurrentOrderId(orderId) {
        this.setState({orderId}, () => this.nextPage('Detail'));
    }

    submmitOrder() {
        if (this.state.submitting) {
            return;
        }

        const {info, goods} = this.state.chart;
        const list = Object.keys(goods).map(key => {            
            return {Id: goods[key].GoodsId, Count: goods[key].count};
        });
        const data = {
            OpenId: 124123,
            TrainNumber: info.TrainNumber,
            CarriageNumber: '' + info.CarriageNumber,
            IsDelay: info.IsDelay == 'on',
            OrderType: 0,
            PayWay: 0,
            Comment: info.Comment,
            Contact: info.Contact,
            ContactTel: info.ContactTel,
            TotalPrice: this.state.chart.total,
            List: list
        }
        this.setState({
            submitting: true
        });
        // actions.submmitOrder(data).then(orderId => {
        //     this.setCurrentOrderId(orderId)
        //  });
       this.setCurrentOrderId(18)
    }

    render() {
        switch(this.state.page){
            case 'Choose':
                return (
                    <ChooseStation stations={this.state.stations}
                                   nextPage={this.nextPage.bind(this, 'Order')}
                                   updateChart={this.updateChart} />);
            case 'Order':
                return (
                    <OrderPage  chart={this.state.chart}
                                updateTotal={this.updateTotal}
                                updateChart={this.updateChart}
                                nextPage={this.nextPage.bind(this, 'Info')} />);
            case 'Info':
                return (
                    <OrderInfo chart={this.state.chart}
                               prePage={this.prePage.bind(this, 'Order')}
                               nextPage={this.nextPage.bind(this, 'Confirm')} 
                               updateChart={this.updateChart}/>);
            case 'Confirm':
                return (
                    <ConfirmPage chart={this.state.chart}
                                 submitting={this.state.submitting}
                                 submmitOrder={this.submmitOrder}
                                 prePage={this.prePage.bind(this, 'Info')}
                                 nextPage={this.nextPage.bind(this, 'Detail')} />);
            case 'Detail':
                return (
                    <OrderDetail setCurrentOrderId={this.setCurrentOrderId}
                                 updateChart={this.updateChart}
                                 submmitOrder={this.submmitOrder}
                                 nextPage={this.nextPage.bind(this, 'Info')}
                                 id={this.state.orderId}/>);
            case 'MyOrders':
                return <MyOrders setCurrentOrderId={this.setCurrentOrderId}/>;

            case 'ShopOrders': 
                return <ShopOrders />;

            case 'Delivery': 
                return <Delivery />;
        }
    }
}