import React, {Component} from 'react';
import Button from './Button';

export default class Footer extends Component {
    render() {
        const {button, left} = this.props;

        return (
            <div className='footer'>
                {left}
                <Button {...button}></Button>
            </div>
        )
    }
};
