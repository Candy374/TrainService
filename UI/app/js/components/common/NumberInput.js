import React, {Component} from 'react';

const NumberInput = ({updateCount, count}) => (
    <div className='number-input'>
        <button onClick={() => updateCount(Math.max(count - 1, 0))}
            style={{visibility: count == 0 ? 'hidden' : 'visible'}}>
            <svg width="1.5em" height="1.5em" viewPort="0 0 20 20">
                <circle cx="12" cy="12" r="12" className='circle'></circle>
                <line x1="2" y1="12" x2="20" y2="12" className="line"></line>
            </svg>
        </button>       
        <label style={{visibility: count == 0 ? 'hidden' : 'visible'}}>{count}</label>              
        <button onClick={() => updateCount(Math.max(count + 1, 0))}>
            <svg width="1.5em" height="1.5em" viewPort="0 0 20 20">
                <circle cx="12" cy="12" r="12" className='circle'></circle>
                <line x1="12" y1="2" x2="12" y2="20" className="line"></line>
                <line x1="2" y1="12" x2="20" y2="12" className="line"></line>
            </svg>
        </button>
    </div>
);

// <button onClick={() => updateCount(Math.max(count - 1, 0))} 
//                 style={{visibility: count == 0 ? 'hidden' : 'visible'}}>-</button>

export default NumberInput;