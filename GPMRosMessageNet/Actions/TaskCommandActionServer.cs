﻿using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.GPMRosMessageNet.Actions
{
    public class TaskCommandActionServer : ActionServer<TaskCommandAction, TaskCommandActionGoal, TaskCommandActionResult, TaskCommandActionFeedback, TaskCommandGoal, TaskCommandResult, TaskCommandFeedback>
    {
        public event EventHandler<TaskCommandGoal> OnNAVGoalReceived;
        public TaskCommandActionServer(string actionName, RosSocket rosSocket)
        {
            this.actionName = actionName;
            this.rosSocket = rosSocket;
            action = new TaskCommandAction();
        }
        protected override void OnGoalActive()
        {

        }

        protected override void OnGoalPreempting()
        {
        }

        protected override void OnGoalRecalling(GoalID goalID)
        {
        }
        protected override void OnGoalSucceeded()
        {
            base.OnGoalSucceeded();
        }
        protected override void OnGoalReceived()
        {
            Thread.Sleep(400);
            TaskCommandGoal? goal = this.action.action_goal.goal;
            if (OnGoalReceived != null)
            {
                if (goal.planPath.poses.Length == 0)
                {
                    Console.WriteLine("goal.planPath.poses.Length == 0");
                    SetAborted();
                    SetSucceeded();
                }
                else
                {

                    Console.WriteLine("OnNAVGoalReceived?.Invoke(this, goal);");
                    OnNAVGoalReceived?.Invoke(this, goal);
                }
            }
            else
            {
                Console.WriteLine("OnGoalReceived Null");
            }
        }

        public void SucceedInvoke()
        {
            SetSucceeded();

        }

        public void AcceptedInvoke()
        {
            SetAccepted();
        }

        public void RejectInvoke()
        {
            SetRejected();
        }
        public void CancelInvoke()
        {

            SetCanceled();
        }
    }
}
