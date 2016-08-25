import React, {Component} from 'react';
import Footer from './common/Footer.js';
import Page from './common/Page.js';
import {Section, Line, Label, Button} from './common/Widgets';
import {ListItem, SummaryLine} from './common/GoodsList';

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
                return value.length <= 7 && value.match(/(G|D|C)\d+$/i);
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

    renderList(total, list) {
        return (
            <Section list={true}>
                <Line><Label>已点菜品：</Label></Line>
                {Object.keys(list).map((key, index) => (
                    <Line key={index} className='short'>
                        <Label flex={true}>{list[key].Name}</Label>
                        <Label size='small' align='end'>{`x${list[key].Count}`}</Label>
                        <Label size='small' align='end'>{`￥${list[key].SellPrice || list[key].Price}`}</Label>
                    </Line>
                ))}
                <SummaryLine label='待支付' price={total} />
            </Section>
        );  
    }

    render() {
        const chart = this.props.chart;
        
        const footer = {
            button: {
                label: '提交订单',
                onClick: this.props.submmitOrder,
                disabled: this.props.submitting || !this.isInfoReady(chart.info)
            },
            left: {
                type: 'button',
                label: '返回修改',
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
                        {this.renderInput('CarriageNumber')}
                        </div>
                        <Label status='error'>{this.state.CarriageNumberError}</Label>
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
                        {this.renderInput('ContactTel')}
                        </div>
                        <Label status='error'>{this.state.ContactTelError}</Label>
                    </Line>
                </Section>                
                {this.renderList(chart.total, chart.goods)}
                <Section>
                    <Line>
                        <Label>订单备注：</Label>
                        {this.renderInput('Comment')}
                        <Label />
                    </Line>
                </Section>
                <p>
                   * 列车到站前， 工作人员会把餐品送到指定餐车前门门口。取餐后请核对餐品和清单。
                    感谢您对我们的支持！
                </p>
                <p>
                    如有任何问题、建议或投诉，请拨打电话xxx-xxxx-xxxx
                </p>
            </Page>
        );
    }
}