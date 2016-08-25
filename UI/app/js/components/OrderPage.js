import React, {Component} from 'react';
import Footer from './common/Footer';
import Page from './common/Page';
import * as actions from '../actions/order.js';
import NumberInput from './common/NumberInput';
import  * as Constants from '../constants/system';
import {Section, Line, DescLine, Label, Img, Price} from './common/Widgets';

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

    add(food, Count){
        const chart = this.props.chart;
        food.Count = Count;
        chart.goods[food.GoodsId] = food;
        let total = 0;
        let count = 0;
        Object.keys(chart.goods).map(key => {
            if (key != 'total') {
                count += chart.goods[key].Count;
                total += chart.goods[key].Count * chart.goods[key].SellPrice
            }
        });
        chart.total = total;
        chart.count = count;
        this.props.updateChart(chart);
    }
    
    render() {       
        const {goods: chart, total, station } = this.props.chart;
        const footer = {
            button: {
                label: '去结算',
                onClick: this.props.nextPage,
                disabled: total < station.MinPrice
            },
            
            left: (<label className='total'>
                    <Price price={total}/>
                    {total < station.MinPrice && 
                        <span className='desc'>{`差${station.MinPrice - total}元起送`}</span>}
                </label>)
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
                        food.Count = chart[food.GoodsId] && chart[food.GoodsId].Count || food.Count || 0;
                     
                        return (
                            <div className='item' key={food.GoodsId}>
                                <div className='desc'>
                                    <Img src= {food.PictureUrl}  className='img'/>
                                    <div >
                                        <Label size='auto' type='title'>{food.Name}</Label> 
                                        <Label type='desc'>{`好评率${food.Rating}%`}</Label>
                                        <Label size='small' type='desc'>{`月售${food.OrderCount}`}</Label>
                                    </div>
                                </div>
                                
                                <div className="detail">
                                    <Price price={food.SellPrice}/>
                                    <NumberInput count={food.Count} updateCount={(Count) => this.add(food, Count)}/>
                                </div> 
                            </div>
                        )
                    })}
                </div>  
            </Page>
        );
    }
}