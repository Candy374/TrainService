import React, {Component} from 'react';
import Header from './common/Header.js';
import Footer from './common/Footer.js';

export default class TrainInfo extends Component {
    render() {
        return (
            <div className='trainInfo'>
                <div>
                车次：<input type='text' />
                餐车车厢号：<input type='number' />
                </div>
                <div>
                列车是否晚点：
                    <select>
                    <option>是</option>
                    <option>否</option>
                    </select>
                </div>
                <p>
                留言或特殊要求：
                </p>
                <textarea ></textarea>
                <p>
                    列车到站前， 工作人员会把餐品送到指定餐车前门门口。取餐后请核对餐品和清单。
                    感谢您对我们的支持！
                </p>
                <p>
                如有任何问题、建议或投诉，请拨打电话xxx-xxxx-xxxx
                </p>
            </div>
        );
    }
}