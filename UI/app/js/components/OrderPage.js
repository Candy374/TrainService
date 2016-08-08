import React, {Component} from 'react';
import Footer from './common/Footer.js';
import FoodList from './FoodList.js';
import TrainInfo from './TrainInfo.js';

export default class OrderPage extends Component {
    componentWillMount() {
        this.state = {
            page: 'step1',
            total: 0
        };
    }

    order(page) {
        this.setState({
            page
        });
    }

    updateTotal(total) {
        this.setState({
            total
        });
    }

    render() {
        const children  = this.props.children;        

        let content, onBack, footer;
        switch(this.state.page){
            case 'step1':
                content = <FoodList total={this.state.total} 
                    updateTotal={this.updateTotal.bind(this)}>{children}</FoodList>;
                onBack = () => console.log('close the page');
                footer = {
                    button: {
                        label: '选好了',
                        onClick: () => this.setState({page:'step2'})
                    },
                    total: this.state.total,
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
                {content}
                <Footer {...footer}/>
            </div>
        );
    }
}