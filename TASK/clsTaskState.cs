using AGVSytemCommonNet6.AGVMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AGVSytemCommonNet6.TASK.clsTaskDispatchDto;

namespace AGVSytemCommonNet6.TASK
{
    public enum TASK_RUN_STATE
    {
        WAIT,
        RUNNING,
        FINISH,
        FAILURE
    }
    public class clsTaskState
    {
        [Key]
        public string TaskName { get; set; }

        [Required]
        public TASK_RUN_STATE State { get; set; } = TASK_RUN_STATE.WAIT;
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
        public clsTaskDispatchDto TaskDispatchData { get; }

        [Required]
        public DateTime RecieveTime { get; }
        public clsTaskState()
        {
            this.RecieveTime = DateTime.Now;
        }

        public clsTaskState(string TaskName, clsTaskDispatchDto TaskDispatchData)
        {
            this.TaskName = TaskName;
            this.TaskDispatchData = TaskDispatchData;
            this.RecieveTime = DateTime.Now;
        }
        public clsTaskState(string TaskName_Simplex)
        {
            this.TaskName = TaskName_Simplex;
            this.RecieveTime = DateTime.Now;
        }

        /// <summary>
        /// 派工人員名稱
        /// </summary>
        /// 
        [Required]
        public string DispatcherName { get; set; } = "";

        /// <summary>
        /// 失敗原因
        /// </summary>
        /// 
        [Required]
        public string FailureReason { get; set; } = "";

    }
}
