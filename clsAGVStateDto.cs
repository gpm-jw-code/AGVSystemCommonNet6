using AGVSystemCommonNet6;
using AGVSystemCommonNet6.TASK;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6
{
    public class clsAGVStateDto
    {
        [Key]
        public string AGV_Name { get; set; }
        public string AGV_Description { get; set; } = "";
        public clsEnums.AGV_MODEL Model { get; set; }
        public clsEnums.MAIN_STATUS MainStatus { get; set; }
        public clsEnums.ONLINE_STATE OnlineStatus { get; set; }
        public string CurrentLocation { get; set; } = "";
        public string CurrentCarrierID { get; set; } = "";
        public double BatteryLevel { get; set; }
        public bool Connected { get; set; }
        public string TaskName { get; set; } = "";

        public TASK_RUN_STATE TaskRunStatus { get; set; }
        public ACTIONS TaskRunAction { get; set; }

        public double Theta { get; set; }
    }
}
