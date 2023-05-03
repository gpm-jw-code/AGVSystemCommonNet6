using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.MAP
{
    public class Map
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public int PointIndex { get; set; }
        public Dictionary<int, MapStation> Points { get; set; }
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Ignore )]
        public Dictionary<string, Bay> Bays { get; set; }
    }
}
