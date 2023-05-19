

using RosSharp.RosBridgeClient;


namespace AGVSystemCommonNet6.GPMRosMessageNet.Messages
{
    public class CurrentMotorValue : Message
    {
        public const string RosMessageName = "gpm_msgs/CurrentMotorValue";

        public double[] value { get; set; }

        public CurrentMotorValue()
        {
            this.value = new double[0];
        }

        public CurrentMotorValue(double[] value)
        {
            this.value = value;
        }
    }
}
