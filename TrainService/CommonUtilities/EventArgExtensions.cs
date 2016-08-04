using System;
using System.Threading;


namespace CommonUtilities
{
    public static class EventArgExtensions
    {
        public static void Raise<TEventArgs>(this TEventArgs e, object sender,
            ref EventHandler<TEventArgs> eventHandler)
        {
            var tempEventHandler = Interlocked.CompareExchange(ref eventHandler, null, null);
            if (tempEventHandler != null)
            {
                tempEventHandler(sender, e);
            }
        }
    }
}
