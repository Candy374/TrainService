import React, {Component} from 'react';
import Footer from './common/Footer.js';
import OrderPage from './OrderPage';
import TrainInfo from './TrainInfo.js';
import MyOrders from './MyOrders.js';
import OrderConfirmPage from './OrderConfirm/Page.js';
export default class Container extends Component {
    componentWillMount() {
        this.state = {
            page: 1,
            chart: {},
            total: 0
        };
        this.nextPage = this.nextPage.bind(this);
        this.updateTotal = this.updateTotal.bind(this);
    }

    nextPage() {
        this.setState({
            page: this.state.page + 1
        });
    }

    updateTotal(total) {
        this.setState({
            total
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
                    <OrderPage  total={this.state.total}
                                updateTotal={this.updateTotal}
                                updateChart={this.updateChart.bind(this)}
                                nextPage={this.nextPage} />);
            case 2:
                return <TrainInfo nextPage={this.nextPage} pay={this.pay.bind(this)}/>;
            case 3:
                return <OrderConfirmPage />;
            case 4:
                return <MyOrders />;
        }
    }
}