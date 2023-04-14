using AGVSytemCommonNet6.MAP;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGVSytemCommonNet6.TASK
{
    public class clsTaskDispatchDto
    {
        public string TaskName { get; set; } = "";
        public string DesignatedAGVName { get; set; } = "";
        public enum ACTIONS
        {
            MOVE,
            LOAD,
            UNLOAD,
            CHARGE,
            CARRY
        }

        public ACTIONS Action { get; set; } = ACTIONS.MOVE;
        public string ActionName
        {
            get
            {
                switch (this.Action)
                {
                    case ACTIONS.MOVE:
                        return "移動";
                    case ACTIONS.LOAD:
                        return "放貨";
                    case ACTIONS.UNLOAD:
                        return "取貨";
                    case ACTIONS.CHARGE:
                        return "充電";
                    case ACTIONS.CARRY:
                        return "搬運";
                    default:
                        return "未知";
                }
            }
        }
        public string From_Station { get; set; } = "";
        public string From_Slot { get; set; } = "";
        public string To_Station { get; set; } = "";
        public string To_Slot { get; set; } = "";
        public string Carrier_ID { get; set; } = "";
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; set; } = 50;

    }
}
