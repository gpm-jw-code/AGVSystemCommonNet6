using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.AGVDispatch.Messages
{
    public class clsCarrierRemovedMessage : MessageBase
    {
        public Dictionary<string, CarrierRemovedData> Header { get; set; } = new Dictionary<string, CarrierRemovedData>();
    }

    public class CarrierRemovedData
    {
        [JsonProperty("Time Stamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("Task Name")]
        public string TaskName { get; set; } = "";

        [JsonProperty("CSTID")]
        public string[] CSTID { get; set; } = new string[0];

        [JsonProperty("OPID")]
        public string OPID { get; set; } = "";

    }
}
