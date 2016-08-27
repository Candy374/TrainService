import React, {Component} from 'react';
import Page from './common/Page';
import {Section, ImgLine, Img, Line, Label, Button} from './common/Widgets';

const Station = ({stations, updateChart, nextPage}) => (
    <Page>
        <Line size='auto' flex={true} 
              style={{margin: '2em 0.5em', fontSize: '2em'}}>请选择车站</Line>
        <Section list={true}>
        {stations.map(station => (
            <Line key={station.StationId}>
                <Button label={station.Name} isPrimary={true}
                    onClick={()=> updateChart({station}, nextPage)}>
                </Button>
            </Line>
        ))}
        </Section>
    </Page>
);

export default Station;