

using RosSharp.RosBridgeClient;


namespace AGVSystemCommonNet6.GPMRosMessageNet.Services
{
    public class PinCommandResponse : Message
    {
        public const string RosMessageName = "gpm_msgs/PinCommand";

        public bool confirm { get; set; }

        public PinCommandResponse()
        {
            this.confirm = false;
        }

        public PinCommandResponse(bool confirm)
        {
            this.confirm = confirm;
        }
    }
}
