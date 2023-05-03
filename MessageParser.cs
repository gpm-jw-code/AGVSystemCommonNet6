using AGVSystemCommonNet6.AGVMessage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AGVSystemCommonNet6
{
    public class MessageParser
    {

        public static Dictionary<string, object> GetDictionary(object obj)
        {
            try
            {
                var dictionary = new Dictionary<string, object>();
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj, null);

                    if (value != null)
                    {
                        string new_propertyName = property.Name.Replace("_", " ");
                        Type type = property.PropertyType;
                        string type_name = property.GetType().Name;
                        if (type.Name == "Dictionary`2")
                        {
                            string json = JsonConvert.SerializeObject(value);
                            var kep = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                            JObject header_content = (JObject)kep.Values.First();
                            Dictionary<string, object> ddd = header_content.ToObject<Dictionary<string, object>>();
                            Dictionary<string, object> ddd_key_replaced = ddd.ToDictionary(kp => kp.Key.Replace("_", " "), kp => kp.Value);
                            dictionary[new_propertyName] = new Dictionary<string, object>()
                            {
                                {kep.Keys.First(),ddd_key_replaced }
                            };

                        }
                        else
                        {
                            dictionary[new_propertyName] = value;
                        }
                    }
                }

                return dictionary;

                //Dictionary<string, object> dict = new Dictionary<string, object>();
                //PropertyInfo[] properties = obj.GetType().GetProperties();
                //foreach (PropertyInfo property in properties)
                //{
                //    object value = property.GetValue(obj);
                //    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                //    {

                //        try
                //        {

                //            value = GetDictionary(value);
                //        }
                //        catch (Exception ex)
                //        {

                //            throw ex;
                //        }
                //    }
                //    dict.Add(property.Name.Replace("_", " "), value);
                //}
                //return dict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static clsAGVSMessage[] MsgParse(string raw_msg)
        {
            if (raw_msg.Contains("0301"))
            {

            }
            string[] splited = raw_msg.Replace("\n", "").Replace("*\r", "&").Split('&');
            string[] msgs = splited.ToList().FindAll(str => str != "").ToArray();
            int msgNums = msgs.Length;
            List<clsAGVSMessage> msgList = new List<clsAGVSMessage>();
            for (int i = 0; i < msgNums; i++)
            {
                clsAGVSMessage agvs_message = new clsAGVSMessage();
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(msgs[i]);

                foreach (var kvp in dictionary)
                {
                    var propertyName = kvp.Key.Replace(" ", "_");
                    var property = agvs_message.GetType().GetProperty(propertyName);
                    if (property != null)
                    {
                        if (propertyName == "Header")
                        {
                            var header_dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(kvp.Value.ToString());

                            var header_ret = ParseHeaderContent(header_dict);
                            property.SetValue(agvs_message, new Dictionary<string, clsHeader>()
                            {
                                {header_ret.header_key,header_ret.header_content }
                            });

                        }
                        else if (propertyName == "System_Bytes")
                        {
                            property.SetValue(agvs_message, uint.Parse(kvp.Value.ToString()));
                        }
                        else
                            property.SetValue(agvs_message, kvp.Value);
                    }
                }

                msgList.Add(agvs_message);
            }
            return msgList.ToArray();
        }
        private static object ReflectDictionary(Dictionary<string, object> dict, object ref_obj)
        {
            foreach (var kvp in dict)
            {
                try
                {
                    var propertyName = kvp.Key.Replace(" ", "_");
                    var property = ref_obj.GetType().GetProperty(propertyName);
                    if (property != null)
                    {
                        var val_type_name = kvp.Value.GetType().Name;
                        if (val_type_name == "Int64")
                        {
                            property.SetValue(ref_obj, int.Parse(kvp.Value.ToString()));
                        }
                        else if (val_type_name == "JObject")
                        {
                            var obj_dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(kvp.Value.ToString());
                            string classNamEnd = kvp.Key.Replace(" ", "").Replace("_", "");
                            Type type = Type.GetType($"AGVSytemCommon.AGVMessage.cls{classNamEnd}");
                            if (type == null)
                            {
                                continue;
                            }

                            object instance = Activator.CreateInstance(type);
                            instance = ReflectDictionary(obj_dict, instance);
                            property.SetValue(ref_obj, instance);
                        }
                        else
                            property.SetValue(ref_obj, kvp.Value);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return ref_obj;
        }

        private static (string header_key, clsHeader header_content) ParseHeaderContent(Dictionary<string, object> header)
        {
            string header_key = header.Keys.First();
            var header_content_dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(header.Values.First().ToString());
            clsHeader headerContent = null;
            if (header_key == "0101")
            {
                cls_0101_OnlineModeQueryHeader _headerContent = new cls_0101_OnlineModeQueryHeader();
                headerContent = _headerContent;
            }
            if (header_key == "0103")
            {
                cls_0103_OnlineRequestHeader _headerContent = new cls_0103_OnlineRequestHeader();
                headerContent = _headerContent;
            }
            if (header_key == "0105")
            {
                cls_0105_RunningStatusReportHeader _headerContent = new cls_0105_RunningStatusReportHeader();
                headerContent = _headerContent;
            }
            if (header_key == "0301")
            {
                cls_0301_TaskDownloadHeader _headerContent = new cls_0301_TaskDownloadHeader();
                headerContent = _headerContent;
            }
            else if (header_key == "0303")
            {
                cls_0303_TaskFeedback _headerContent = new cls_0303_TaskFeedback();
                headerContent = _headerContent;
            }
            else if (header_key == "0305")
            {
                cls_0305_AGVSResetCommand _headerContent = new cls_0305_AGVSResetCommand();
                headerContent = _headerContent;
            }
            else if (header_key == "0324")
            {
                cls_0324_CarrierVirtualIDQueryAcknowledge _headerContent = new cls_0324_CarrierVirtualIDQueryAcknowledge();
                headerContent = _headerContent;
            }
            else if (header_key == "0327")
            {
                cls_0327_EQCargoSensorResponse _headerContent = new cls_0327_EQCargoSensorResponse();
                headerContent = _headerContent;
            }
            else if (header_key == "0102")
            {
                cls_0102_OnlineModeQueryAckHeader _headerContent = new cls_0102_OnlineModeQueryAckHeader();
                headerContent = _headerContent;
            }
            else if (header_key == "0104" | header_key == "0106" | header_key == "0302" | header_key == "0304" | header_key == "0322" | header_key == "0326")
            {
                clsReturnCode _headerContent = new clsReturnCode();
                headerContent = _headerContent;
            }

            if (headerContent == null)
            {

            }
            foreach (var kvp in header_content_dict)
            {
                try
                {

                    var propertyName = kvp.Key.Replace(" ", "_");
                    var property = headerContent.GetType().GetProperty(propertyName);
                    if (property != null)
                    {
                        object property_value = null;
                        if (propertyName == "Time_Stamp")
                        {
                            property_value = kvp.Value.ToString();
                            //
                        }
                        else
                        {
                            var val_type_name = kvp.Value.GetType().Name;
                            if (val_type_name == "Int64")
                            {
                                property_value = int.Parse(kvp.Value.ToString());
                            }
                            else if (val_type_name == "JArray")
                            {

                                if (propertyName == "Trajectory" | propertyName == "Homing_Trajectory")
                                {
                                    var Trajectorys = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(kvp.Value.ToString());
                                    clsMapPoint[] Trajectory = Trajectorys.Select(cst => (clsMapPoint)ReflectDictionary(cst, new clsMapPoint())).ToArray();
                                    property_value = Trajectory;

                                }
                                else if (propertyName == "CST")
                                {
                                    var csts = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(kvp.Value.ToString());
                                    clsCST[] CST = csts.Select(cst => (clsCST)ReflectDictionary(cst, new clsCST())).ToArray();
                                    property_value = CST;
                                }
                                else if (propertyName == "CSTID")
                                {
                                    property_value = JsonConvert.DeserializeObject<string[]>(kvp.Value.ToString());

                                }
                                else if (propertyName == "Electric_Volume")
                                {
                                    property_value = JsonConvert.DeserializeObject<double[]>(kvp.Value.ToString());
                                }
                            }
                            else
                                property_value = kvp.Value;
                        }
                        property.SetValue(headerContent, property_value);
                    }

                }
                catch (Exception ex)
                {
                }
            }


            return (header_key, headerContent);
        }

    }
}
