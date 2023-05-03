using AGVSystemCommonNet6.HttpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSystemCommonNet6.Microservices
{
    public class MapSync
    {
        public static async Task<(bool,string)> SendReloadRequest()
        {
            try
            {
                bool alive = await Http.GetAsync<bool>(string.Format("{0}/{1}",Configs.VMSHost,"api/Map/Reload"));
                return (alive, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
