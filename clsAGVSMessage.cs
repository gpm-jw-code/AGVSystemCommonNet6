using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using static AGVSytemCommonNet6.clsEnums;

namespace AGVSytemCommonNet6.AGVMessage
{
    public enum ACTION_TYPE
    {
        Move, Unload, Load, Charge, Discharge, Escape
    }

    public class clsAGVSMessage
    {
        public string SID { get; set; }
        public string EQName { get; set; }
        public uint System_Bytes { get; set; }
        public Dictionary<string, clsHeader> Header { get; set; }
    }

    public class clsHeader
    {
        public clsHeader()
        {
            Time_Stamp = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
        }
        public string Time_Stamp { get; set; }
    }
    public class cls_0101_OnlineModeQueryHeader : clsHeader
    {

    }
    public class cls_0102_OnlineModeQueryAckHeader : clsHeader
    {
        public int Remote_Mode { get; set; }
    }
    /// <summary>
    /// 0103
    /// </summary>
    public class cls_0103_OnlineRequestHeader : clsHeader
    {
        public int Mode_Request { get; set; }
        public int Current_Node { get; set; }

    }


    public class cls_0105_RunningStatusReportHeader : clsHeader
    {
        public clsCorrdination Corrdination { get; set; } = new clsCorrdination();
        public int Last_Visited_Node { get; set; } = 0;
        /// <summary>
        /// 1. IDLE: active but no mission
        /// 2. RUN: executing mission
        /// 3. DOWN: alarm or error
        /// 4. Charging: in charging,
        /// 
        /// </summary>
        public int AGV_Status { get; set; } = 3;
        public bool Escape_Flag { get; set; } = false;

        public double CPU_Usage_Percent { get; set; } = 0;
        public double RAM_Usage_Percent { get; set; } = 0;

        public bool AGV_Reset_Flag { get; set; } = true;
        public double Signal_Strength { get; set; } = 0;
        public int Cargo_Status { get; set; } = 0;

        public string[] CSTID { get; set; } = new string[0];
        public double Odometry { get; set; } = 0;
        public double[] Electric_Volume { get; set; } = new double[2] { 0, 0 };
        public clsAlarmCode[] Alarm_Code { get; set; } = new clsAlarmCode[0];
        public double Fork_Height { get; set; } = 0;

    }

    public class cls_0301_TaskDownloadHeader : clsHeader
    {
        public string Task_Name { get; set; }
        public string Task_Simplex { get; set; }
        public int Task_Sequence { get; set; }
        public clsMapPoint[] Trajectory { get; set; } = new clsMapPoint[0];
        public clsMapPoint[] Homing_Trajectory { get; set; } = new clsMapPoint[0];
        public string Action_Type { get; set; }

        public clsCST[] CST { get; set; } = new clsCST[0];
        public int Destination { get; set; }
        public int Height { get; set; }
        public bool Escape_Flag { get; set; }
        public int Station_Type { get; set; }

    }

    public class cls_0303_TaskFeedback : clsHeader
    {
        public string Task_Name { get; set; }
        public string Task_Simplex { get; set; }
        public int Task_Sequence { get; set; }
        public int Point_Index { get; set; }
        /// <summary>
        /// 0-預設(沒有任務)
        /// 1-走行中為狀態，
        /// 2-走行至軌跡最後一點，
        /// 3-開始動作，
        /// 4-動作結束
        /// </summary>
        public int Task_Status { get; set; }
    }


    public class cls_0327_EQCargoSensorResponse : clsHeader
    {
        public int Sensor_Status { get; set; }
    }


    public class cls_0305_AGVSResetCommand : clsHeader
    {
        public int Reset_Mode { get; set; }
    }


    public class cls_0324_CarrierVirtualIDQueryAcknowledge : clsHeader
    {
        public string Virtual_ID { get; set; }
    }



    public class clsReturnCode : clsHeader
    {
        public int Return_Code { get; set; }
        public int Process_Result { get; set; }

        [NonSerialized]
        public bool Timeout = false;
    }


    public class clsCorrdination
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
        public double Theta { get; set; } = 0;
    }

    public class clsAlarmCode
    {
        public int Alarm_ID { get; set; }
        public int Alarm_Level { get; set; }
        public int Alarm_Category { get; set; }
        public string Alarm_Description { get; set; } = "";
    }

    /// <summary>
    /// {
    /// "Point ID": 67,
    /// "X": 4.447,
    /// "Y": 1.388,
    /// "Theta": 90,
    /// "Laser": 1,
    /// "Speed": 0,
    /// "Map Name": "Map_UMTC_3F_Yellow",
    /// "Auto Door": {"Key Name": "","Key Password": ""},
    /// "Control Mode": {"Dodge": 0,"Spin": 0},
    /// "Control Bypass": {"Ground Hole CCD": false,"Ground Hole Sensor": false,"Ultrasonic Sensor": false},
    /// "UltrasonicDistance": 0}
    /// </summary>
    public class clsMapPoint
    {
        public clsMapPoint() { }
        public clsMapPoint(int index)
        {
            this.index = index;
        }
        [NonSerialized]
        public int index;
        public int Point_ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }
        public int Laser { get; set; }
        public double Speed { get; set; }
        public string Map_Name { get; set; } = "";
        public clsAutoDoor Auto_Door { get; set; } = new clsAutoDoor();
        public clsControlMode Control_Mode { get; set; } = new clsControlMode();
        public double UltrasonicDistance { get; set; } = 0;
    }
    public class clsAutoDoor
    {
        public string Key_Name { get; set; }
        public string Key_Password { get; set; }
    }
    public class clsControlMode
    {
        public int Dodge { get; set; }
        public int Spin { get; set; }
    }

    public class clsCST
    {
        public string CST_ID { get; set; }
        public int CST_Type { get; set; }
    }

    public static class Extension
    {
        /// <summary>
        /// 把派車系統Download下來的 Action_Type 轉成列舉
        /// </summary>
        /// <param name="taskData"></param>
        /// <returns></returns>
        public static ACTION_TYPE GetActionTypeEnum(this cls_0301_TaskDownloadHeader taskData)
        {
            return Enum.GetValues(typeof(ACTION_TYPE)).Cast<ACTION_TYPE>().ToList().FirstOrDefault(a => a.ToString() == taskData.Action_Type);
        }

        public static STATION_TYPE GetStationType(this int station_type_code)
        {
            return Enum.GetValues(typeof(STATION_TYPE)).Cast<STATION_TYPE>().ToList().FirstOrDefault(a => (int)a == station_type_code);
        }
    }

}
