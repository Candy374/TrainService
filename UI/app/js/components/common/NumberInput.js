import React, {Component} from 'react';
import {Add, Minus} from './Icons';

const NumberInput = ({updateCount, count}) => (
    <div className='number-input'>
        <button onClick={() => updateCount(Math.max(count - 1, 0))}
            style={{visibility: count == 0 ? 'hidden' : 'visible'}}>
            <Minus />
        </button>       
        <label style={{visibility: count == 0 ? 'hidden' : 'visible'}}>{count}</label>              
        <button onClick={() => updateCount(Math.max(count + 1, 0))}>
            <Add />
        </button>
    </div>
);

// <button onClick={() => updateCount(Math.max(count - 1, 0))} 
//                 style={{visibility: count == 0 ? 'hidden' : 'visible'}}>-</button>

export default NumberInput;