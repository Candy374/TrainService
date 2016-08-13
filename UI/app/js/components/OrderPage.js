import React, {Component} from 'react';
import Footer from './common/Footer.js';
import FoodList from './FoodList.js';
import TrainInfo from './TrainInfo.js';
import Page from './common/Page.js';
export default class OrderPage extends Component {
    componentWillMount() {
        this.state = {
            total: 0
        };
    }
    
    updateTotal(total) {
        this.setState({
            total
        });
    }
    
    render() {
        const {nextPage, updateChart}  = this.props;        
        const total = this.state.total;
        const footer = {
            button: {
                label: '选好了',
                onClick: nextPage,
                className: 'active',
                disabled: total == 0
            },
            total
        };

        return (
            <Page footer={footer}>
                <FoodList total={total} 
                    updateChart={updateChart}
                    updateTotal={this.updateTotal.bind(this)}></FoodList>
            </Page>
        );
    }
}