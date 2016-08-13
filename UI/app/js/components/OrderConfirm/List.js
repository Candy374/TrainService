import React, {Component} from 'react';

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
                            return (<div className='line' key={index}>
                                <label className='label'>{goods.Name}</label>
                                <label className='width-small'>X{goods.count}</label>
                                <label className='width-small price'>￥{goods.SellPrice}</label>
                            </div>);
                        });
        fee.price = total > fee.startPrice ? fee.price : 0;
        return (
            <div className='section'>
                <div className='head line'>
                    <div className='title'>已点菜品：</div>
                </div>
                <div className='goods-list'>
                    {goodList}
                    <div className='line'>
                        <label className='label'>{fee.Name}</label>
                        <label className='width-small'></label>
                        <label className='width-small price'>￥{total}</label>
                    </div>
                </div>
                
                <div className='line'>
                    <label className='label'/>
                    <label className='width-small'>待支付</label>
                    <label className='width-small'>￥{total + fee.price}</label>
                </div>
            </div>
        );
    }
}