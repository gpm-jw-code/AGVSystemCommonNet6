using AGVSytemCommonNet6.TASK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.DATABASE
{
    public class TaskDatabaseHelper
    {
        private readonly string connection_str;
        private DbContextHelper dbhelper;
        public TaskDatabaseHelper(string connection_str = "Data Source=D://param//Database//AGVSWebSystem.db")
        {
            this.connection_str = connection_str;
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
                var incompleteds = dbhelper._context.Set<clsTaskDto>().Where(tsk => tsk.State != TASK_RUN_STATE.FINISH).OrderByDescending(t => t.RecieveTime).ToList();
                return incompleteds;
            }
        }

        public List<clsTaskDto> GetALLCompletedTask()
        {
            using (var dbhelper = new DbContextHelper(connection_str))
            {
                var incompleteds = dbhelper._context.Set<clsTaskDto>().Where(tsk => tsk.State != TASK_RUN_STATE.WAIT).OrderByDescending(t => t.RecieveTime).ToList();
                return incompleteds;
            }
        }

        public int AddTask(clsTaskDto taskState)
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

        public bool ModifyTaskState(clsTaskDto executingTask, TASK_RUN_STATE state)
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
    }
}
