import React, {Component} from 'react';
import Footer from './common/Footer.js';
import Page from './common/Page.js';
import {Section, Line, Label, Button} from './common/Widgets';
import {SummaryLine} from './common/GoodsList';
import NumberInput from './common/NumberInput';

export default class OrderInfo extends Component {
    render() {
        const chart = this.props.chart;
        
        const footer = {
            button: {
                label: '去结算',
                onClick: this.props.submitOrder,
                disabled: this.props.submitting
            },
            left: 
        };
        
        return (
            <Page className='order-info' footer={footer}>         
                <div className='list'>
                    {chart.goods.map((food) => {
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
                    <SummaryLine label='共计' price={chart.total} className='short'/>
                </div>  
            </Page>
        );
    }
}