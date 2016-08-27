import React, {Component} from 'react';
import {Section, Line} from '../common/Widgets';

const Comments = ({Comment}) =>(
    <Section title='订单备注' className='comments'>
        <Line>{Comment || '无'}</Line>
    </Section>
);

export default Comments;
