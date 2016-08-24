import React, {Component} from 'react';

const NumberInput = ({updateCount, count}) => (
    <div className='number-input'>
        <button onClick={() => updateCount(Math.max(count - 1, 0))}
            style={{visibility: count == 0 ? 'hidden' : 'visible'}}>
            <svg width="30px" height="30px" >
                <circle cx="15px" cy="15px" r="15px" className='circle'/>
                <line x1="5" y1="15" x2="25" y2="15" className='line'></line>
            </svg>
        </button>       
        <label style={{visibility: count == 0 ? 'hidden' : 'visible'}}>{count}</label>              
        <button onClick={() => updateCount(Math.max(count + 1, 0))}>
            <svg width="30px" height="30px" >
                <circle cx="15px" cy="15px" r="15px" className='circle'/>
                <line x1="15" y1="5" x2="15" y2="25" className='line'></line>
                <line x1="5" y1="15" x2="25" y2="15" className='line'></line>
            </svg>
        </button>
    </div>
);

// <button onClick={() => updateCount(Math.max(count - 1, 0))} 
//                 style={{visibility: count == 0 ? 'hidden' : 'visible'}}>-</button>

export default NumberInput;