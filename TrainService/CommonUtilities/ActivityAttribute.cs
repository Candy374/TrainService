/*********************************************************
*  This Attribute means all parameters and return value 
*  can be serialized. And can be transfered throw WCF
*
* OWNER: Zhao Shuhang-CXVR47
*
**********************************************************/
using System;

namespace CommonUtilities
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ActivityAttribute : Attribute
    {
        public ActivityAttribute()
        {
           
        }
    }
}
