using RosSharp.RosBridgeClient;
namespace AGVSystemCommonNet6.GPMRosMessageNet.Services
{
    public class PlayMusicRequest : Message
    {
        public const string RosMessageName = "sound_play_adapter/PlayMusic";

        /// <summary>
        /// 空字串時會停止音樂
        /// </summary>
        public string file_path { get; set; }
        public int total_sec { get; set; }

        public PlayMusicRequest()
        {
            this.file_path = "";
        }

        public PlayMusicRequest(string request)
        {
            this.file_path = request;
        }


    }
}
