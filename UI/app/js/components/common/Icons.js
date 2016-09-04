import React from 'react';

export const Star = ({active})=> {
  //color = 'red'
  return (
    <svg viewBox="200 50 300 300" width='1em' height='1em'>
      <polygon fill={active ? '#FF9800' : '#fff'} stroke={active ? '#FF9800' : '#eee'} strokeWidth='10'
              points="350,75 400,150 469,161 408,226 423,301 350,261 277,301 292,226 231,161 310,150" />
    </svg>)
};

export const Minus = () => (
    <svg width="1.5em" height="1.5em">
        <circle cx="12" cy="12" r="12" className='circle'></circle>
        <line x1="2" y1="12" x2="22" y2="12" className="line"></line>
    </svg>
);

export const Add = () => (
    <svg width="1.5em" height="1.5em">
        <circle cx="12" cy="12" r="12" className='circle'></circle>
        <line x1="12" y1="2" x2="12" y2="22" className="line"></line>
        <line x1="2" y1="12" x2="22" y2="12" className="line"></line>
    </svg>
);

export const Chart = ({label, onClick})=> (
  <label style={{float: 'left', position: 'relative'}} onClick={onClick}>
    <span style={{position: 'absolute', left: 16, zIndex: 1, top: -25}}>{label || 0}</span>
    <svg viewBox="-20 0 600 600" width="2em" height="2em" style={{position: 'absolute', top: -25}}>
    <title>test</title>
      <polygon stroke="#fff" fill='transparent' strokeWidth="30" points="100,270 150,450 450,450 500,270"></polygon>
      <circle cx="360" cy="200" r="200" fill="red">1</circle>
      <line x1="0" y1="135" x2="50" y2="135" stroke="#fff" strokeWidth="30"></line>
      <line x1="50" y1="135" x2="100" y2="270" stroke="#fff" strokeWidth="30"></line>
      <circle cx="400" cy="500" r="60" fill="#fff"></circle>
      <circle cx="200" cy="500" r="60" fill="#fff"></circle>
    </svg>
  </label>
);
