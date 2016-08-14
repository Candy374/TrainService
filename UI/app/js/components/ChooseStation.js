import React, {Component} from 'react';
import Button from './common/Button';
import Page from './common/Page';
import {Section, ImgLine, Img, Line, Label} from './common/Widgets';

export default class Station extends Component {
    render() {
       return (
        <Page>
            <Line size='auto' flex={true} style={{margin: '2em 0.5em',
                fontSize: '2em'}}>请选择车站</Line>
                <Section list={true}>
                {this.props.stations.map(station => (
                    <Line key={station.StationId}>
                        <Button label={station.Name} isPrimary={true}
                            onClick={()=> this.props.updateChart({station}, this.props.nextPage)}>
                        
                        </Button>
                    </Line>
                ))}
                </Section>
        </Page>);
    }
}