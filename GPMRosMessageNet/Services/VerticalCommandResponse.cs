

using RosSharp.RosBridgeClient;



namespace AGVSystemCommonNet6.GPMRosMessageNet.Services
{
    public class VerticalCommandResponse : Message
    {
        public const string RosMessageName = "gpm_msgs/VerticalCommand";

        public bool confirm { get; set; }

        public VerticalCommandResponse()
        {
            this.confirm = false;
        }

        public VerticalCommandResponse(bool confirm)
        {
            this.confirm = confirm;
        }
    }
}
