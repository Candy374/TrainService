import React, {Component} from 'react';
import Footer from './common/Footer.js';
import FoodList from './FoodList.js';
import TrainInfo from './TrainInfo.js';

export default class OrderPage extends Component {
    render() {
        const {nextPage, updateChart, updateTotal, total}  = this.props;        

        const footer = {
                    button: {
                        label: '选好了',
                        onClick: nextPage
                    },
                    total: total,
                };

        return (
            <div>
                <FoodList total={total} 
                    updateChart={updateChart}
                    updateTotal={updateTotal}></FoodList>
                <Footer {...footer}/>
            </div>
        );
    }
}