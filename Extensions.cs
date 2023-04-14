using AGVSytemCommonNet6.AGVMessage;
using AGVSytemCommonNet6.MAP;
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

        /// <summary>
        /// 取得站點類型
        /// </summary>
        /// <param name="map_station"></param>
        /// <returns></returns>
        public static STATION_TYPE GetStationTypeEnum(this MapStation? map_station)
        {
            if (map_station == null)
                return STATION_TYPE.Unknown;
            return Enum.GetValues(typeof(STATION_TYPE)).Cast<STATION_TYPE>().First(st => (int)st == map_station.StationType);
        }


        /// <summary>
        /// 該站點是否可充電
        /// </summary>
        /// <param name="map_station"></param>
        /// <returns></returns>
        public static bool IsChargeAble(this MapStation map_station)
        {
            STATION_TYPE station_type = map_station.GetStationTypeEnum();
            return station_type == STATION_TYPE.Charge | station_type == STATION_TYPE.Charge_Buffer | station_type == STATION_TYPE.Charge_STK;
        }

        /// <summary>
        /// 該站點是否可供AGV Load/Unload
        /// </summary>
        /// <param name="map_station"></param>
        /// <returns></returns>
        public static bool IsLoadAble(this MapStation map_station)
        {
            STATION_TYPE station_type = map_station.GetStationTypeEnum();
            return station_type == STATION_TYPE.EQ | station_type == STATION_TYPE.EQ_LD
                    | station_type == STATION_TYPE.STK | station_type == STATION_TYPE.STK_LD
                    | station_type == STATION_TYPE.Charge_STK;
        }
        /// <summary>
        /// 該站點是否可供AGV Load/Unload
        /// </summary>
        /// <param name="map_station"></param>
        /// <returns></returns>
        public static bool IsUnloadAble(this MapStation map_station)
        {
            STATION_TYPE station_type = map_station.GetStationTypeEnum();
            return station_type == STATION_TYPE.EQ | station_type == STATION_TYPE.EQ_ULD
                    | station_type == STATION_TYPE.STK | station_type == STATION_TYPE.STK_ULD
                    | station_type == STATION_TYPE.Charge_STK;
        }
        /// <summary>
        /// 計算與站點的距離
        /// </summary>
        /// <param name="map_station"></param>
        /// <param name="from_loc_x"></param>
        /// <param name="from_loc_y"></param>
        /// <returns></returns>
        public static double CalculateDistance(this MapStation map_station, double from_loc_x, double from_loc_y)
        {
            return Math.Sqrt(Math.Pow(map_station.X - from_loc_x, 2) + Math.Pow(map_station.Y - from_loc_y, 2));
        }


        /// <summary>
        /// 計算與站點的距離
        /// </summary>
        /// <param name="map_station"></param>
        /// <param name="from_loc_x"></param>
        /// <param name="from_loc_y"></param>
        /// <returns></returns>
        public static double CalculateDistance(this MapStation map_station, MapStation from_station)
        {
            return map_station.CalculateDistance(from_station.X, from_station.Y);
        }
    }
}
