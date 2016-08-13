import React, {Component} from 'react';

export default class OrderList extends Component {
    render() {
        const orderList = [{
            name:'水煮鱼',
            num: 1,
            price: 25
        },{
            name:'皮蛋豆腐',
            num: 1,
            price: 9
        },{
            name:'水煮鱼',
            num: 1,
            price: 25
        },{
            name:'皮蛋豆腐',
            num: 1,
            price: 9
        },
        {
            name:'水煮鱼',
            num: 1,
            price: 25
        },{
            name:'皮蛋豆腐',
            num: 1,
            price: 9
        }];
        const fee = {
            name:'配送费',
            num: '1',
            price: 9,
            startPrice: 20
        };
        let total = 0.0;
        const goodList = orderList.map((food, index) => {
                            const price = food.price * food.num;
                            total += price;
                            return (<div className='line' key={index}>
                                <label className='label'>{food.name}</label>
                                <label className='width-small'>X{food.num}</label>
                                <label className='width-small price'>￥{price}</label>
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
                        <label className='label'>{fee.name}</label>
                        <label className='width-small'></label>
                        <label className='width-small price'>￥{total}</label>
                    </div>
                </div>
                
                <div className='line fee'>
                    <label className='label'>待支付 ￥{total + fee.price}</label>
                </div>
            </div>
        );
    }
}