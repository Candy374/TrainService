import React, {Component} from 'react';
import {Section, ImgLine, Line, Label, Price} from '../common/Widgets';

export const SummaryLine = ({left, label, price, className}) => (
    <Line className={className ? className : ''}>
        <Label flex={true}>{left}</Label>
        <Label size='small' align='end'>{label || '共计'}:</Label>
        <Price size='small' className='price' align='end' price={price}/>
    </Line>
);

export const NumberLine = ({item}) => (
    <Line>
        <Label flex={true}>{item.Name}</Label>
        <Label size='small' align='end'>{`x${item.Count}`}</Label>
    </Line>
);

export const ListItem = ({url, name, count, price}) => (
    <ImgLine url={url}>
        <Label flex={true}>{name}</Label>
        <Label size='small' align='end'>{`x${count}`}</Label>
        <Label size='small' align='end'>{`￥${price}`}</Label>                      
    </ImgLine>
);

const OrderList = ({total, list}) => (
  <Section title='已点菜品' list={true}>
    {Object.keys(list).map((key, index) => (
        <ListItem key={index}
                  url={list[key].PictureUrl || list[key].PicUrl}
                  name={list[key].Name}
                  count={list[key].Count}
                  price={list[key].SellPrice || list[key].Price}/>)
    )}
    <SummaryLine label='配送费' price='0' />
    <SummaryLine label='待支付' price={total} />
  </Section>
);

export const OrderListNoImg = ({total, list, totalLabel, short}) => {
  let arrayList = list instanceof Array ? list : Object.keys(list).map(key => list[key]);

  const lineClass = short ? 'short' : '';
  return (
    <Section list={true}>
      {!short && <Line><Label>已点菜品：</Label></Line>}
      {arrayList.map((item, index) => (
        <Line key={index} className={lineClass}>
          <Label flex={true}>{item.Name}</Label>
          <Label size='small' align='end'>{`x${item.Count}`}</Label>
          <Label size='small' align='end'>{`￥${item.SellPrice || item.Price}`}</Label>
        </Line>
      ))}
      <SummaryLine label={totalLabel || '待支付'} price={total} className={lineClass}/>
    </Section>
  );
};

export const RateItem = ({url, name, rate, updateRate}) => {
    const getRateClass =(current) => {
        return current <= rate ? 'active' : 'normal';
    };

    return (
      <Line align={name ? 'start' : 'center'}>
        {name && <Label flex={true}>{name}</Label>}
        {[1,2,3,4,5].map(rate => (
            <span className={'star-'+ getRateClass(rate)}
                    onClick={()=> updateRate(rate)}
                    key={rate}/>
        ))}
      </Line>
    );
};

export default OrderList;
