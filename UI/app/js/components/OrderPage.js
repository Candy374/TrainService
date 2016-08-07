import React, {Component} from 'react';
import Header from './common/Header.js';
import Footer from './common/Footer.js';
import FoodList from './FoodList.js';
import TrainInfo from './TrainInfo.js';

export default class OrderPage extends Component {
    componentWillMount() {
        this.state = {
            page: 'step1'
        };
    }

    order(page) {
        this.setState({
            page
        });
    }

    render() {
        const children  = this.props.children;        

        let content, onBack, footer;
        switch(this.state.page){
            case 'step1':
                content = <FoodList>{children}</FoodList>;
                onBack = () => console.log('close the page');
                footer = {
                    button: {
                        label: '选好了',
                        onClick: () => this.setState({page:'step2'})
                    },
                    total: 0
                };
                break;
            case 'step2':
                content = <TrainInfo>{children}</TrainInfo>;
                onBack = () => this.setState({page:'step1'});
                footer = {
                    button: {
                        label: '微信支付',
                        onClick: () => console.log('go to pay page')
                    }
                };
                break;
        }

        return (
            <div>
                <Header onBack={onBack}>Order</Header>
                {content}
                <Footer {...footer}/>
            </div>
        );
    }
}