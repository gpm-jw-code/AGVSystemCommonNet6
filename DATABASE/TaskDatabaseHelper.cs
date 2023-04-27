using AGVSytemCommonNet6.TASK;
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
            var finishone = alltasks.First(tsk => tsk.State == TASK_RUN_STATE.FINISH);
            return alltasks;
        }


        public List<clsTaskDto> GetALLInCompletedTask()
        {
            using (var dbhelper = new DbContextHelper(connection_str))
            {
                var incompleteds = dbhelper._context.Set<clsTaskDto>().Where(tsk => tsk.State == TASK_RUN_STATE.WAIT| tsk.State == TASK_RUN_STATE.RUNNING).OrderByDescending(t => t.RecieveTime).ToList();
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
                var incompleteds = dbhelper._context.Set<clsTaskDto>().Where(tsk => tsk.State == TASK_RUN_STATE.FINISH| tsk.State == TASK_RUN_STATE.FAILURE).OrderByDescending(t => t.RecieveTime).ToList();
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

        public bool ModifyState(clsTaskDto executingTask, TASK_RUN_STATE state)
        {
            try
            {
                clsTaskDto? taskExist = dbhelper._context.Set<clsTaskDto>().FirstOrDefault(tsk => tsk.TaskName == executingTask.TaskName);
                if (taskExist != null)
                {
                    if (state == TASK_RUN_STATE.FAILURE | state == TASK_RUN_STATE.FINISH)
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
                    // TODO: 處置受控狀態 (受控物件)
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
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
