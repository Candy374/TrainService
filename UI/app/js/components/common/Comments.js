import React, {Component} from 'react';
import {Section, Line} from './Widgets';

const Comments = ({Comment}) =>(
    <div className='comments'>
      <p>
        * 列车到站前， 工作人员会把餐品送到指定餐车前门门口。取餐后请核对餐品和清单。
        感谢您对我们的支持！
      </p>
      <p>
        如有任何问题、建议或投诉，请拨打电话xxx-xxxx-xxxx
      </p>
    </div>
);

export default Comments;
