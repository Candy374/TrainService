﻿import React, {Component} from 'react';
import Footer from './common/Footer';
import OrderPage from './OrderPage';
import OrderInfo from './OrderInfo';
import MyOrders from './MyOrders';
import OrderDetail from './OrderDetail';
import ChooseStation from './ChooseStation';
import Login from './Login';
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
        let page = 'Order';
        if (location.hash.indexOf('#MyOrders') == 0) {
            page = 'MyOrders';
        } else if (location.hash.indexOf('#Shop') == 0) {
            page = 'Shop';
        } else if (location.hash.indexOf('#Deliver') == 0) {
            page = 'Deliver';
        } else if (location.hash.indexOf('#Login') == 0) {
            page = 'Login';
        } 

        const station = {
          StationId: 1,
          Name: "郑州东",
          StationCode: "ZAF",
          MinPrice: 30,
          PicUrl: "/imgs/ZZD_samll.jpg"
        };
        this.state = {
            page,
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
                station
            },
            orderId: null,
            stations: [station],
            submitting: false,
            openId: null,
            providerId: null
        };
        this.updateChart = this.updateChart.bind(this);
        this.submitOrder = this.submitOrder.bind(this);
        this.setCurrentOrderId = this.setCurrentOrderId.bind(this);        
        actions.getStations().then((stations) => {
            this.setState({stations})
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

    submitOrder() {
        if (this.state.submitting) {
            return;
        }

        const chart = this.state.chart;
        const {info, goods} = chart;
        let list;
        if (list instanceof Array) {
             list = goods.map(item => ({Id: item.GoodsId, Count: item.Count}));
        } else {
            list = Object.keys(goods).map(key => {            
                return {Id: goods[key].GoodsId, Count: goods[key].Count};
            });
        }

        const data = {
            OpenId: this.state.openId,
            TrainNumber: info.TrainNumber,
            CarriageNumber: '' + info.CarriageNumber,
            IsDelay: info.IsDelay,
            OrderType: 0,
            PayWay: 0,
            Comment: info.Comment,
            Contact: info.Contact,
            ContactTel: info.ContactTel,
            TotalPrice: chart.total,
            List: list
        };
        this.setState({
            submitting: true
        });

        actions.submitOrder(data, (orderId, success) => {
            if (!success) {               
                this.setState({
                    submitting: false
                }, () => this.setCurrentOrderId(orderId));
            } else {
                this.setCurrentOrderId(orderId);
            }
           
            if (data.OpenId == 'TBD') {
                actions.redirect(orderId);    
            }
        });
       //this.setCurrentOrderId(18)
    }

    updateOpenId(id) {
        this.setState({openId: id});
        
        actions.getUserInfo(id).then((user) => {
            const info = this.state.chart.info;
            info.Contact = user.Contact;
            info.ContactTel = user.ContactTel;
            info.TrainNumber = user.TrainNumber;
            info.CarriageNumber = user.CarriageNumber;
            this.setState({
                chart: this.state.chart
            });
        });
    }

    updateProviderId(id) {
      this.setState({providerId: id});
    }

    render() {
        const {openId, stations, chart, submitting, orderId, providerId } = this.state;
        switch(this.state.page){
            case 'Booking':
                return (
                    <ChooseStation stations={stations}
                                   nextPage={this.nextPage.bind(this, 'Order')}
                                   updateChart={this.updateChart} />);
            case 'Order':
                return (
                    <OrderPage  chart={chart}
                                updateChart={this.updateChart}
                                nextPage={this.nextPage.bind(this, 'Info')} />);
            case 'Info':
                return (
                    <OrderInfo chart={chart}
                               submitting={submitting}
                               submitOrder={this.submitOrder}
                               prePage={this.prePage.bind(this, 'Order')}
                               nextPage={this.nextPage.bind(this, 'Confirm')} 
                               updateChart={this.updateChart}/>);
            // case 'Confirm':
            //     return (
            //         <ConfirmPage chart={chart}
            //                      submitting={submitting}
            //                      submitOrder={this.submitOrder}
            //                      prePage={this.prePage.bind(this, 'Info')}
            //                      nextPage={this.nextPage.bind(this, 'Detail')} />);
            case 'Detail':
                return (
                    <OrderDetail setCurrentOrderId={this.setCurrentOrderId}
                                 updateChart={this.updateChart}
                                 submitOrder={this.submitOrder}
                                 nextPage={this.nextPage.bind(this)}
                                 submitting={submitting}
                                 openId={openId}
                                 id={orderId}/>);
            case 'MyOrders':
                return <MyOrders openId={openId}
                                 updateChart={this.updateChart}
                                 submitOrder={this.submitOrder}
                                 setCurrentOrderId={this.setCurrentOrderId}/>;

            case 'Shop': 
                return <ShopOrders providerId={providerId} openId={openId}/>;

            case 'Deliver': 
                return <Delivery openId={openId}/>;

            case 'Login': 
                return <Login updateProviderId={this.updateProviderId.bind(this)}
                              updateOpenId={this.updateOpenId.bind(this)}
                              nextPage={this.nextPage.bind(this)}/>;
        }
    }
}