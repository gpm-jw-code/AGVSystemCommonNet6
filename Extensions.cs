using AGVSystemCommonNet6.AGVDispatch.Messages;
using AGVSystemCommonNet6.AGVMessage;
using AGVSystemCommonNet6.MAP;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AGVSystemCommonNet6.clsEnums;

namespace AGVSystemCommonNet6
{
    public static class Extensions
    {
        public static Time ToStdTime(this DateTime _time)
        {
            return new Time()
            {
                secs = (uint)(_time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds),
                nsecs = (uint)(_time.Millisecond * 1000000),
            };
        }

        public static string ToAGVSTimeFormat(this DateTime _time)
        {
            return _time.ToString("yyyyMMdd HH:mm:ss");
        }

        /// <summary>
        /// 將角度值轉換為 Quaternion(四位元)
        /// </summary>
        /// <param name="Theta"></param>
        /// <returns></returns>
        public static Quaternion ToQuaternion(this double Theta)
        {
            double yaw_radians = (float)Theta * Math.PI / 180.0;
            double cos_yaw = Math.Cos(yaw_radians / 2.0);
            double sin_yaw = Math.Sin(yaw_radians / 2.0);
            return new Quaternion(0.0f, 0.0f, (float)sin_yaw, (float)cos_yaw);
        }

        public static int[] GetRemainPath(this IEnumerable<clsMapPoint> points, int startTag)
        {
            if (points.Count() == 0)
                return new int[1] { startTag };
            
            int find_index(int tag)
            {
                return points.ToList().FindIndex(p => p.Point_ID == tag);
            }
            var startTag_index = find_index(startTag);
            return points.ToList().FindAll(p => find_index(p.Point_ID) >= startTag_index).Select(pt => pt.Point_ID).ToArray();
        }
        public static double ToTheta(this RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion orientation)
        {
            double yaw;
            double x = orientation.x;
            double y = orientation.y;
            double z = orientation.z;
            double w = orientation.w;
            // 計算角度
            double siny_cosp = 2.0 * (w * z + x * y);
            double cosy_cosp = 1.0 - 2.0 * (y * y + z * z);
            yaw = Math.Atan2(siny_cosp, cosy_cosp);
            return yaw * 180.0 / Math.PI;
        }
        public static string ToJson(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return "{}";
            }
        }
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
            if (task.GetActionTypeEnum() == ACTION_TYPE.None)
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
            if (task.GetActionTypeEnum() == ACTION_TYPE.None)
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
