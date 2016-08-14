import React, {Component} from 'react';
import {Section, Line, Label} from '../common/Widgets';

export default class OrderList extends Component {
    render() {
        const {total, list, station} = this.props;
        const goodList = Object.keys(list).map((key, index) => (
                                <Line key={index}>
                                    <Label flex={true}>{list[key].Name}</Label>
                                    <Label size='small'>x{list[key].count}</Label>
                                    <Label size='small' className='price'>￥{list[key].SellPrice}</Label>
                                </Line>));
        return (
            <Section title='已点菜品：'>
                <div className='goods-list'>
                    {goodList}
                    {/*<Line>
                        <Label flex={true}>'配送费'</Label>
                        <Label size='small'></Label>
                        <Label size='small' className='price'>{`￥${total}`}</Label>
                    </Line>*/}
                </div>
                
                <Line>
                    <Label flex={true}/>
                    <Label size='small'>待支付</Label>
                    <Label size='small'>{`￥${total}`}</Label>
                </Line>
            </Section>
        );
    }
}