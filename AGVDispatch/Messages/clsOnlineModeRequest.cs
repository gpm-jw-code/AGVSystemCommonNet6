using Newtonsoft.Json;

namespace AGVSystemCommonNet6.AGVDispatch.Messages
{
    public class clsOnlineModeRequestMessage : MessageBase
    {
        public Dictionary<string, OnlineModeRequest> Header { get; set; } = new Dictionary<string, OnlineModeRequest>();

    }


    public class OnlineModeRequest
    {
        [JsonProperty("Time Stamp")]
        public string TimeStamp { get; set; }
        [JsonProperty("Mode Request")]
        public REMOTE_MODE ModeRequest { get; set; }

        [JsonProperty("Current Node")]
        public int CurrentNode { get; set; }
    }

    public class clsOnlineModeRequestResponseMessage : MessageBase
    {
        public Dictionary<string, SimpleRequestResponseWithTimeStamp> Header { get; set; } = new Dictionary<string, SimpleRequestResponseWithTimeStamp>();
        public RETURN_CODE ReturnCode => (RETURN_CODE)this.Header[Header.Keys.First()].ReturnCode;
    }


    
}
