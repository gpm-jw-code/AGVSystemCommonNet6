using AGVSytemCommonNet6.AGVMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AGVSytemCommonNet6.TASK
{
    public enum ACTIONS
    {
        MOVE,
        LOAD,
        UNLOAD,
        CHARGE,
        CARRY
    }
    public enum TASK_RUN_STATE
    {
        WAIT,
        RUNNING,
        FINISH,
        FAILURE
    }
    public class clsTaskDto
    {
        public DateTime RecieveTime { get; set; }
        public DateTime FinishTime { get; set; }
       
        [Key]
        public string TaskName { get; set; } = string.Empty;

        [Required]
        public TASK_RUN_STATE State { get; set; }
        public string StateName
        {
            get
            {
                switch (this.State)
                {
                    case TASK_RUN_STATE.WAIT:
                        return "等待";
                    case TASK_RUN_STATE.RUNNING:
                        return "執行中";
                    case TASK_RUN_STATE.FINISH:
                        return "完成";
                    case TASK_RUN_STATE.FAILURE:
                        return "失敗";
                    default:
                        return "等待";
                }
            }

        }

        /// <summary>
        /// 派工人員名稱
        /// </summary>
        /// 
        public string DispatcherName { get; set; } = string.Empty;

        /// <summary>
        /// 失敗原因
        /// </summary>
        /// 
        public string FailureReason { get; set; } = string.Empty;

        public string DesignatedAGVName { get; set; } = "";
     

        [Required]
        public ACTIONS Action { get; set; }
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

        [Required]
        public string From_Station { get; set; } = "-1";

        [Required]
        public string From_Slot { get; set; } = "-1";

        [Required]
        public string To_Station { get; set; } = "-1";

        [Required]
        public string To_Slot { get; set; } = "-1";

        [Required]
        public string Carrier_ID { get; set; } = "";
        /// <summary>
        /// 優先度
        /// </summary>
        /// 
        [Required]
        public int Priority { get; set; } = 50;
    }
}
