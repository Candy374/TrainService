import React, {Component} from 'react';
import Page from './common/Page.js';
import {Line, Label} from './common/Widgets';

export default class Station extends Component {
    render() {
       return (
        <Page>
            <Line size='auto' flex={true}>请选择提供服务的车站</Line>
            {this.props.stations.map(station => (
                <a onClick={()=> this.props.updateChart({station}, this.props.nextPage)} 
                        key={station.StationId}>
                {station.Name}</a>
            ))}
        </Page>);
    }
}