using System;
using System.Collections.Generic;
using System.Text;

namespace AGVSytemCommonNet6
{
    public class clsEnums
    {

        public enum AGV_MODEL
        {
            FORK_AGV,
            YUNTECH_FORK_AGV
        }

        public enum ONLINE_STATE
        {
            OFFLINE,
            ONLINE,
            UNKNOWN,
        }

        public enum MAIN_STATUS
        {
            IDLE = 1,
            RUN,
            DOWN,
            Charging
        }
    }
}
