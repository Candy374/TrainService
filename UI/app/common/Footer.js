import React, {Component} from 'react';

export default class Footer extends Component {
    render() {
        const {button, children} = this.props;

        return (
            <div>
                {children}
                {button}
            </div>
        )
    }
};

