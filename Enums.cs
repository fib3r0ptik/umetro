using System;
using System.Collections.Generic;
using System.Linq;

namespace EZTVMetro {
    public static class Enums {
        [Flags]
        public enum Status {
            Started = 1,
            Checking = 2,
            StartAfterCheck = 4,
            Checked = 8,
            Error = 16,
            Paused = 32,
            Queued = 64,
            Loaded = 128
        }
        public enum Actions {
            Verify, ForceStart, Start, Stop, ReAnnounce, Pause, UnPuase, ReCheck, Remove, RemoveData, SetSettings, List
        }
        public enum ClientTypes {
            UTORRENT, TRANSMISSION
        }
        public  enum ClientFields {
            ID, NAME, TYPE, HOST, PORT, UID, PWD, AUTH
        }
        public enum ViewFilter {
            COMPLETED, RUNNING, PAUSED, QUEUED, ALL
        }

    }
}
