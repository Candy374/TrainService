import React, {Component} from 'react';
import Page from './common/Page';
import {login, updateOpenId} from '../actions/login';
import {getProviderId} from '../actions/shopOrders';

export default class Login extends Component {
    componentWillMount() {
        const hash = location.hash;
        const query = hash.split('?')[1];
        const states = query.split('&');
        this.state = {};

        states.map(item=> {
            const parts = item.split('=');
            this.state[parts[0]] = parts[1];
        });

        const page = this.state.state;
        const code = this.state.code;
        if (page && page.indexOf('ReLogin') > -1) {
            const orderId = this.state.state.replace('ReLogin_', '');
            login(code, 'ReLogin').then((id) => {
                // alert('renew open Id is' + id);
                updateOpenId(orderId).then(id => {
                    this.props.updateOpenId(id);
                });
            }, err => {
                alert('服务器出现错误， 请重新打开');
                console.log(err);
            });
        } else {
            this.props.nextPage(page);
            login(code, page).then((id) => {
                console.log('open Id is' + id);
                this.props.updateOpenId(id);
            });

            if (page == 'Shop') {
                getProviderId(code).then(providerId => {
                    this.props.updateProviderId(providerId);
                });
            }
        }
        // this.props.updateOpenId('ouzHawBv2svApr1IiNxXykpmAuI0');
        // this.props.updateOpenId('ouzHawP6e4hKHYhLVJO0sej3Akng');
        // this.props.updateProviderId(3);
        // this.props.nextPage(page)
    }

    render() {
        return <div>登录中...</div>;
    }
}

