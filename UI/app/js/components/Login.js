import React, {Component} from 'react';
import Page from './common/Page';
import {login, renewOpenId} from '../actions/login';

export default class Login extends Component {
    componentWillMount() {
        const hash = location.hash;
        const query = hash.split('?')[1];
        const states = query.split('&');
        this.state = {};

        states.map(item=> {
            const parts = item.split('=');
            this.state[parts[0]] = parts[1]
        });

        if (this.state.state && this.state.state.indexOf('ReLogin') > -1) {
            const orderId = this.state.state.replace('ReLogin_', '');
            renewOpenId(orderId).then(id => {
                this.props.updateOpenId(id, 'do_not_know');
            }, err => {
                alert('服务器出现错误， 请重新打开');
                console.log(err);
            })
        }

        this.props.nextPage(this.state.state);
        login(this.state.code, this.state.state).then((id) => {
            alert('open Id is' + id)
            this.props.updateOpenId(id);
        });
    }

    render() {
        return <div>登录中...</div>;
    }
}

