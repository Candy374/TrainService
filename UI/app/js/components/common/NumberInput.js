import React, {Component} from 'react';

const NumberInput = ({updateCount, count}) => (
    <div className='number-input'>
        <button onClick={() => updateCount(Math.max(count - 1, 0))} 
                style={{visibility: count == 0 ? 'hidden' : 'visible'}}>-</button>
        <label >{count}</label>              
        <button onClick={() => updateCount(Math.max(count + 1, 0))}>+</button>
    </div>
);

export default NumberInput;