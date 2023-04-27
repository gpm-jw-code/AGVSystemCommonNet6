using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.Alarm
{
    public enum ALARM_LEVEL
    {
        WARNING,
        ALARM,
    }
    public enum ALARM_SOURCE
    {
        AGVS, EQP
    }

    public enum ALARMS
    {
        VMS_DISCONNECT = 2,
        AGV_DISCONNECT = 3,
        GET_ONLINE_REQ_BUT_AGV_DISCONNECT = 4,
        GET_OFFLINE_REQ_BUT_AGV_DISCONNECT = 5,
        GET_ONLINE_REQ_BUT_AGV_STATE_ERROR = 6,
        TRAFFIC_BLOCKED_NO_PATH_FOR_NAVIGATOR = 7,
        NO_AVAILABLE_CHARGE_PILE = 8,
        TRAFFIC_CONTROL_CENTER_EXCEPTION_WHEN_CHECK_NAVIGATOR_PATH = 9,
        GET_CHARGE_TASK_BUT_AGV_CHARGING_ALREADY = 10
    }

}
