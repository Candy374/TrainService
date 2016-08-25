import React from 'react';
import {basicUrl} from '../../constants/system';

export {Button, SmallButton} from './Button';
 
export const Label = ({size, children, flex, align, className, status, type}) => {
    const classes = [`width-${size || 'medium'}`, `align-${align || 'start'}`];
    if (flex) {
        classes.push('flex');
    }

    if (className) {
        classes.push(className);
    }

    if (status) {
        classes.push(status);
    }

    if (type) {
        classes.push(type);
    }
    
    return  <label className={classes.join(' ')}>{children}</label>;
};

export const Line = ({children, className, align = 'start', direction, style}) => {
    const classes = ['line', `align-${align}`, `direction-${direction || 'row'}`];
    if (className) {
        classes.push(className);
    }
    return <div className={classes.join(' ')} style={style}> {children}</div>;
};

export const ImgLine = ({children, className, url, type, onClick, imgClassName, bottom}) => {
    const classes = ['item'];
    if (className) {
        classes.push(className);
    }

    return (
        <div className={classes.join(' ')} onClick={onClick}>
            <img src= {basicUrl + url}  className={imgClassName || 'img'}/>
            <div className='line'>{children}</div>
            {bottom}
        </div>
    );  
};

export const DescLine = ({children, className, url, type, onClick, imgClassName, bottom}) => {
    const classes = ['item'];
    if (className) {
        classes.push(className);
    }

    return (
        <div className={classes.join(' ')} onClick={onClick}>
            <img src= {basicUrl + url}  className={imgClassName || 'img'}/>
            <div className='desc'>{children}</div>
            {bottom}
        </div>
    );  
};

export const Img = (props) => <img  {...props} src= {basicUrl + props.src}/>

export const Section = ({title, children, list, className, onChange }) => {
    const classes = ['section'];
    if (list) {
        classes.push('list');
    }
    if (className) {
        classes.push(className);
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

export const Price = ({price}) =>(
    <span className='price'>
        <span className='tag'>￥</span>{`${price}.00`}
    </span>
)