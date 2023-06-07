﻿using AGVSystemCommonNet6.AGVDispatch.Messages;
using AGVSystemCommonNet6.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.AGVDispatch
{
    public partial class clsAGVSConnection
    {

        private bool TryTaskDownloadReqAckAsync(bool accept_task, int system_byte)
        {
            if (AGVSMessageStoreDictionary.TryRemove(system_byte, out MessageBase _retMsg))
            {
                byte[] data = AGVSMessageFactory.CreateTaskDownloadReqAckData(accept_task, system_byte, out clsSimpleReturnMessage ackMsg);
                LOG.INFO($"TaskDownload Ack : {ackMsg.ToJson()}");
                return WriteDataOut(data);
            }
            else
                return false;
        }

        public async Task TryTaskFeedBackAsync(clsTaskDownloadData taskData, int point_index, TASK_RUN_STATUS task_status, int currentTAg)
        {
            _ = Task.Run(async () =>
            {
                // PauseRunningStatusReport();
                await Task.Delay(100);

                //if (task_status == TASK_RUN_STATUS.ACTION_FINISH)
                //{
                //    await TryRnningStateReportWithActionFinishAtLastPtAsync();
                //}
                //else
                //{
               // await TryRnningStateReportAsync();
                //}

                //LOG.WARN($"Try Task Feedback to AGVS: Task:{taskData.Task_Name}_{taskData.Task_Simplex}| Point Index : {point_index} | Status : {task_status.ToString()}");

                while (true)
                {
                    byte[] data = AGVSMessageFactory.CreateTaskFeedbackMessageData(taskData, point_index, task_status, out clsTaskFeedbackMessage msg);
                    bool success = await WriteDataOut(data, msg.SystemBytes);
                    if (AGVSMessageStoreDictionary.TryRemove(msg.SystemBytes, out MessageBase _retMsg))
                    {
                        try
                        {
                            clsSimpleReturnMessage msg_return = (clsSimpleReturnMessage)_retMsg;
                            LOG.INFO($" Task Feedback to AGVS RESULT(Task:{taskData.Task_Name}_{taskData.Task_Simplex}| Point Index : {point_index}(Tag:{currentTAg}) | Status : {task_status.ToString()}) ===> {msg_return.ReturnData.ReturnCode}");
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                    else
                    {
                        LOG.ERROR($"TryTaskFeedBackAsync FAIL>.>>");
                    }
                }

                // ResumeRunningStatusReport();

            });

        }

        private async Task<(bool, SimpleRequestResponseWithTimeStamp runningStateReportAck)> TryRnningStateReportWithActionFinishAtLastPtAsync()
        {
            try
            {
                byte[] data = AGVSMessageFactory.CreateRunningStateReportQueryData(out clsRunningStatusReportMessage msg, true);
                await WriteDataOut(data, msg.SystemBytes);
                lastRunningStatusDataReport = msg;
                if (AGVSMessageStoreDictionary.TryRemove(msg.SystemBytes, out MessageBase mesg))
                {
                    clsRunningStatusReportResponseMessage QueryResponseMessage = mesg as clsRunningStatusReportResponseMessage;
                    if (QueryResponseMessage != null)
                        return (true, QueryResponseMessage.RuningStateReportAck);
                    else
                        return (false, null);
                }
                else
                    return (false, null);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        private async Task<(bool, SimpleRequestResponseWithTimeStamp runningStateReportAck)> TryRnningStateReportAsync()
        {
            try
            {
                byte[] data = AGVSMessageFactory.CreateRunningStateReportQueryData(out clsRunningStatusReportMessage msg);
                await WriteDataOut(data, msg.SystemBytes);
                lastRunningStatusDataReport = msg;
                if (AGVSMessageStoreDictionary.TryRemove(msg.SystemBytes, out MessageBase mesg))
                {
                    clsRunningStatusReportResponseMessage QueryResponseMessage = mesg as clsRunningStatusReportResponseMessage;
                    if (QueryResponseMessage != null)
                        return (true, QueryResponseMessage.RuningStateReportAck);
                    else
                        return (false, null);
                }
                else
                    return (false, null);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        private void TryTaskResetReqAckAsync(bool reset_accept, int system_byte)
        {
            byte[] data = AGVSMessageFactory.CreateSimpleReturnMessageData("0306", reset_accept, system_byte, out clsSimpleReturnWithTimestampMessage msg);
            Console.WriteLine(msg.ToJson());
            bool writeOutSuccess = WriteDataOut(data);
            Console.WriteLine("TryTaskResetReqAckAsync : " + writeOutSuccess);

        }
        public async Task<(bool success, RETURN_CODE return_code)> TrySendOnlineModeChangeRequest(int currentTag, REMOTE_MODE mode)
        {
            Console.WriteLine($"[Online Mode Change] 車載請求 {mode} , Tag {currentTag}");
            try
            {
                WaitAGVSAcceptOnline = new ManualResetEvent(false);
                byte[] data = AGVSMessageFactory.CreateOnlineModeChangeRequesData(currentTag, mode, out clsOnlineModeRequestMessage msg);
                await WriteDataOut(data, msg.SystemBytes);
                if (AGVSMessageStoreDictionary.TryRemove(msg.SystemBytes, out MessageBase mesg))
                {
                }
                WaitAGVSAcceptOnline.WaitOne(1000);
                bool success = AGVOnlineReturnCode == RETURN_CODE.OK;
                return (success, AGVOnlineReturnCode);
            }
            catch (Exception ex)
            {
                LOG.WARN($"[AGVS] OnlineModeChangeRequest Fail...Code Error:{ex.Message}");
                return (false, RETURN_CODE.System_Error);
            }
        }


        public async Task<(bool, OnlineModeQueryResponse onlineModeQuAck)> TryOnlineModeQueryAsync()
        {
            try
            {
                byte[] data = AGVSMessageFactory.CreateOnlineModeQueryData(out clsOnlineModeQueryMessage msg);
                await WriteDataOut(data, msg.SystemBytes);

                if (AGVSMessageStoreDictionary.TryRemove(msg.SystemBytes, out MessageBase mesg))
                {
                    clsOnlineModeQueryResponseMessage QueryResponseMessage = mesg as clsOnlineModeQueryResponseMessage;
                    return (true, QueryResponseMessage.OnlineModeQueryResponse);
                }
                else
                    return (false, null);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        public async Task<RETURN_CODE> TryRemoveCSTData(string toRemoveCSTID, string task_name = "", string opid = "")
        {
            try
            {
                byte[] data = AGVSMessageFactory.CreateCarrierRemovedData(new string[] { toRemoveCSTID }, task_name, opid, out clsCarrierRemovedMessage msg);
                await WriteDataOut(data, msg.SystemBytes);

                if (AGVSMessageStoreDictionary.TryRemove(msg.SystemBytes, out MessageBase mesg))
                {
                    clsSimpleReturnWithTimestampMessage CarrierRemovedAckMessage = mesg as clsSimpleReturnWithTimestampMessage;
                    return CarrierRemovedAckMessage.ReturnData.ReturnCode;
                }
                else
                    return RETURN_CODE.System_Error;
            }
            catch (Exception)
            {
                return RETURN_CODE.System_Error;
            }
        }
    }
}
