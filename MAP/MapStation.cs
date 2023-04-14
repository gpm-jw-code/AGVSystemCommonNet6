﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSytemCommonNet6.MAP
{
    public class MapStation
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Name { get; set; }
        public int TagNumber { get; set; }
        public int Direction { get; set; }
        public bool Enable { get; set; }
        public bool AGV_Alarm { get; set; }
        public bool IsStandbyPoint { get; set; }
        public bool IsSegment { get; set; }
        public string InvolvePoint { get; set; }
        public int StationType { get; set; }
        public int LsrMode { get; set; }
        public int Speed { get; set; }
        public string Bay { get; set; }
        public bool IsOverlap { get; set; }
        public Graph Graph { get; set; }
        public Dictionary<string, int> Target { get; set; }
        public string DodgeMode { get; set; }
        public string SpinMode { get; set; }
        public string SubMap { get; set; }
        public string IsParking { get; set; }
        public bool IsAvoid { get; set; }
        public AutoDoor AutoDoor { get; set; }
        public MotionInfo MotionInfo { get; set; }
        [NonSerialized]
        internal int Index;
    }
}
