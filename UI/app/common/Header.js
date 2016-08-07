import React, {Component} from 'react';
import Button from 'react-native-button';

export default class Header extends Component {
    render() {
        const {title, back, menu} = this.props;

        return (
            <div>
                <Button>Back</Button>
                <label>{title}</label>
                <button>...</button>
            </div>
        )
    }
};