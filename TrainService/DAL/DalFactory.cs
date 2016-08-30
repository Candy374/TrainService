using DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Foundation;

namespace DAL
{
    public static class DalFactory
    {
        public static readonly ProviderDao Provider = new ProviderDao();
        public static readonly GoodsDao Goods = new GoodsDao();
        public static readonly TagsDao Tags = new TagsDao();
        public static readonly AccountDao Account = new AccountDao();
        public static readonly OrderDao Orders = new OrderDao();
        public static readonly StationDao Stations = new StationDao();
        public static readonly PaymentDao Payment = new PaymentDao();


        private static Task _task = null;
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        static DalFactory()
        {
            if (_task == null || _task.Status != TaskStatus.Running)
            {
                _task = new Task(RefreshCache, _cts.Token);
                _task.Start();
            }
        }

        private static void RefreshCache()
        {
            var actions = new Action[] {
                () => Provider.RefreshData(false),
                () => Goods.RefreshData(false),
                () => Tags.RefreshData(false),
                () => Stations.RefreshData(true),
                () => Account.RefreshData(false)
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

        private static void SafeExecute(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
            }
        }
    }
}
