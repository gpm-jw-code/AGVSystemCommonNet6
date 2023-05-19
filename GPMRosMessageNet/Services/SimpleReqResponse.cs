

using RosSharp.RosBridgeClient;



namespace AGVSystemCommonNet6.GPMRosMessageNet.Services
{
    public class SimpleReqResponse : Message
    {
        public const string RosMessageName = "gpm_msgs/SimpleReq";

        public bool response { get; set; }

        public SimpleReqResponse()
        {
            this.response = false;
        }

        public SimpleReqResponse(bool response)
        {
            this.response = response;
        }
    }
}
