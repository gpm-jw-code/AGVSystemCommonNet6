

using RosSharp.RosBridgeClient;


using AGVSystemCommonNet6.GPMRosMessageNet.Messages;

namespace AGVSystemCommonNet6.GPMRosMessageNet.Messages
{
    public class DriversState : Message
    {
        public const string RosMessageName = "gpm_msgs/DriversState";

        public DriverState[] driversState { get; set; }

        public DriversState()
        {
            this.driversState = new DriverState[2];
            this.driversState[0] = new DriverState();
            this.driversState[1] = new DriverState();
        }

        public DriversState(DriverState[] driversState)
        {
            this.driversState = driversState;
        }
    }
}
