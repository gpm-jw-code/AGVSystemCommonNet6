﻿using Newtonsoft.Json;

namespace AGVSystemCommonNet6.AGVDispatch.Messages
{
    public class clsSimpleReturnWithTimestampMessage : MessageBase
    {
        public new Dictionary<string, SimpleRequestResponseWithTimeStamp> Header { get; set; } = new Dictionary<string, SimpleRequestResponseWithTimeStamp>();
        internal SimpleRequestResponse ReturnData => this.Header[Header.Keys.First()];
    }


    public class clsSimpleReturnMessage : MessageBase
    {
        public new Dictionary<string, SimpleRequestResponse> Header { get; set; } = new Dictionary<string, SimpleRequestResponse>();
        public SimpleRequestResponse ReturnData => this.Header[Header.Keys.First()];
    }

    public class SimpleRequestResponseWithTimeStamp : SimpleRequestResponse
    {
        [JsonProperty("Time Stamp")]
        public string TimeStamp { get; set; }

    }

    public class SimpleRequestResponse
    {
        [JsonProperty("Return Code")]
        public int ReturnCode { get; set; }

    }
}
