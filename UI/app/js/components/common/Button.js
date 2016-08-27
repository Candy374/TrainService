import React, {Component} from 'react';

export const Button = ({onClick, label, img, disabled, isPrimary}) => {
    let _onClick =  onClick;
    const classes = ['button'];

    if (disabled) {
        _onClick = undefined;
        classes.push('disabled');
    } else if (isPrimary == true) {
        classes.push('active');
    }

    return (
        <button className={classes.join(' ')} onClick={_onClick}>
            {img}
            <span>{label}</span>
        </button>
    );
};

export const SmallButton = ({onClick, label, primary}) => (
    <button className={(primary ? 'primary' : '') + ' small'} onClick={onClick}>{label}</button>
);