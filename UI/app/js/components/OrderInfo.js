import React, {Component} from 'react';
import Header from './common/Header.js';
import Footer from './common/Footer.js';
import Page from './common/Page.js';
import Button from './common/Button';
import {Section, Line, Label} from './common/Widgets';

export default class OrderInfo extends Component {
    componentWillMount() {
        this.state = {
            trainNo: '',
            carriageNo: '',
            isLate: false,
            userName: '',
            userNumber: '',
            comments: ''
        }
    }

    renderInput(name) {
        return (
            <input value={this.props.chart.info[name]} 
                                ref={node=> this[name] = node}
                                onChange={() => {
                                    const state = this.props.chart.info;
                                    state[name] = this[name].value;
                                    this.props.updateChart({info: state});
                                }}
                                type='text' />
        );
    }

    render() {
        const info = this.props.chart.info;
        const footer = {
            button: {
                label: '确认下单',
                onClick: this.props.nextPage,
                disabled: !info.TrainNumber || !info.CarriageNumber || !info.Contact || !info.ContactTel
            },
            left: <Button label='返回修改' onClick={this.props.prePage}/>
        };
        
        
        return (
            <Page className='order-info' footer={footer}>
                <Section title='列车信息'>
                    <Line>
                        <Label>车次：</Label>
                        {this.renderInput('TrainNumber')}*
                    </Line>
                    <Line>
                        <Label>餐车车厢号：</Label>
                        {this.renderInput('CarriageNumber', 'number')}*
                    </Line>
                    <Line>
                        <Label>列车已晚点</Label>
                        <input  value={this.props.chart.info.IsDelay} 
                                checked={this.props.chart.info.IsDelay}
                                ref={node=> this.IsDelay = node}
                                onChange={() => {
                                    info.IsDelay = this.IsDelay.checked;
                                    this.props.updateChart({info});
                                }}
                                type='checkbox' />
                    </Line>
                </Section>
                <Section title='联系人信息'>
                    <Line>
                        <Label size='small'>姓名：</Label>
                        {this.renderInput('Contact')}*
                    </Line>
                    <Line>
                        <Label size='small'>电话：</Label>
                        {this.renderInput('ContactTel')}*
                    </Line>
                </Section>
                <Section title='留言或特殊要求：'>
                    <textarea value={info.Comment} 
                            ref={node=> this.Comment = node}
                            onChange={() => {
                                    info.Comment = this.Comment.value;
                                    this.props.updateChart({info});
                            }}/>
                    <p>
                        列车到站前， 工作人员会把餐品送到指定餐车前门门口。取餐后请核对餐品和清单。
                        感谢您对我们的支持！
                    </p>
                    <p>
                        如有任何问题、建议或投诉，请拨打电话xxx-xxxx-xxxx
                    </p>
                </Section>
            </Page>
        );
    }
}