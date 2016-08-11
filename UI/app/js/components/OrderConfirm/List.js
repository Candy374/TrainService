import React, {Component} from 'react';

export default class OrderList extends Component {
    render() {
        const orderList = [{
            name:'水煮鱼',
            num: '1',
            price: '25'
        },{
            name:'皮蛋豆腐',
            num: '1',
            price: '9'
        }];
        const fee = {
            name:'配送费',
            num: '1',
            price: '9',
            startPrice: '20'
        };

        let total;
        return (
            <div className='section'>
                <div className='head line'>
                    <div className='title'>已点菜品：</div>
                </div>
                <div className='goods-list'>{
                    orderList.map((food) => {
                        const price = food.price * food.num;
                        total += price;
                        return (<div className='line'>
                            <label className='label'>{food.name}</label>
                            <label className='number'>X{food.num}</label>
                            <label className='price'>￥{price}</label>
                        </div>);
                    })
                }
                </div>
                <div className='line'>
                    <label className='name'>{fee.name}</label>
                    <label className='number'>{' '}</label>
                    <label className='price'>￥{total > fee.startPrice ? fee.price : 0}</label>
                </div>
                <div className='line'>
                待支付 ￥{total + (total > fee.startPrice ? fee.price : 0)}
                </div>
            </div>
        );
    }
}