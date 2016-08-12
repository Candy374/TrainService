import React, {Component} from 'react';
import Footer from './Footer.js';

export default class Page extends Component {
    render() {
        const {children, footer, className} = this.props;
        const classes = className ? `container ${className}` : 'container';
        return (
            <div className={classes}>
                <div className='page'>
                {children}
                </div>
                {footer && <Footer {...footer} />}
            </div>
        );
    }
}