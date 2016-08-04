using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public class ListType
    {
        private object _list;
        private List<object> items;

        public Type GetItemType
        {
            get
            {
                var interfaces = _list.GetType().GetInterfaces();
                if (interfaces.Length > 0)
                {
                    return interfaces[0].GetGenericArguments().FirstOrDefault();
                }

                throw new NotSupportedException();
            }
        }

        public int Count
        {
            get
            {
                return Convert.ToInt32(_list.GetType().GetProperty("Count").GetValue(_list, null));
            }
        }

        public ListType(object list)
        {
            this._list = list;
            this.items = new List<object>();
        }

        public object GetItem(int index)
        {
            object listItem = _list.GetType().GetProperty("Item").GetValue(_list, new object[] { index });

            return listItem;
        }
    }
}
