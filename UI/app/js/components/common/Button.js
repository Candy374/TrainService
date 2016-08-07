import React, {Component} from 'react';

export default class Button extends Component {
    render() {
        const {onClick, label, img} = this.props;
        return (
            <div className='button' onClick={onClick}>
                {img}
                <span>{label}</span>
            </div>
        );
    }
}