import React from 'react';
import {basicUrl} from '../../constants/system';

export const Label = ({size, children, flex, align}) => {
    const classes = [`width-${size || 'medium'}`, `align-${align || 'start'}`];
    if (flex) {
        classes.push('flex');
    }
    
    return  <label className={classes.join(' ')}>{children}</label>;
};

export const Line = ({children, className, imgUrl, align}) => {
    const classes = ['line', `align-${align || 'start'}`];
    if (className) {
        classes.push(className);
    }
    return <div className={classes.join(' ')}> {children}</div>;
};

export const ImgLine = ({children, className, url, type}) => {
    const classes = ['item'];
    if (className) {
        classes.push(className);
    }

    return (
        <div className={classes.join(' ')}>
            <img src= {basicUrl + url}  className='img'/>
            <div className={type || 'line'}>{children}</div>
        </div>
    );  
};

export const Section = ({ title, children, list }) => {
    const classes = ['section'];
    if (list) {
        classes.push('list');
    }
    return (
        <div className={classes.join(' ')}>
            {title && 
                <div className='head'>
                    <div className='title'>{title}</div>
                </div>
            }
            {children}
        </div>
    )
};
