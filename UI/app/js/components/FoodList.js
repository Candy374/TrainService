import React, {Component} from 'react';
import * as actions from '../actions/order.js';
import NumberInput from './common/NumberInput';

const hostname = '123.207.164.202';
const basicUrl = `http://${hostname}/TrainService`;

export default class FoodList extends Component {
    constructor() {
        super();
        this.state = {
            goodsTypes: [],
            goodsList: [],
            activeType: 1,
                 
        };
        this.chart = {};
        this.foodMap = {};
        this.promiseList = [];
    }

    componentWillMount() {
        actions.getTypes().then(types => {      
            this.types = types;
            this.setState({
                goodsTypes: this.types
            });

            types.map(type => {
                this.foodMap[type.ID] = type;
                this.foodMap[type.ID].list = [];
            });
        }).then(() => {
            actions.getGoodsList().then(goodsList => {
                goodsList.map(goods => {
                    goods.Tags.forEach(tagId => {
                        if (tagId)
                         this.foodMap[tagId].list.push(goods)
                     })
                });
            }).then(() => {
                this.setState({
                    goodsList: this.foodMap[this.state.activeType].list
                })
            })
        })

        
    }

    chooseType(type) {
        this.setState({
            goodsList: this.foodMap[type.ID].list,
            activeType: type.ID
        });
    }

    add(food, count){
        const chart = this.chart;
        food.count = count;
        chart[food.GoodsId] = food;
        //this.foodMap[this.state.activeType].list[food.index].count = count;
        let total = 0;
        Object.keys(chart).map(key => {
            total += chart[key].count * chart[key].SellPrice
        })
        this.props.updateTotal(total);
        this.props.updateChart(chart);
    }

    render() {
        return (
            <div className='order-content'>
                <div className='type'>
                    {this.state.goodsTypes.map(type => {
                        return (
                            <div key={type.ID}
                                onClick={this.chooseType.bind(this, type)}
                                className={this.state.activeType == type.ID ? 'active item' : 'item'}>
                            {type.DisplayName}
                            </div>
                        )
                    })}
                </div>
                <div className='list'>
                    {this.state.goodsList.map((food, index) => {
                        food.count = food.count || 0;
                        food.index = index;
                        return (
                            <div key={food.GoodsId} className='item'>
                                <img src= {basicUrl + food.PictureUrl}  className='img'/>
                                <div className="descriptions">
                                    <label className="name">
                                        {food.Name}
                                    </label>
                                    <div className="detail">
                                        <div className="left">
                                            {`月售${food.OrderCount}`}
                                            {`好评率${food.Rating}%`}
                                            <div className="price">
                                                {`￥${food.SellPrice}`}
                                            </div>
                                        </div>

                                        <div className="number-input">
                                            <NumberInput count={food.count} updateCount={(count) => this.add(food, count)}/>
                                        </div>
                                    </div>    
                                </div>                           
                            </div>
                        )
                    })} 
                </div>  
            </div>
        );
    }
}