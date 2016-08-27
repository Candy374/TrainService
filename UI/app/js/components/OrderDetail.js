import React, {Component} from 'react';
import {RateItem, ListItem, SummaryLine, OrderListNoImg} from './common/GoodsList';
import * as actions from '../actions/order';
import Page from './common/Page';
import Detail from './common/Detail';
import Comments from './common/Comments';
import OrderStatus from './common/OrderStatus';
import {Section, Line, Label} from './common/Widgets';

export default class OrderDetail extends Component {
    componentWillMount() {
        this.state = {
            order: null,
            rate: false
        };
        this.updateOrder = this.updateOrder.bind(this);
        this.updateOrder();
    }

    updateOrder() {
        actions.getOrderDetail(this.props.id).then(order => {
            this.setState({
              order,
              rate: false
            });
        });
    }

    score() {
      const order = this.state.order;
      const data = {
        OrderId: order.OrderId,
        Rates: []
      };
      order.SubOrders.map(item => {
        data.Rates.push({
          GoodsId: item.GoodsId,
          Rate: item.Rate,
          SubId: item.Id
        });
      });
      actions.submitRates(data).then(this.updateOrder);
    }

    cancelOrder() {
        const order = this.state.order;
        if (order.StatusCode != 0) {
            alert('请联系xxxxxx取消订单');
        } else {
            actions.cancelOrder(order.OrderId).then(this.updateOrder);
        }  
    }

    orderAgain() {
        const chart = {goods: {}};
        this.state.order.SubOrders.map(item => {
            chart.goods[item.GoodsId] = item
        });
        chart.total = this.state.order.Amount;
        this.props.updateChart(chart, () => this.props.nextPage('Info'));
    }

    getButton() {
      const StatusCode = this.state.order.StatusCode;
      let button;
      if (StatusCode == 0) {
        button = {
          label: '立即支付',
          onClick: this.props.submitOrder,
          disabled: this.state.submitting
        }
      } else if (StatusCode < 4) {
        button = {
          label: '取消订单',
          onClick: this.cancelOrder.bind(this)
        }
      } else {
        if (StatusCode == 6) {
          button = {
            label: this.state.order.IsRated ? '查看评价' : '去评价',
            onClick: () => this.setState({rate: true})
          }
        } else {
          button = {
            label: '重新下单',
            onClick: this.orderAgain.bind(this)
          }
        }
      }
      return button;
    }
    render() {
        const order = this.state.order;
        if (!order) {
            return null;
        }

        if (this.state.rate) {
          const footer = {
            button: {
              label: '提交评价',
              onClick: this.score.bind(this),
              disabled: this.state.order.IsRated
            },
            left: {
              label: '返回',
              onClick: () => this.setState({rate: false})
            }
          };

          return (
            <Page footer={footer} className='order-rate'>
              <Section list={true} title='为商家服务打分'>
                <RateItem rate={order.Rate}
                          updateRate={(Rate) => {
                              if (order.IsRated) {
                                return;
                              }
                              order.Rate = Rate;
                              this.setState({order});
                            }}
                          name={order.Name}/>
              </Section>
              <Section list={true} title='为菜品打分'>
                {order.SubOrders.map((item, index) => (
                  <RateItem key={index}
                            rate={item.Rate}
                            updateRate={(Rate) => {
                              if (order.IsRated) {
                                return;
                              }
                              item.Rate = Rate;
                              this.setState({order});
                            }}
                            name={item.Name}/>))
                }
              </Section>
            </Page>
          );
        } else {
          const footer = {
            button: this.getButton(),
            left: {
              label: '订单列表',
              onClick: () => this.props.nextPage('MyOrders')
            }
          };

          return (
            <Page footer={footer} className='order-detail'>
              <OrderStatus status={order.StatusCode}/>
              <OrderListNoImg total={order.Amount} list={order.SubOrders} totalLabel='共计'/>
              <Detail {...order}/>
              <Comments />
            </Page>
          );
        }
    }
}