import React, {Component} from 'react';
import * as actions from '../actions/order.js';
import NumberInput from './common/NumberInput';

export default class Orders extends Component {
    constructor() {
        super();
        this.state = {
            foodTypes: [],
            foodList: [],
            activeType: '',
                 
        };
        this.chart = {};
        this.foodMap = {};
    }

    componentWillMount() {
        actions.getOrders().then((foodTypes) => {
            if (foodTypes.length > 0) {
                const types = [];
                foodTypes.map(type => {
                    this.foodMap[type.id] = type.detail; 
                    this.foodMap[type.id + 'map'] = {};
                    types.push(type);
                })
               
                this.setState({
                    activeType: types[0].id,
                    foodTypes: types,
                    foodList: foodTypes[0].detail
                });
            }
        })
    }

    chooseType(type) {
        this.setState({
            foodList: this.foodMap[type.id],
            activeType: type.id
        });
    }

    add(food, count){
        const chart = this.chart;
        food.count = count;
        chart[food.id] = food;
        this.foodMap[this.state.activeType][food.index].count = count;
        let total = 0;
        Object.keys(chart).map(key => {
            total += chart[key].count * chart[key].price
        })
        this.props.updateTotal(total);
        this.props.updateChart(chart);
    }

    render() {
        return (
            <div className='order-content'>
                <div className='type'>
                    {this.state.foodTypes.map(type => {
                        return (
                            <div key={type.id}
                                onClick={this.chooseType.bind(this, type)}
                                className={this.state.activeType == type.id ? 'active' : ''}>
                            {type.name}
                            </div>
                        )
                    })}
                </div>
                <div className='list'>
                    {this.state.foodList.map((food, index) => {
                        this.chart[food.id] = this.chart[food.id] || food;
                        food.count = this.chart[food.id].count || food.count || 0;
                        food.index = index;
                        return (
                            <div key={food.name} className='item'>
                            <div className='img'>
                            {food.img} 
                            </div>
                            <div className='descriptions'>
                                <label className='name'>{food.name}</label>
                                <div className='detail'>
                                    {`￥${food.price}     `.substr(0, 5)}
                                    {`月售${food.sale}     `.substr(0, 5)}
                                    {`好评率${food.rate}%`}
                            </div>
                            </div>
                            <NumberInput count={food.count} updateCount={(count) => this.add(food, count)}/>
                            </div>
                        )
                    })} 
                </div>  
            </div>
        );
    }
}