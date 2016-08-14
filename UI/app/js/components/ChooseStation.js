import React, {Component} from 'react';
import Page from './common/Page.js';
import {Section, Line, Label} from './common/Widgets';

export default class Station extends Component {
    render() {
       return (
        <Page>
            <Line size='auto' flex={true} style={{margin: '2em 0.5em',
                fontSize: '2em'}}>请选择提供服务的车站</Line>
                <Section >
                {this.props.stations.map(station => (
                    <a onClick={()=> this.props.updateChart({station}, this.props.nextPage)} 
                            key={station.StationId}>
                    <Line>{station.Name}</Line>
                    </a>
                ))}
                </Section>
        </Page>);
    }
}