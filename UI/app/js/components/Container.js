import React, {Component} from 'react';
import Footer from './common/Footer';
import OrderPage from './OrderPage';
import OrderInfo from './OrderInfo';
import MyOrders from './MyOrders';
import OrderDetail from './OrderDetail';
import ChooseStation from './ChooseStation';
import ConfirmPage from './OrderConfirm/Page';
import * as actions from '../actions/order';

const _extend = Object.assign || function(target) {
    for (var i = 1; i < arguments.length; i++) {
        var source = arguments[i];
        for (var key in source) {
            if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } }
    return target; };
    
export default class Container extends Component {
    componentWillMount() {
        this.state = {
            page: location.hash.indexOf('#MyOrders') == 0 ? 'MyOrders' : 'Choose',
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
            stations: []
        };
        this.updateChart = this.updateChart.bind(this);
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
                                 setCurrentOrderId={this.setCurrentOrderId}
                                 prePage={this.prePage.bind(this, 'Info')}
                                 nextPage={this.nextPage.bind(this, 'Detail')} />);
            case 'Detail':
                return <OrderDetail id={this.state.orderId}/>;
            case 'MyOrders':
                return <MyOrders setCurrentOrderId={this.setCurrentOrderId}/>;
        }
    }
}