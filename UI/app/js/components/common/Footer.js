import React, {Component} from 'react';
import Button from './Button';

export default class Footer extends Component {
    render() {
        const {button, total, disabled} = this.props;
        
        if (disabled) {
            button.onClick = undefined;
            button.className += ' disabled';
        }

        return (
            <div className='footer'>
                {total != null && <div className='total'>
                    共: ￥{total} 元
                </div>}
                <Button {...button}></Button>
            </div>
        )
    }
};

