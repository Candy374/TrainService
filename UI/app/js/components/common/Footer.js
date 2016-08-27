import React, {Component, PropTypes} from 'react';
import {Button} from './Widgets';

const Footer = ({button, left}) =>  (
    <div className='footer'>
        {left && (
            left.onClick ? <Button {...left}/> : left)}
        <Button {...button} isPrimary={true}></Button>
    </div>
);

Footer.propTypes = {
    button: PropTypes.shape({
        label: PropTypes.string,
        onClick: PropTypes.func
    }).isRequired
};

export default Footer;

