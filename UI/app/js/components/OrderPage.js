import React, {Component} from 'react';
import Header from './common/Header.js';
import Footer from './common/Footer.js';
import Orders from './Orders.js';

export default class OrderPage extends Component {
    render() {
        const children  = this.props.children;
        const footerButton = {
            label: 'Finish',
            onClick: () => console.log(`That's all`)
        };

        return (
            <div>
                <Header>Order</Header>
                <Orders>{children}</Orders>
                <Footer button={footerButton} total={0}/>
            </div>
        );
    }
}