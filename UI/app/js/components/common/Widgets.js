import React from 'react';
import {basicUrl} from '../../constants/system';

export {Button, SmallButton} from './Button';
 
export const Label = ({size, children, flex, align, className, status, type, onClick, done}) => {
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

    if (onClick) {
        classes.push('link');
    }

    if (done) {
        classes.push('done');
    }
    
    return  <label className={classes.join(' ')} onClick={onClick}>{children}</label>;
};

export const LinkLabel = (props) => {
    return <Label {...props} onClick={props.onClick}/>
}

export const Line = ({children, className, direction, style, align = 'start'}) => {
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

export const Img = (props) => <img  {...props} src= {basicUrl + props.src}/>;

export const Section = ({title, children, list, className, onClick }) => {
    const classes = ['section'];
    if (list) {
        classes.push('list');
    }
    if (className) {
        classes.push(className);
    }

    return (
        <div className={classes.join(' ')} onClick={onClick}>
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
);
