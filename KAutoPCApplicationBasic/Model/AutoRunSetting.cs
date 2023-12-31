using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoPCApplicationBasic.Model
{
    public class AutoRunSetting
    {
        public static string SaveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"AutoRun");
        public AutoRunSetting() { 
            if (!Directory.Exists(SaveFilePath)) {
            Directory.CreateDirectory(SaveFilePath);
                AutoRun.ListAutoRun.Add(new ApplicationRunInfo());
            }
        }
    }
    public static class AutoRun
    {
        public static List<ApplicationRunInfo> ListAutoRun { get; set; } = new List<ApplicationRunInfo>();
           

    }
    public class ApplicationRunInfo
    {
        public ApplicationRunInfo() {
            AppName = "com.dots.connect.game.one";
            TapRecord = new TapRecord(180,567);

        }
        public string? AppName { get; set; }
        public TapRecord? TapRecord { get;set; }
    }
    public class TapRecord
    {
        public TapRecord(int x,int y) {
        this.Xpos = x; this.Ypos = y; Delay = 0;
        }
        public int Xpos { get; set; } = 0;
        public int Ypos { get; set; } = 0;
        public int Delay { get; set; } = 0;
    }
}
