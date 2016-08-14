import React, {Component} from 'react';

export default class Button extends Component {
    render() {
        const {onClick, label, img, disabled, isPrimary} = this.props;
        let _onClick =  onClick;
        const classes = ['button'];
        
        if (disabled) {
            _onClick = undefined;
            classes.push('disabled');
        } else if (isPrimary == true) {
            classes.push('active');
        }

        return (
            <div className={classes.join(' ')} onClick={_onClick}>
                {img}
                <span>{label}</span>
            </div>
        );
    }
}