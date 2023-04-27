﻿using AGVSystemCommonNet6.DATABASE;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.Alarm
{
    public class AlarmManagerCenter
    {
        public static string ALARM_CODE_FILE_PATH = @"D:\param\AGVS_AlarmCodes.json";
        public static Dictionary<ALARMS, clsAlarmCode> AlarmCodes = new Dictionary<ALARMS, clsAlarmCode>();
        public static List<clsAlarmDto> uncheckedAlarms
        {
            get
            {
                using (var dbhelper = new DbContextHelper(Configs.DBConnection))
                {
                    return dbhelper._context.Set<clsAlarmDto>().Where(al => !al.Checked).ToList();
                }
            }
        }
        private static bool Initialized = false;
        public AlarmManagerCenter() { }

        public static void Initialize()
        {
            var dbhelper = new DbContextHelper(Configs.DBConnection);
            LoadAlarmCodes();
            Initialized = true;
        }
        public static void UpdateAlarm(clsAlarmDto alarmDto)
        {
            try
            {

                var dbhelper = new DbContextHelper(Configs.DBConnection);
                var alarmExist = dbhelper._context.SystemAlarms.FirstOrDefault(alarm => alarm.Time == alarmDto.Time);
                if (alarmExist != null)
                {
                    foreach (var prop in alarmDto.GetType().GetProperties())
                    {
                        var val = prop.GetValue(alarmDto);
                        var val_old = prop.GetValue(alarmExist);
                        if (val_old.ToString() != val.ToString() && prop.PropertyType != typeof(DateTime))
                        {

                            alarmExist.GetType().GetProperty(prop.Name).SetValue(alarmExist, val);
                        }
                    }

                    //dbhelper._context.SystemAlarms.Update(alarmDto);
                    dbhelper._context.SaveChanges();
                }
                else
                    AddAlarm(alarmDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void AddAlarm(clsAlarmDto alarmDto)
        {
            if (!Initialized)
                Initialize();

            try
            {
                var dbhelper = new DbContextHelper(Configs.DBConnection);
                dbhelper._context.SystemAlarms.Add(alarmDto);
                dbhelper._context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddAlarm(ALARMS alarm, ALARM_SOURCE source = ALARM_SOURCE.AGVS, ALARM_LEVEL level = ALARM_LEVEL.ALARM,

             string Equipment_Name = "", string location = "", string taskName = "")
        {
            if (!Initialized)
                Initialize();

            try
            {
                clsAlarmCode alarmCodeData = AlarmCodes[alarm];
                clsAlarmDto alarmDto = new clsAlarmDto()
                {
                    Equipment_Name = Equipment_Name,
                    Description_Zh = alarmCodeData.Description_Zh,
                    Description_En = alarmCodeData.Description_En,
                    Level = level,
                    AlarmCode = (int)alarm,
                    OccurLocation = location == null ? "" : location,
                    Task_Name = taskName == null ? "" : taskName,
                    Time = DateTime.Now,
                    Source = source
                };
                AddAlarm(alarmDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static void LoadAlarmCodes()
        {
            if (File.Exists(ALARM_CODE_FILE_PATH))
            {
                clsAlarmCode[]? _AlarmCodes = JsonConvert.DeserializeObject<clsAlarmCode[]>(File.ReadAllText(ALARM_CODE_FILE_PATH));
                AlarmCodes = _AlarmCodes.ToDictionary(ac => ac.AlarmCode, ac => ac);
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ALARM_CODE_FILE_PATH));
                AlarmCodes.Add(ALARMS.VMS_DISCONNECT, new clsAlarmCode
                {
                    AlarmCode = ALARMS.VMS_DISCONNECT,
                    Description_En = "VMS Disconnect",
                    Description_Zh = "車載管理系統斷線"
                });
                File.WriteAllText(ALARM_CODE_FILE_PATH, JsonConvert.SerializeObject(AlarmCodes.Values.ToArray(), Formatting.Indented));
            }
        }

        public static clsAlarmCode GetAlarmCode(ALARMS alarm_enum)
        {
            if (!Initialized)
                Initialize();
            return AlarmCodes[alarm_enum];
        }

        public static void ResetAlarm(clsAlarmDto alarm, bool resetAllSameCode = false)
        {
            using (var dbhelper = new DbContextHelper(Configs.DBConnection))
            {
                if (resetAllSameCode)
                {
                    var alarms_same_code = dbhelper._context.Set<clsAlarmDto>().Where(_alarm => _alarm.AlarmCode == alarm.AlarmCode).ToList();
                    foreach (var alarm_ in alarms_same_code)
                    {
                        alarm_.Checked = true;
                        alarm_.ResetAalrmMemberName = alarm.ResetAalrmMemberName;
                        UpdateAlarm(alarm_);
                    }
                }
                else
                {
                    if (dbhelper._context.Set<clsAlarmDto>().FirstOrDefault(a => a == alarm) != null)
                    {
                        alarm.Checked = true;
                        UpdateAlarm(alarm);
                    }
                }

            }

        }

        public static void RemoveAlarm(clsAlarmDto alarmDto)
        {
            try
            {
                using (var dbhelper = new DbContextHelper(Configs.DBConnection))
                {
                    dbhelper._context.Set<clsAlarmDto>().Remove(alarmDto);
                    dbhelper._context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
