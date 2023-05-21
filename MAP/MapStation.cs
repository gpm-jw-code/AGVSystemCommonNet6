using AGVSystemCommonNet6.AGVDispatch.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.MAP
{
    public class MapStation
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string? Name { get; set; }
        public int TagNumber { get; set; }
        public int Direction { get; set; }
        public bool Enable { get; set; }
        public bool AGV_Alarm { get; set; }
        public bool IsStandbyPoint { get; set; }
        public bool IsSegment { get; set; }
        public string? InvolvePoint { get; set; }
        public STATION_TYPE StationType { get; set; }
        public int LsrMode { get; set; }
        public double Speed { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Bay { get; set; }
        public bool IsOverlap { get; set; }
        public Graph Graph { get; set; }
        /// <summary>
        /// Key Point Index, value:權重
        /// </summary>
        public Dictionary<string, int>? Target { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? DodgeMode { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? SpinMode { get; set; }
        public string? SubMap { get; set; }
        public string? IsParking { get; set; }
        public bool IsAvoid { get; set; }
        public AutoDoor? AutoDoor { get; set; }
        public MotionInfo? MotionInfo { get; set; }
    }
}
