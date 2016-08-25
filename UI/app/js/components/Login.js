import React, {Component} from 'react';
import Page from './common/Page';
import {login} from '../actions/login';

export default class Login extends Component {
    componentWillMount() {
        alert(location.hash)
        const hash = location.hash;
        const query = hash.split('?')[1];
        const states = query.split('&');
        this.state = {};

        states.map(item=> {
            const parts = item.split('=');
            this.state[parts[0]] = parts[1]
        });
        login(this.state.code, this.state.state).then((id) => {
            this.props.updateOpenId(id);
        });
    }

    render() {
        return <div>登录中...</div>;
    }
}

