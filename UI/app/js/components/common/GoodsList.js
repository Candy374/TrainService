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
                          url={list[key].PictureUrl}
                          name={list[key].Name}
                          count={list[key].count}
                          price={list[key].SellPrice}/>)
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

export class RateItem extends Component {
    componentWillMount() {
        this.state = {
            rate: this.props.rate || 0
        }
    }

    getRateClass(current) {
        return current <= this.state.rate ? 'active' : 'normal';
    };

    render() {
        const {url, name} = this.props;

        return (
            <ImgLine url={url}>
            <Label flex={true}>{name}</Label>
            {[1,2,3,4,5].map(rate => (
                <span className={'star-'+ this.getRateClass(rate)}
                      onClick={()=> this.setState({rate})}
                      key={rate}/>
            ))}
        </ImgLine>);
    }
}

export default OrderList;
