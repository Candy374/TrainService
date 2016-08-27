import React, {Component} from 'react';

const NumberInput = ({updateCount, count}) => (
    <div className='number-input'>
        <button onClick={() => updateCount(Math.max(count - 1, 0))}
            style={{visibility: count == 0 ? 'hidden' : 'visible'}}>
            <svg width="20px" height="20px" >
                <circle cx="10px" cy="10px" r="10px" className='circle'/>
                <line x1="3" y1="10" x2="17" y2="10" className='line'></line>
            </svg>
        </button>       
        <label style={{visibility: count == 0 ? 'hidden' : 'visible'}}>{count}</label>              
        <button onClick={() => updateCount(Math.max(count + 1, 0))}>
            <svg width="20px" height="20px" >
                <circle cx="10px" cy="10px" r="10px" className='circle'/>
                <line x1="10" y1="3" x2="10" y2="17" className='line'></line>
                <line x1="3" y1="10" x2="17" y2="10" className='line'></line>
            </svg>
        </button>
    </div>
);

// <button onClick={() => updateCount(Math.max(count - 1, 0))} 
//                 style={{visibility: count == 0 ? 'hidden' : 'visible'}}>-</button>

export default NumberInput;