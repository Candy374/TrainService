import React, {Component} from 'react';
import Footer from './common/Footer.js';
import OrderPage from './OrderPage';
import OrderInfo from './OrderInfo.js';
import MyOrders from './MyOrders.js';
import ConfirmPage from './OrderConfirm/Page.js';
export default class Container extends Component {
    componentWillMount() {
        this.state = {
            page: location.hash.indexOf('#MyOrders') == 0 ? 4 : 1,
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
                }
            }
        };
        this.nextPage = this.nextPage.bind(this);
        this.prePage = this.prePage.bind(this);
        this.updateChart = this.updateChart.bind(this);
    }

    nextPage() {
        this.setState({
            page: this.state.page + 1
        });
    }

    prePage() {
        this.setState({
            page: this.state.page - 1
        });
    }

    updateChart(chart, callback) {
        const updated = Object.assign({}, this.state.chart, chart);
        this.setState({
            chart: updated
        }, callback);
    }

    render() {
        switch(this.state.page){
            case 1:
                return (
                    <OrderPage  chart={this.state.chart}
                                updateTotal={this.updateTotal}
                                updateChart={this.updateChart}
                                nextPage={this.nextPage} />);
            case 2:
                return (
                    <OrderInfo chart={this.state.chart}
                               prePage={this.prePage}
                               nextPage={this.nextPage} 
                               updateChart={this.updateChart}/>);
            case 3:
                return (
                    <ConfirmPage chart={this.state.chart}
                                 prePage={this.prePage} />);
            case 4:
                return <MyOrders />;
        }
    }
}