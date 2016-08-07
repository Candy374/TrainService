import React, {Component} from 'react';
import * as actions from '../actions/order.js';

export default class Orders extends Component {
    constructor() {
        super();
        this.state = {
            foodTypes: [],
            foodList: [],
            activeType: ''
        };
        this.foodMap = {};
    }

    componentWillMount() {
        actions.getOrders().then((foodTypes) => {
            if (foodTypes.length > 0) {
                const types = [];
                foodTypes.map(type => {
                    this.foodMap[type.name] = type.detail; 
                    types.push(type.name);
                })
               
                this.setState({
                    activeType: types[0],
                    foodTypes: types,
                    foodList: foodTypes[0].detail
                });
            }
        })
    }

    chooseType(type) {
        this.setState({
            foodList: this.foodMap[type]
        });
    }

    render() {
        return (
            <div className='order-content'>
            <div className='type'>
                {this.state.foodTypes.map(typeName => {
                    return (
                        <div key={typeName}
                            onClick={this.chooseType.bind(this, typeName)}
                            className={this.state.activeType == typeName ? 'active' : ''}>
                        {typeName}
                        </div>
                    )
                })}
            </div>
            <div className='list'>
                {this.state.foodList.map((food) => {
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
                        <input type='number' onClick={() => console.log('add')}></input>
                        </div>
                    )
                })} 
            </div>  
            </div>
        );
    }
}