import React, {Component} from 'react';
import Footer from './common/Footer.js';
import Page from './common/Page.js';
import {Section, Line, Label, Button} from './common/Widgets';

export default class OrderInfo extends Component {
    componentWillMount() {
        this.state = {
            TrainNumberError: '',
            CarriageNumberError: '',
            ContactTelError: ''
        }
    }

    renderInput(name) {
        return (
            <input  value={this.props.chart.info[name]} 
                    style={{width: '7em'}}
                    ref={node=> this[name] = node}
                    onChange={() => {
                        const value = this[name].value;
                        const info = this.props.chart.info;
                        info[name] = this[name].value;
                        this.props.updateChart({info});

                        let state = this.state;
                        if (!this.isValid(value, name)) {
                            if (state[name + 'Error'] == '') {
                                let label = this[name].parentElement.children[0].textContent;
                                state[name + 'Error'] = '请输入正确的' + label.replace('：', '').trim();
                                this.setState(state);   
                            }                         
                        } else {
                            if (state[name + 'Error'] != '') {
                                state[name + 'Error'] = '';
                                this.setState(state);
                            }  
                        }
                    }}
                    type='text' />
        );
    }

    isValid(value, type) {
        switch(type) {
            case 'TrainNumber':
                //value 长度小于5， 字母开头或者全数字
                return value.length < 5 && value.match(/(G|D|C)\d+$/i);
            case 'CarriageNumber':
                // less than 16
                return value/1 < 16 && value/1 > 0;
            case 'ContactTel':
                // start with 1, length == 11
                return value.length == 11 && !isNaN(value/1) && value.indexOf('1') == 0;
        }
    }

    isInfoReady(info) {
        return info.TrainNumber && info.CarriageNumber && info.Contact && info.ContactTel &&
            !this.state.TrainNumberError && !this.state.CarriageNumberError && !this.state.ContactTelError;
    }

    render() {
        const info = this.props.chart.info;
        const footer = {
            button: {
                label: '确认下单',
                onClick: this.props.nextPage,
                disabled: !this.isInfoReady(info)
            },
            left: {
                label: '返回修改',
                onClick: this.props.prePage
            }
        };
        
        
        return (
            <Page className='order-info' footer={footer}>
                <Section title='列车信息'>
                    <Line>
                        <Label>车次：</Label>
                        {this.renderInput('TrainNumber')}*
                        <Label size='large' status='error'>{this.state.TrainNumberError}</Label>
                    </Line>
                    <Line>
                        <Label>餐车车厢号：</Label>
                        {this.renderInput('CarriageNumber')}*
                        <Label size='large' status='error'>{this.state.CarriageNumberError}</Label>
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
                        <Label>姓名：</Label>
                        {this.renderInput('Contact')}*
                        <Label size='large' status='error'/>
                    </Line>
                    <Line>
                        <Label>手机号：</Label>
                        {this.renderInput('ContactTel')}*
                        <Label size='large' status='error'>{this.state.ContactTelError}</Label>
                    </Line>
                </Section>
                <Section title='留言或特殊要求'>
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