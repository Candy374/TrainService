import React, {Component} from 'react';
import * as actions from '../actions/order.js';
import NumberInput from './common/NumberInput';

export default class FoodList extends Component {
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
        actions.getTypes().then(types => {      
            this.types = types;
            this.setState({
                activeType: types[0].ID,
                foodTypes: this.types
            });

            types.map(type => {
                this.foodMap[type.ID] = type;
                actions.getGoodsList(type.ID).then(foodList => {
                    this.foodMap[type.ID].list = foodList;
                });
            });
            return types;
        }).then((types) => {
            actions.getGoodsList(types[0]).then(foodList => {
                this.foodMap[types[0].ID].list = foodList;
                this.setState({
                    foodList
                });
            });
        });
    }

    chooseType(type) {
        this.setState({
            foodList: this.foodMap[type.ID].list,
            activeType: type.ID
        });
    }

    add(food, count){
        const chart = this.chart;
        food.OrderCount = count;
        chart[food.GoodsId] = food;
        //this.foodMap[this.state.activeType].list[food.index].OrderCount = count;
        let total = 0;
        Object.keys(chart).map(key => {
            total += chart[key].OrderCount * chart[key].PurchasePrice
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
                            <div key={type.ID}
                                onClick={this.chooseType.bind(this, type)}
                                className={this.state.activeType == type.ID ? 'active' : ''}>
                            {type.DisplayName}
                            </div>
                        )
                    })}
                </div>
                <div className='list'>
                    {this.state.foodList.map((food, index) => {
                        this.chart[food.GoodsId] = this.chart[food.GoodsId] || food;
                        food.count = this.chart[food.GoodsId].count = (food.count || 0);
                        food.index = index;
                        return (
                            <div key={food.GoodsId} className='item'>
                                <img src= {food.PictureUrl}  className='img'/>
                                <div className='descriptions'>
                                    <label className='name'>{food.Name}</label>
                                    <div className='detail'>
                                        {`￥${food.SellPrice}     `.substr(0, 5)}
                                        {`月售${food.sale}     `.substr(0, 5)}
                                        {`好评率${food.Rating}%`}
                                </div>
                                </div>
                                <NumberInput count={food.OrderCount} updateCount={(OrderCount) => this.add(food, OrderCount)}/>
                            </div>
                        )
                    })} 
                </div>  
            </div>
        );
    }
}