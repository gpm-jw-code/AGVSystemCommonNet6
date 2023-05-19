using Newtonsoft.Json;

namespace AGVSystemCommonNet6.AGVDispatch.Messages
{

    public class clsOnlineModeQueryMessage : MessageBase
    {
        public Dictionary<string, OnlineModeQuery> Header { get; set; } = new Dictionary<string, OnlineModeQuery>();
    }
    public class OnlineModeQuery
    {
        [JsonProperty("Time Stamp")]
        public string TimeStamp { get; set; } = DateTime.Now.ToAGVSTimeFormat();
    }

    public class clsOnlineModeQueryResponseMessage : MessageBase
    {
        public Dictionary<string, OnlineModeQueryResponse> Header { get; set; } = new Dictionary<string, OnlineModeQueryResponse>();
        internal OnlineModeQueryResponse OnlineModeQueryResponse => this.Header[Header.Keys.First()];
    }
    public class OnlineModeQueryResponse
    {
        [JsonProperty("Time Stamp")]
        public string TimeStamp { get; set; }
        [JsonProperty("Remote Mode")]
        public REMOTE_MODE RemoteMode { get; set; }
    }
}
