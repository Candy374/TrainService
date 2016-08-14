import React, {Component} from 'react';
import {Section, Line} from '../common/Widgets';

export default class Comments extends Component {
    render() {
        return (
            <Section title='订单备注：' className='comments'>
                <Line>{this.props.Comment || '无'}</Line>
            </Section>
        );
    }
}