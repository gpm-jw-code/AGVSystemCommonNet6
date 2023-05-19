/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;

namespace AGVSystemCommonNet6.GPMRosMessageNet.Actions
{
    public class TaskCommandActionFeedback : RosSharp.RosBridgeClient.ActionFeedback<TaskCommandFeedback>
    {
        public const string RosMessageName = "gpm_msgs/TaskCommandActionFeedback";

        public TaskCommandActionFeedback() : base()
        {
            this.feedback = new TaskCommandFeedback();
        }

        public TaskCommandActionFeedback(Header header, GoalStatus status, TaskCommandFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
    }
}
