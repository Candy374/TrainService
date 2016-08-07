import React, {Component} from 'react';
import Header from './common/Header.js';
import Footer from './common/Footer.js';

export default class OrderPage extends Component {
    render() {
        const children  = this.props.children;
        return (
            <div>
                <Header>Order</Header>
                {children}
                <Footer />
            </div>
        );
    }
}