import React, {Component} from 'react';
import {Section, Line, Label} from '../common/Widgets';

export default class OrderList extends Component {
    render() {
        const fee = {
            Name:'配送费',
            num: '1',
            price: 9,
            startPrice: 20
        };
        const {total, list} = this.props;
        const goodList = Object.keys(list).map((key, index) => {
                            const goods = list[key];
                            return (
                                <Line key={index}>
                                    <Label flex={true}>{goods.Name}</Label>
                                    <Label size='small'>x{goods.count}</Label>
                                    <Label size='small' className='price'>￥{goods.SellPrice}</Label>
                                </Line>);
                        });
        fee.price = total > fee.startPrice ? fee.price : 0;
        return (
            <Section title='已点菜品：'>
                <div className='goods-list'>
                    {goodList}
                    <Line>
                        <Label flex={true}>{fee.Name}</Label>
                        <Label size='small'></Label>
                        <Label size='small' className='price'>{`￥${total}`}</Label>
                    </Line>
                </div>
                
                <Line>
                    <Label flex={true}/>
                    <Label size='small'>待支付</Label>
                    <Label size='small'>{`￥${total + fee.price}`}</Label>
                </Line>
            </Section>
        );
    }
}