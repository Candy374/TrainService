import React, {Component} from 'react';
import Button from './Button.js';

export default class Header extends Component {
    render() {
        const {children, back, menu} = this.props;

        const buttonLeft = {
            label: 'back',
            onClick: () => console.log('back'),
            img: '< '
        };

        const buttonRight = {
            label: '...',
            onClick: () => console.log('more')
        }

        return (
            <div className='header'>
                <Button {...buttonLeft}></Button>
                <div className='title'>
                    <label >{children}</label>
                </div>                
                <Button {...buttonRight}>...</Button>
            </div>
        )
    }
};