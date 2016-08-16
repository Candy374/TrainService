import React, {Component} from 'react';
import {Button} from './Widgets';

const Footer = ({button, left}) => {
    if (left && left.type == 'button') {
        left = (
            <Button label={left.label}
                disabled={left.disabled}
                onClick={left.onClick}>
            </Button>);
    }

    return (
        <div className='footer'>
            {left}
            <Button {...button} isPrimary={true}></Button>
        </div>
    )
};

export default Footer;
