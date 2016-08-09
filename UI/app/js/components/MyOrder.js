import React, {Component} from 'react';
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
            <div className='order-page'>
                <FoodList total={total} 
                    updateChart={updateChart}
                    updateTotal={updateTotal}></FoodList>
                <Footer {...footer}/>
            </div>
        );
    }
}