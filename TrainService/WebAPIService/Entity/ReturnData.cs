using System;
using System.Runtime.Serialization;

namespace WebAPIService.Entity
{
    [Serializable]
    [DataContract]
    public class ReturnData
    {
        [DataMember]
        public object Data;

        [DataMember]
        public int ErrCode;

        [DataMember]
        public string ErrMsg;

        /// <summary>
        ///  0: Show to user, 1: show to console, 2: do not show
        /// </summary>
        [DataMember]
        public int ShowLevel;
    }
}