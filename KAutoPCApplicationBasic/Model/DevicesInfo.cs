using KAutoPCApplicationBasic.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoPCApplicationBasic.Model
{
    public class DevicesInfo
    {
        public string LD_Index { get; set; }
        public string? LD_Name { get; set; }
        public string? Adb_id_Name { get; set; }
        public bool? Running { get; set; }
        public IntPtr HandleWindow { get; set; }
        public IntPtr TopWindowHandle { get; set; }
        public IntPtr BindWindowHandle { get; set; }
        public DevicesInfo()
        {
            
        }
        public DevicesInfo(Info_Devices2 info, IntPtr intPtr)
        {
            LD_Index = info.index.ToString();
            LD_Name = info.name;
            Adb_id_Name = info.adb_id;
            HandleWindow = intPtr;
            this.Running = false;
            this.TopWindowHandle = (IntPtr)info.tophw;
            this.BindWindowHandle = (IntPtr)info.bindhw;
        }

    }
}
