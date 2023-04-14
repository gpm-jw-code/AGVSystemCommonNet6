using AGVSytemCommonNet6.AGVMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AGVSytemCommonNet6.clsEnums;

namespace AGVSytemCommonNet6
{
    public static class Extensions
    {
        public static MAIN_STATUS GetAGVStatus(this cls_0105_RunningStatusReportHeader data)
        {
            return Enum.GetValues(typeof(MAIN_STATUS)).Cast<MAIN_STATUS>().First(enu => (int)enu == data.AGV_Status);
        }

        public static ONLINE_STATE GetOnlineReqMode(this cls_0103_OnlineRequestHeader data)
        {
            return Enum.GetValues(typeof(ONLINE_STATE)).Cast<ONLINE_STATE>().First(enu => (int)enu == data.Mode_Request);

        }

        /// <summary>
        /// 取得站點路線By Action Type
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static clsMapPoint[] GetMainTrajectory(this cls_0301_TaskDownloadHeader task)
        {
            if (task.GetActionTypeEnum() == ACTION_TYPE.Move)
                return task.Trajectory;
            else
                return task.Homing_Trajectory;
        }

        /// <summary>
        /// 取得站點路線By Action Type
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static void ReplanTrajectort(this cls_0301_TaskDownloadHeader task, clsMapPoint[] newTrajectory)
        {
            if (task.GetActionTypeEnum() == ACTION_TYPE.Move)
                task.Trajectory = newTrajectory;
            else
                task.Homing_Trajectory = newTrajectory;
        }

    }
}
