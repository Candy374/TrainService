import React, {Component} from 'react';

export default class Comments extends Component {
    render() {
        return (
            <div className='section'>
                <div className='head line'>
                    <div className='title'>用餐人数</div>
                    <div>2</div>
                </div>
                <div className='comments line'>
                    <div className='label'>订单备注：</div>
                    <textarea></textarea>
                </div>
            </div>
        );
    }
}