import React, {Component} from 'react';
import Footer from './Footer.js';

const Page = ({children, footer, className, flex, direction}) => {
    const classes = ['page', `direction-${direction || 'row'}`];
    if (flex) {
        classes.push('flex');
    }

    if (className) {
        classes.push(className);
    }

    return (
        <div className='container'>
            <div className={classes.join(' ')}>
                {children}
            </div>
            {footer && <Footer {...footer} />}
        </div>
    );
};

export default Page;
