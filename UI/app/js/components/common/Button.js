import React, {Component} from 'react';

export default class Button extends Component {
    render() {
        const {onClick, label, img, className} = this.props;
        const classes = ['button'];
        if (className) {
            classes.push(className);
        }

        return (
            <div className={classes.join(' ')} onClick={onClick}>
                {img}
                <span>{label}</span>
            </div>
        );
    }
}