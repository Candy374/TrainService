import React, {Component} from 'react';
import Footer from './common/Footer';
import Page from './common/Page';
import * as actions from '../actions/order.js';
import NumberInput from './common/NumberInput';
import  * as Constants from '../constants/system';
import {Section, Line, ImgLine, Label} from './common/Widgets';

export default class OrderPage extends Component {
    componentWillMount() {
        this.state = {
            goodsTypes: [],
            goodsList: [],
            activeType: 1
        };
        this.chart = {};
        this.foodMap = {};
        this.promiseList = [];

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
        });        
    }

    chooseType(type) {
        this.setState({
            goodsList: this.foodMap[type.ID].list,
            activeType: type.ID
        });
    }

    add(food, count){
        const chart = this.props.chart;
        food.count = count;
        chart.goods[food.GoodsId] = food;
        let total = 0;
        Object.keys(chart.goods).map(key => {
            if (key != 'total') {
                total += chart.goods[key].count * chart.goods[key].SellPrice
            }
        });
        chart.total = total;
        this.props.updateChart(chart);
    }
    
    render() {       
        const chart = this.props.chart.goods;
        const total = this.props.chart.total;
        const footer = {
            button: {
                label: '选好了',
                onClick: this.props.nextPage,
                disabled: total == 0
            },
            
            left: (
                <label className='total'>{`共: ￥${total} 元`}</label>)
        };

        return (
            <Page footer={footer} className='order-content'>
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
                            food.count = chart[food.GoodsId] && chart[food.GoodsId].count || food.count || 0;
                            return (
                                <ImgLine url={food.PictureUrl} type='desc' key={food.GoodsId}>
                                        <Label size='auto'>{food.Name}</Label>
                                        <div className="detail">
                                            <div className="left">
                                                {`月售${food.OrderCount}`}
                                                {`好评率${food.Rating}%`}
                                                <div className="price">{`￥${food.SellPrice}`}</div>
                                            </div>

                                            <div className="number-input">
                                                <NumberInput count={food.count} updateCount={(count) => this.add(food, count)}/>
                                            </div>
                                        </div>                         
                                </ImgLine>
                            )
                        })}
                    </div>  
            </Page>
        );
    }
}