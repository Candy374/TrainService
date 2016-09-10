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
        <p style={{textIndent: '1em'}}>
        公众号正在逐步完善中，不足之处敬请谅解。
        </p>
        <p style={{textIndent: '1em'}}>
        如果您有任何建议或者意见，欢迎直接在公众号中留言。
        </p>
    </Page>
);

export default Station;