import React, {Component} from 'react';
import {Section, ImgLine, Line, Label} from '../common/Widgets';

export const ListItem = ({url, name, count, price}) => (
    <ImgLine url={url}>
        <Label flex={true}>{name}</Label>
        <Label size='small'>{`x${count}`}</Label>
        <Label size='small'>{`￥${price}`}</Label>                      
    </ImgLine>
);

const OrderList = ({total, list, station}) => {
    return (
        <Section title='已点菜品' list={true}>
            {Object.keys(list).map((key, index) => (
                <ListItem key={index}
                          url={list[key].PictureUrl || list[key].PicUrl}
                          name={list[key].Name}
                          count={list[key].Count}
                          price={list[key].SellPrice || list[key].Price}/>)
            )}
            {/*<Line>
                <Label flex={true}>'配送费'</Label>
                <Label size='small'></Label>
                <Label size='small' className='price'>{`￥${total}`}</Label>
            </Line>*/}
            
            <Line>
                <Label flex={true}/>
                <Label size='small'>待支付</Label>
                <Label size='small' className='price'>{`￥${total}`}</Label>
            </Line>
        </Section>
    );    
};

export const RateItem = ({url, name, rate, updateRate}) => {
    const getRateClass =(current) => {
        return current <= rate ? 'active' : 'normal';
    };

    return (
        <ImgLine url={url}>
        <Label flex={true}>{name}</Label>
        {[1,2,3,4,5].map(rate => (
            <span className={'star-'+ getRateClass(rate)}
                    onClick={()=> updateRate(rate)}
                    key={rate}/>
        ))}
    </ImgLine>);
};

export default OrderList;
