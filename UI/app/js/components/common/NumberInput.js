import React, {Component} from 'react';

const NumberInput = ({updateCount, count}) => (
    <div className='number-input'>
        <button onClick={() => updateCount(Math.max(count - 1, 0))} 
                style={{display: count > 0 ? 'inline-block' : 'none'}}>-</button>
        <label >{count}</label>              
        <button onClick={() => updateCount(Math.max(count + 1, 0))}>+</button>
    </div>
);

export default NumberInput;