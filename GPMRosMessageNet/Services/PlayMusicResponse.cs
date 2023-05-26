using RosSharp.RosBridgeClient;
namespace AGVSystemCommonNet6.GPMRosMessageNet.Services
{
    public class PlayMusicResponse : Message
    {
        public const string RosMessageName = "sound_play_adapter/PlayMusic";

        public bool success { get; set; }

        public PlayMusicResponse()
        {
            this.success = false;
        }

        public PlayMusicResponse(bool success)
        {
            this.success = success;
        }
    }
}
