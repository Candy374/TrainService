import React, {Component} from 'react';

export default class NumberInput extends Component {
    add() {
        this.props.updateCount(Math.max(this.props.count + 1, 0));
    }

    minues() {
        this.props.updateCount(Math.max(this.props.count - 1, 0));
    }


    render() {
        return (
            <div className='number-input'>
                <button onClick={this.minues.bind(this)} style={{display: this.props.count > 0 ? 'inline-block' : 'none'}}>-</button>
                <label >{this.props.count}</label>              
                <button onClick={this.add.bind(this)}>+</button>
            </div>
        )
    }
};