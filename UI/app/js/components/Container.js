import React, {Component} from 'react';
import Footer from './common/Footer.js';
import OrderPage from './OrderPage';
import OrderInfo from './OrderInfo.js';
import MyOrders from './MyOrders.js';
import OrderConfirmPage from './OrderConfirm/Page.js';
export default class Container extends Component {
    componentWillMount() {
        this.state = {
            page: 1,
            chart: {}
        };
        this.nextPage = this.nextPage.bind(this);
        this.prePage = this.prePage.bind(this);
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

    updateChart(chart) {
        this.setState({
            chart
        });
    }

    pay() {
       this.nextPage();
    }

    render() {
        switch(this.state.page){
            case 1:
                return (
                    <OrderPage  chart={this.state.chart}
                                updateTotal={this.updateTotal}
                                updateChart={this.updateChart.bind(this)}
                                nextPage={this.nextPage} />);
            case 2:
                return (
                    <OrderInfo prePage={this.prePage} 
                               nextPage={this.nextPage} 
                               pay={this.pay.bind(this)}/>);
            case 3:
                return <OrderConfirmPage />;
            case 4:
                return <MyOrders />;
        }
    }
}