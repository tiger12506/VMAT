using System.Collections.Generic;

namespace VMAT.Models
{
    public static class GlobalReservedIP
    {
        private static readonly object syncSwitch = new object();
        private static Dictionary<string, string> reservedIpList;

        public static void CreateList()
        {
            lock (syncSwitch)
                reservedIpList = new Dictionary<string, string>();
        }

        public static string GetItemByTag(string tag)
        {
            lock (syncSwitch)
                return reservedIpList[tag];
        }

        public static Dictionary<string, string> GetReservedIPs()
        {
            lock (syncSwitch)
                return reservedIpList;
        }

        public static void ReserveIP(string imagePathName, string ip)
        {
            lock (syncSwitch)
                reservedIpList[imagePathName] = ip;
        }

        public static void UnreserveIP(string ip)
        {
            lock (syncSwitch)
            {
                foreach (var image in reservedIpList.Keys)
                {
                    if (reservedIpList[image] == ip)
                        reservedIpList.Remove(image);
                }
            }
        }
    }
}
