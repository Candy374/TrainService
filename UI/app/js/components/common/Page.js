import React, {Component} from 'react';
import Footer from './Footer.js';

export default class Page extends Component {
    render() {
        const {children, footer, className} = this.props;
        const classes = className ? `page ${className}` : 'page';
        return (
            <div className='container'>
                <div className={classes}>
                {children}
                </div>
                {footer && <Footer {...footer} />}
            </div>
        );
    }
}