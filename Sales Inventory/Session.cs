using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales_Inventory
{
    internal class Session
    {
        public static string FullName { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static int ShiftID { get; set; }

        public static DateTime ShiftStart { get; set; }  // ✅ Added for EndShift
    }

}
