﻿using AGVSystemCommonNet6.AGVDispatch.Messages;
using AGVSystemCommonNet6.TASK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.DATABASE
{
    public class TaskDatabaseHelper:IDisposable
    {
        protected readonly string connection_str;
        protected  DbContextHelper dbhelper;
        private bool disposedValue;
        public TaskDatabaseHelper()
        {
            this.connection_str = Configs.DBConnection;
            dbhelper = new DbContextHelper(connection_str);
        }

        public List<clsTaskDto> GetALL()
        {
            var alltasks = dbhelper._context.Tasks.ToList();
            var finishone = alltasks.First(tsk => tsk.State == TASK_RUN_STATUS.ACTION_FINISH);
            return alltasks;
        }


        public List<clsTaskDto> GetALLInCompletedTask()
        {
            using (var dbhelper = new DbContextHelper(connection_str))
            {
                var incompleteds = dbhelper._context.Set<clsTaskDto>().Where(tsk => tsk.State == TASK_RUN_STATUS.WAIT| tsk.State == TASK_RUN_STATUS.NAVIGATING).OrderByDescending(t => t.RecieveTime).ToList();
                if(incompleteds.Count > 0)
                {


                }
                return incompleteds;
            }
        }

        public List<clsTaskDto> GetALLCompletedTask()
        {
            using (var dbhelper = new DbContextHelper(connection_str))
            {
                var incompleteds = dbhelper._context.Set<clsTaskDto>().Where(tsk => tsk.State == TASK_RUN_STATUS.NAVIGATING| tsk.State == TASK_RUN_STATUS.FAILURE).OrderByDescending(t => t.RecieveTime).ToList();
                return incompleteds;
            }
        }

        /// <summary>
        /// 新增一筆任務資料
        /// </summary>
        /// <param name="taskState"></param>
        /// <returns></returns>
        virtual public int Add(clsTaskDto taskState)
        {
            try
            {
                using (var dbhelper = new DbContextHelper(connection_str))
                {
                    Console.WriteLine($"{JsonConvert.SerializeObject(taskState, Formatting.Indented)}");
                    dbhelper._context.Set<clsTaskDto>().Add(taskState);
                    int ret = dbhelper._context.SaveChanges();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool ModifyState(clsTaskDto executingTask, TASK_RUN_STATUS state)
        {
            try
            {
                clsTaskDto? taskExist = dbhelper._context.Set<clsTaskDto>().FirstOrDefault(tsk => tsk.TaskName == executingTask.TaskName);
                if (taskExist != null)
                {
                    if (state == TASK_RUN_STATUS.FAILURE | state == TASK_RUN_STATUS.ACTION_FINISH)
                        taskExist.FinishTime = DateTime.Now;
                    taskExist.State = state;
                    int ret = dbhelper._context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                int ret = dbhelper._context.SaveChanges();
                return false;
            }
        }

        public bool ModifyFromToStation(clsTaskDto executingTask, int from_station, int to_station)
        {
            try
            {
                clsTaskDto? taskExist = dbhelper._context.Set<clsTaskDto>().FirstOrDefault(tsk => tsk.TaskName == executingTask.TaskName);
                if (taskExist != null)
                {
                    taskExist.From_Station = from_station.ToString();
                    taskExist.To_Station = to_station.ToString();
                    int ret = dbhelper._context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                int ret = dbhelper._context.SaveChanges();
                return false;
            }
        }

        public bool DeleteTask(string task_name)
        {
            try
            {
                clsTaskDto? taskExist = dbhelper._context.Set<clsTaskDto>().FirstOrDefault(tsk => tsk.TaskName == task_name);
                if (taskExist != null)
                {
                    dbhelper._context.Set<clsTaskDto>().Remove(taskExist);
                    dbhelper._context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        // ~TaskDatabaseHelper()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
