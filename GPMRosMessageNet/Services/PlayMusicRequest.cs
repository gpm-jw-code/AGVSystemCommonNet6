using RosSharp.RosBridgeClient;
namespace AGVSystemCommonNet6.GPMRosMessageNet.Services
{
    public class PlayMusicRequest : Message
    {
        public const string RosMessageName = "sound_play_adapter/PlayMusic";

        public string file_path { get; set; }

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
