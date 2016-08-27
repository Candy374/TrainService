import React, {Component} from 'react';
import Page from './common/Page';
import {login, updateOpenId} from '../actions/login';

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
        if (page && page.indexOf('ReLogin') > -1) {
            const orderId = this.state.state.replace('ReLogin_', '');
            login(this.state.code, 'ReLogin').then((id) => {
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
            login(this.state.code, page).then((id) => {
                //id =123;
                // alert('open Id is' + id);
                this.props.updateOpenId(id);
            });
        }
    }

    render() {
        return <div>登录中...</div>;
    }
}

