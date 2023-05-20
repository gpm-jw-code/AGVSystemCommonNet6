﻿using AGVSystemCommonNet6.GPMRosMessageNet.Actions;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;

namespace AGVSystemCommonNet6.GPMRosMessageNet.Actions
{
    public class TaskCommandActionClient : ActionClient<TaskCommandAction, TaskCommandActionGoal, TaskCommandActionResult, TaskCommandActionFeedback, TaskCommandGoal, TaskCommandResult, TaskCommandFeedback>, IDisposable
    {
        public Action<ActionStatus> OnTaskCommandActionDone;
        public Action<ActionStatus> OnActionStatusChanged;
        public TaskCommandGoal goal;
        private bool disposedValue;

        public TaskCommandActionClient(string actionName, RosSocket rosSocket)
        {
            this.actionName = actionName;
            this.rosSocket = rosSocket;
            action = new TaskCommandAction();
            goalStatus = new RosSharp.RosBridgeClient.MessageTypes.Actionlib.GoalStatus();
        }

        protected override TaskCommandActionGoal GetActionGoal()
        {
            if (action == null)
                return new TaskCommandActionGoal();
            action.action_goal.goal = goal;
            return action.action_goal;
        }

        protected override void OnFeedbackReceived()
        {
            if (action == null)
                return;
            var _feedback = action.action_feedback;
        }

        protected override void FeedbackCallback(TaskCommandActionFeedback actionFeedback)
        {
            try
            {
                base.FeedbackCallback(actionFeedback);
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnResultReceived()
        {
            if (action == null)
                return;
            TaskCommandActionResult result = action.action_result;
            ActionStatus status = (ActionStatus)(result.status.status);
            if (OnTaskCommandActionDone != null)
            {
                OnTaskCommandActionDone(status);
            }
        }
        private ActionStatus previousActionStatus = ActionStatus.NO_GOAL;
        protected override void OnStatusUpdated()
        {
            if (goalStatus != null)
            {
                var _actionStatus = (ActionStatus)(goalStatus.status);
                if (previousActionStatus != _actionStatus)
                {
                    if (OnActionStatusChanged != null)
                    {
                        OnActionStatusChanged(_actionStatus);
                    }
                    Console.WriteLine("[TaskCommandActionClient] OnStatusUpdated : Status : " + _actionStatus);
                }
                previousActionStatus = _actionStatus;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                OnTaskCommandActionDone = null;
                goalStatus = null;
                action = null;
                disposedValue = true;
            }
        }

        // ~TaskCommandActionClient()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
