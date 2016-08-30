using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebAPIService
{
    public static class Jobs
    {
        private static Task _task = null;
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        public static void Start()
        {
            if (_task == null || _task.Status != TaskStatus.Running)
            {
                _task = new Task(RunJobs, _cts.Token);
                _task.Start();
            }
        }
        static Jobs()
        {
            //if (_task == null || _task.Status != TaskStatus.Running)
            //{
            //    _task = new Task(RunJobs, _cts.Token);
            //    _task.Start();
            //}
        }

        private static void RunJobs()
        {
            var actions = new Action[] {
                () => RefundJob(),
                () => QueryRefundJob()
            };

            while (true)
            {
                foreach (var act in actions)
                {
                    if (_cts.IsCancellationRequested)
                    {
                        break;
                    }
                    SafeExecute(act);
                }

                Thread.Sleep(10000);
            }
        }

        private static void QueryRefundJob()
        {
            var checkList = DAL.DalFactory.Payment.GetRefundCheckList();
            foreach (var item in checkList)
            {
                var result = WxPayApi.RefundHelper.RefundQuery(item.RefundTradeNumber, null, null, null);
                DAL.DalFactory.Payment.SetRefundFee(item.Id, result, item.RefundTradeNumber);
            }
        }

        private static void RefundJob()
        {
            var checkList = DAL.DalFactory.Payment.GetNeedRefundPaymentList();

            foreach (var item in checkList)
            {
                var tradeNo = WxPayApi.RefundHelper.Refund(item.TradeNumber, item.OrderId.ToString(), item.Amount, item.NeedRefund);
                DAL.DalFactory.Payment.SetRefundTradeNumber(item.Id, tradeNo);
            }
        }

        private static void SafeExecute(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                LoggerContract.Logger.Warn(ex, "Unknown Error", "Jobs");
            }
        }
    }
}