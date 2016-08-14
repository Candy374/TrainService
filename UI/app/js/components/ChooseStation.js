import React, {Component} from 'react';
import Page from './common/Page.js';
import {Section, ImgLine, Img, Line, Label} from './common/Widgets';

export default class Station extends Component {
    render() {
       return (
        <Page>
            <Line size='auto' flex={true} style={{margin: '2em 0.5em',
                fontSize: '2em'}}>请选择车站</Line>
                <Section list={true}>
                {this.props.stations.map(station => (
                    // <li key={station.StationId} style={{listStyle: 'none'}}
                    //     onClick={()=> this.props.updateChart({station}, this.props.nextPage)} >
                    //     <Img src={station.PicUrl} style={{width: '4em'}}/>
                    //     <Label>{station.Name}</Label>
                    // </li>

                    <ImgLine url={station.PicUrl} key={station.StationId}
                        imgClassName='width-small height-small'
                        onClick={()=> this.props.updateChart({station}, this.props.nextPage)}>
                        <Label>{station.Name}</Label>
                    </ImgLine>
                ))}
                </Section>
        </Page>);
    }
}