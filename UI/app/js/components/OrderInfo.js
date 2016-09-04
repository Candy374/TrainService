import React, {Component} from 'react';
import Footer from './common/Footer.js';
import Page from './common/Page.js';
import Comments from './common/Comments.js';
import {Section, Line, Label, Button} from './common/Widgets';
import {OrderListNoImg} from './common/GoodsList';
import {getTrainTime} from '../actions/order';

export default class OrderInfo extends Component {
    componentWillMount() {
        this.state = {
            TrainNumberError: '',
            CarriageNumberError: '',
            ContactTelError: '',
            validTrain: false
        }
    }

    renderInput(name, type) {
        return (
            <input  value={this.props.chart.info[name]}
                    ref={node=> this[name] = node}
                    onChange={() => {
                        const info = this.props.chart.info;
                        info[name] =  this[name].value;
                        this.props.updateChart({info});
                    }}
                    onBlur={() => {
                        let state = this.state;
                        if (!this.isValid(this[name].value, name)) {
                            if (state[name + 'Error'] == '') {
                                let label = this[name].parentElement.children[0].textContent;
                                state[name + 'Error'] = '请输入正确的' + label.replace('：', '').trim();
                            }                         
                        } else {
                            if (state[name + 'Error'] != '') {
                                state[name + 'Error'] = '';
                            }  
                        }
                        this.setState(state);
                    }}
                    type={type || 'text'} />
        );
    }

    validTrain(number = this.props.chart.info.TrainNumber) {
      getTrainTime(this.props.chart.station.StationCode, number).then(msg => {
        this.setState({
          TrainNumberError: msg != 'OK' ? msg : '',
          validTrain: msg == 'OK'
        });
      });
    }

    isValid(value, type, checkTrainTime = true) {
        switch(type) {
            case 'TrainNumber': {
                if (checkTrainTime) {
                    if (!this.props.chart.info.IsDelay) {
                        this.validTrain(value);
                    } else if (!this.state.validTrain) {
                        this.setState({validTrain: true})
                    }
                }
              //value 长度小于5， 字母开头或者全数字
              return value.length <= 7 && value.match(/(G|D|C)\d+$/i);
            }
            case 'CarriageNumber':
                // less than 16
                return value/1 < 16 && value/1 > 0;
            case 'ContactTel':
                // start with 1, length == 11
                return value.length == 11 && !isNaN(value/1) && value.indexOf('1') == 0;
        }
    }

    isInfoReady(info) {
        return this.isValid(info.TrainNumber, 'TrainNumber', false) && 
                this.isValid(info.CarriageNumber, 'CarriageNumber') && 
                this.isValid(info.ContactTel, 'ContactTel') && info.Contact && this.state.validTrain &&
            !this.state.TrainNumberError && !this.state.CarriageNumberError && !this.state.ContactTelError;
    }

    render() {
        const chart = this.props.chart;
        
        const footer = {
            button: {
                label: '提交订单',
                onClick: this.props.submitOrder,
                disabled: this.props.submitting || !this.isInfoReady(chart.info)
            },
            left: {
                type: 'button',
                label: '修改菜品',
                disabled: this.props.submitting,
                onClick: this.props.prePage
            }
        };
        
        return (
            <Page className='order-info' footer={footer}>
                <Section >
                    <Line direction='col'>
                     <div style={{width : '100%'}}>
                        <Label className='must'>送达车次：</Label>
                        {this.renderInput('TrainNumber')}
                        </div>
                        <Label status='error'>{this.state.TrainNumberError}</Label>
                    </Line>
                    <Line direction='col'>
                        <div style={{width : '100%'}}>
                        <Label className='must'>餐车车厢号：</Label>
                        {this.renderInput('CarriageNumber', 'number')}
                        </div>
                        <Label status='error'>{this.state.CarriageNumberError}</Label>
                    </Line>
                    <Line>
                        <Label flex={true}>如果您所在列车已晚点运行请勾选右侧</Label>
                        <input  value={chart.info.IsDelay}
                                checked={chart.info.IsDelay}
                                ref={node=> this.IsDelay = node}
                                onChange={() => {
                                    chart.info.IsDelay = this.IsDelay.checked;
                                    if (chart.info.IsDelay) {
                                       if (this.state.TrainNumberError.indexOf('小于45分钟') > -1) {
                                          this.setState({
                                            TrainNumberError: '',
                                            validTrain: true
                                          });
                                       }
                                    } else {
                                      this.validTrain();
                                    }

                                    this.props.updateChart({info: chart.info});
                                }}
                                type='checkbox' />
                    </Line>
                </Section>
                <Section>
                    <Line direction='col'>
                    <div style={{width : '100%'}}>
                        <Label className='must'>联系人：</Label>
                        {this.renderInput('Contact')}
                        </div>
                        <Label status='error'/>
                    </Line>
                    <Line direction='col'>
                    <div style={{width : '100%'}}>
                        <Label className='must'>手机号：</Label>
                        {this.renderInput('ContactTel', 'number')}
                        </div>
                        <Label status='error'>{this.state.ContactTelError}</Label>
                    </Line>
                </Section>                
                <OrderListNoImg total={chart.total} list={chart.goods}/>
                <Section>
                    <Line>
                        <Label>订单备注：</Label>
                        {this.renderInput('Comment')}
                        <Label />
                    </Line>
                </Section>
                <Comments />
            </Page>
        );
    }
}