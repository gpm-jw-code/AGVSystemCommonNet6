﻿using AGVSystemCommonNet6.Abstracts;
using AGVSystemCommonNet6.AGVDispatch.Messages;
using AGVSystemCommonNet6.Log;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AGVSystemCommonNet6.AGVDispatch
{
    public partial class clsAGVSConnection : Connection
    {
        TcpClient tcpClient;
        clsSocketState socketState = new clsSocketState();
        ConcurrentDictionary<int, ManualResetEvent> WaitAGVSReplyMREDictionary = new ConcurrentDictionary<int, ManualResetEvent>();
        ConcurrentDictionary<int, MessageBase> AGVSMessageStoreDictionary = new ConcurrentDictionary<int, MessageBase>();
        public delegate bool taskDonwloadExecuteDelage(clsTaskDownloadData taskDownloadData);
        public delegate bool onlineModeChangeDelelage(REMOTE_MODE mode);
        public delegate bool taskResetReqDelegate(RESET_MODE reset_data);
        public event EventHandler<clsTaskDownloadData> OnTaskDownloadFeekbackDone;
        public taskDonwloadExecuteDelage OnTaskDownload;
        public onlineModeChangeDelelage OnRemoteModeChanged;
        public taskResetReqDelegate OnTaskResetReq;
        private clsRunningStatusReportMessage lastRunningStatusDataReport = new clsRunningStatusReportMessage();
        private ManualResetEvent RunningStatusRptPause = new ManualResetEvent(true);
        public enum MESSAGE_TYPE
        {
            REQ_0101 = 0101,
            ACK_0102 = 0102,
            REQ_0103 = 0103,
            ACK_0104 = 0104,
            REQ_0105 = 0105,
            ACK_0106 = 0106,
            REQ_0301 = 0301,
            ACK_0302 = 0302,
            REQ_0303 = 0303,
            ACK_0304 = 0304,
            REQ_0305 = 0305,
            ACK_0306 = 0306,
            ACK_0322 = 0322,
            UNKNOWN = 9999,
        }

        public string LocalIP { get; }
        public clsAGVSConnection(string IP, int Port) : base(IP, Port)
        {
            this.IP = IP;
            this.Port = Port;
            LocalIP = null;
        }
        public clsAGVSConnection(string HostIP, int HostPort, string localIP)
        {
            this.IP = HostIP;
            this.Port = HostPort;
            this.LocalIP = localIP;
        }


        public override bool Connect()
        {
            try
            {
                if (LocalIP != null)
                {
                    IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Parse(LocalIP), 0);
                    tcpClient = new TcpClient(ipEndpoint);
                    tcpClient.ReceiveBufferSize = 65535;
                    tcpClient.Connect(IP, Port);
                }
                else
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(IP, Port);
                }
                socketState.stream = tcpClient.GetStream();
                socketState.Reset();
                socketState.stream.BeginRead(socketState.buffer, socketState.offset, clsSocketState.buffer_size - socketState.offset, ReceieveCallbaak, socketState);
                LOG.INFO($"[AGVS] Connect To AGVS Success !!");
                return true;
            }
            catch (Exception ex)
            {
                LOG.ERROR($"[AGVS] Connect Fail..{ex.Message}. Can't Connect To AGVS ({IP}:{Port})..Will Retry it after 3 secoond...");
                tcpClient = null;
                Thread.Sleep(3000);
                return false;
            }
        }

        public void Start()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (!IsConnected())
                    {
                        Connect();
                        continue;
                    }
                    (bool, OnlineModeQueryResponse onlineModeQuAck) result = TryOnlineModeQueryAsync().Result;
                    if (!result.Item1)
                    {
                        LOG.Critical("[AGVS] OnlineMode Query Fail...AGVS No Response");
                        continue;
                    }
                    else
                    {
                        // LOG.TRACE($" OnlineMode Query Done=>Remote Mode : {result.onlineModeQuAck.RemoteMode}");
                    }
                    Task.Factory.StartNew(() =>
                    {
                        RunningStatusRptPause.WaitOne();
                        (bool, SimpleRequestResponseWithTimeStamp runningStateReportAck) runningStateReport_result = TryRnningStateReportAsync().Result;
                        if (!runningStateReport_result.Item1)
                            LOG.Critical("[AGVS] Running State Report Fail...AGVS No Response");
                        else
                        {
                            // LOG.TRACE($" RunningState Report Done=> ReturnCode: {runningStateReport_result.runningStateReportAck.ReturnCode}");
                        }
                    });

                }
            });
            thread.IsBackground = false;
            thread.Start();
        }

        public void PauseRunningStatusReport()
        {
            RunningStatusRptPause.Reset();
        }

        public void ResumeRunningStatusReport()
        {
            RunningStatusRptPause.Set();
        }
        void ReceieveCallbaak(IAsyncResult ar)
        {
            clsSocketState _socketState = (clsSocketState)ar.AsyncState;
            try
            {

                int rev_len = _socketState.stream.EndRead(ar);

                string _revStr = Encoding.ASCII.GetString(_socketState.buffer, _socketState.offset, rev_len);
                _socketState.revStr += _revStr;
                _socketState.offset += rev_len;

                if (_revStr.EndsWith("*\r"))
                {
                    string strHandle = _socketState.revStr.Replace("*\r", "$");
                    string[] splited = strHandle.Split('$');//預防粘包，包含多個message包

                    foreach (var str in splited)
                    {
                        if (str == "" | str == null | str == "\r")
                            continue;
                        string _json = str.TrimEnd(new char[] { '*' });
                        HandleAGVSJsonMsg(_json);
                    }
                    _socketState.Reset();
                    _socketState.waitSignal.Set();

                }
                else
                {
                }

                try
                {
                    Task.Factory.StartNew(() => _socketState.stream.BeginRead(_socketState.buffer, _socketState.offset, clsSocketState.buffer_size - _socketState.offset, ReceieveCallbaak, _socketState));
                }
                catch (Exception ex)
                {
                    tcpClient.Dispose();
                    tcpClient = null;
                }
            }
            catch (Exception)
            {
                tcpClient.Dispose();
                tcpClient = null;
            }

        }
        private REMOTE_MODE CurrentREMOTE_MODE_Downloaded = REMOTE_MODE.OFFLINE;
        private RETURN_CODE AGVOnlineReturnCode;
        private ManualResetEvent WaitAGVSAcceptOnline = new ManualResetEvent(false);
        public Task CarrierRemovedRequestAsync(string v, string[] vs)
        {
            throw new NotImplementedException();
        }

        public MESSAGE_TYPE GetMESSAGE_TYPE(string message_json)
        {

            var _Message = JsonConvert.DeserializeObject<Dictionary<string, object>>(message_json);

            string headerContent = _Message["Header"].ToString();
            var headers = JsonConvert.DeserializeObject<Dictionary<string, object>>(headerContent);

            var firstHeaderKey = headers.Keys.First();

            if (firstHeaderKey.Contains("0101"))
                return MESSAGE_TYPE.REQ_0101;
            if (firstHeaderKey.Contains("0102"))
                return MESSAGE_TYPE.ACK_0102;
            if (firstHeaderKey.Contains("0103"))
                return MESSAGE_TYPE.REQ_0103;
            if (firstHeaderKey.Contains("0104"))
                return MESSAGE_TYPE.ACK_0104;
            if (firstHeaderKey.Contains("0105"))
                return MESSAGE_TYPE.REQ_0105;
            if (firstHeaderKey.Contains("0106"))
                return MESSAGE_TYPE.ACK_0106;
            if (firstHeaderKey.Contains("0301"))
                return MESSAGE_TYPE.REQ_0301;
            if (firstHeaderKey.Contains("0302"))
                return MESSAGE_TYPE.ACK_0302;

            if (firstHeaderKey.Contains("0303"))
                return MESSAGE_TYPE.REQ_0303;

            if (firstHeaderKey.Contains("0304"))
                return MESSAGE_TYPE.ACK_0304;

            if (firstHeaderKey.Contains("0305"))
                return MESSAGE_TYPE.REQ_0305;

            if (firstHeaderKey.Contains("0306"))
                return MESSAGE_TYPE.ACK_0306;
            if (firstHeaderKey.Contains("0322"))
                return MESSAGE_TYPE.ACK_0322;
            else
                return MESSAGE_TYPE.UNKNOWN;
        }
        public override void Disconnect()
        {
            tcpClient?.Dispose();
        }

        public override bool IsConnected()
        {
            return tcpClient != null && tcpClient.Connected;
        }



        public bool WriteDataOut(byte[] dataByte)
        {
            if (!IsConnected())
                return false;
            try
            {
                socketState.stream.Write(dataByte, 0, dataByte.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public async Task<bool> WriteDataOut(byte[] dataByte, int systemBytes)
        {
            if (!IsConnected())
                return false;
            Task _task = new Task(() =>
            {
                try
                {
                    ManualResetEvent manualResetEvent = new ManualResetEvent(false);
                    socketState.stream.Write(dataByte, 0, dataByte.Length);
                    if (WaitAGVSReplyMREDictionary.ContainsKey(systemBytes))
                    {

                    }
                    bool addsucess = WaitAGVSReplyMREDictionary.TryAdd(systemBytes, manualResetEvent);

                    if (addsucess)
                        manualResetEvent.WaitOne();
                    else
                    {
                        LOG.WARN($"[WriteDataOut] 將 'ManualResetEvent' 加入 'WaitAGVSReplyMREDictionary' 失敗");
                    }

                }
                catch (IOException ioex)
                {
                    Console.WriteLine($"[AGVS] 發送訊息的過程中發生 IOException : {ioex.Message}");
                    Disconnect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AGVS] 發送訊息的過程中發生未知的錯誤  {ex.Message}");
                    Disconnect();
                }

            });
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(Debugger.IsAttached ? 13 : 1));

            try
            {
                _task.Start();
                _task.Wait(cts.Token);
                return true;
            }
            catch (OperationCanceledException ex)
            {
                return false;
            }
        }


    }
}
