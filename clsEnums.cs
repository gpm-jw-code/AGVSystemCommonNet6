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

        public enum STATION_TYPE
        {
            Normal = 0,
            EQ = 1,
            STK = 2,
            Charge = 3,
            Buffer = 4,
            Charge_Buffer = 5,
            Charge_STK = 6,
            Escape = 8,
            EQ_LD = 11,
            STK_LD = 12,
            EQ_ULD = 21,
            STK_ULD = 22,
            Fire_Door = 31,
            Fire_EQ = 32,
            Auto_Door = 33,
            Elevator = 100,
            Unknown = 9999
        }
    }
}
