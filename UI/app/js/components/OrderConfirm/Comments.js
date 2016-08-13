import React, {Component} from 'react';

export default class Comments extends Component {
    render() {
        return (
            <div className='section'>
                <div className='comments line'>
                    <div className='line'>
                        <div className='title'>订单备注：</div>
                    </div>
                    <div className='line'>{this.props.comments || '无'}</div>
                </div>
            </div>
        );
    }
}