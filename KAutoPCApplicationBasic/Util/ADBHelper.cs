using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
namespace KAutoPCApplicationBasic.Util
{
    public class ADBHelper
    {
        private static string LIST_DEVICES = "adb devices";
        private static string TAP_DEVICES = "adb -s {0} shell input tap {1} {2}";
        private static string SWIPE_DEVICES = "adb -s {0} shell input swipe {1} {2} {3} {4} {5}";
        private static string KEY_DEVICES = "adb -s {0} shell input keyevent {1}";
        private static string INPUT_TEXT_DEVICES = "adb -s {0} shell input text \"{1}\"";
        private static string CAPTURE_SCREEN_TO_DEVICES = "adb -s {0} shell screencap -p \"{1}\"";
        private static string PULL_SCREEN_FROM_DEVICES = "adb -s {0} pull \"{1}\"";
        private static string REMOVE_SCREEN_FROM_DEVICES = "adb -s {0} shell rm -f \"{1}\"";
        private static string GET_SCREEN_RESOLUTION = "adb -s {0} shell dumpsys display | Find \"mCurrentDisplayRect\"";
        private const int DEFAULT_SWIPE_DURATION = 100;
        private static string ADB_FOLDER_PATH = "";
        private static string ADB_PATH = "";
        private static string GET_IMEI = "adb -s {0} shell \"service call iphonesubinfo 1 s16 com.android.shell | cut -c 52-66 | tr -d '.[:space:]'\"";

        

        public static string ExecuteCMD(string cmdCommand)
        {
            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = ADBHelper.ADB_FOLDER_PATH,
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    Verb = "runas"
                };
                process.Start();
                process.StandardInput.WriteLine(cmdCommand);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit(3000);
                return process.StandardOutput.ReadToEnd();
            }
            catch
            {
                return "";
            }
        }
        // Get IMEI
        public static string GetIMEI(string deviceID)
        {
            string? result = "";
            string cmdCommand = string.Format(ADBHelper.GET_IMEI, (object)deviceID);
            string cmdCommand2 = cmdCommand.Replace(@"\", "");
            var plainresult = ADBHelper.ExecuteCMD(cmdCommand);
            var liststring = plainresult.Split(new string("\r\n"), StringSplitOptions.RemoveEmptyEntries);
            if (liststring != null)
            {
                result =  liststring.Where(x => x.Length == 15).Any() ? liststring.Where(x => x.Length == 15).FirstOrDefault() : "";
            }
            else
            {
                return result;
            }
            return result;
            
        }
        // Get List Device
        public static List<string> GetDevices()
        {
            List<string> stringList = new List<string>();
            MatchCollection matchCollection = Regex.Matches(ADBHelper.ExecuteCMD("adb devices"), "(?<=List of devices attached)([^\\n]*\\n+)+", RegexOptions.Singleline);
            if (matchCollection.Count > 0)
            {
                foreach (string str1 in Regex.Split(matchCollection[0].Groups[0].Value, "\r\n"))
                {
                    if (!string.IsNullOrEmpty(str1) && str1 != " ")
                    {
                        string[] strArray = str1.Trim().Split('\t');
                        string str2 = strArray[0];
                        try
                        {
                            if (strArray[1] != "device")
                                continue;
                        }
                        catch
                        {
                        }
                        stringList.Add(str2.Trim());
                    }
                }
            }
            return stringList;
        }
        // Get ScreenResolution
        public static Point GetScreenResolution(string deviceID)
        {
            string str1 = ADBHelper.ExecuteCMD(string.Format(ADBHelper.GET_SCREEN_RESOLUTION, (object)deviceID));
            string str2 = str1.Substring(str1.IndexOf("- "));
            string[] strArray = str2.Substring(str2.IndexOf(' '), str2.IndexOf(')') - str2.IndexOf(' ')).Split(',');
            return new Point(Convert.ToInt32(strArray[0].Trim()), Convert.ToInt32(strArray[1].Trim()));
        }
        // Tap By Percent
        public static void TapByPercent(string deviceID, double x, double y, int count = 1)
        {
            Point screenResolution = ADBHelper.GetScreenResolution(deviceID);
            int num1 = (int)(x * ((double)screenResolution.X * 1.0 / 100.0));
            int num2 = (int)(y * ((double)screenResolution.Y * 1.0 / 100.0));
            string cmdCommand = string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)num1, (object)num2);
            for (int index = 1; index < count; ++index)
                cmdCommand = cmdCommand + " && " + string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)x, (object)y);
            ADBHelper.ExecuteCMD(cmdCommand);
        }
        // Tap
        public static void Tap(string deviceID, int x, int y, int count = 1)
        {
            string cmdCommand = string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)x, (object)y);
            for (int index = 1; index < count; ++index)
                cmdCommand = cmdCommand + " && " + string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)x, (object)y);
            ADBHelper.ExecuteCMD(cmdCommand);
        }
        public static void Swipe(string deviceID, int x1, int y1, int x2, int y2, int duration = 100) => ADBHelper.ExecuteCMD(string.Format(ADBHelper.SWIPE_DEVICES, (object)deviceID, (object)x1, (object)y1, (object)x2, (object)y2, (object)duration));
        public static Bitmap ScreenShoot
          (
          string deviceID = null,
          bool isDeleteImageAfterCapture = true,
          string fileName = "screenShoot.png"
          
          )
        {
            if (string.IsNullOrEmpty(deviceID))
            {
                List<string> devices = ADBHelper.GetDevices();
                if (devices == null || devices.Count <= 0)
                    return (Bitmap)null;
                deviceID = devices.First<string>();
            }
            string str1 = deviceID;
            try
            {
                if (str1.Contains(':'))
                {
                    str1 = deviceID.Split(':')[1];
                }
                else
                {
                    str1 = deviceID;
                }
                
            }
            catch
            {
            }
            string path = Path.GetFileNameWithoutExtension(fileName) + str1 + Path.GetExtension(fileName);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                }
            }
            string filename = Directory.GetCurrentDirectory() + "\\" + path;
            //string str2 = "\"" + Directory.GetCurrentDirectory().Replace("\\\\", "\\") + "\"";
            string str2 = Directory.GetCurrentDirectory().Replace("\\\\", "\\") ;
            string cmdCommand1 = string.Format("adb -s {0} shell screencap -p \"{1}\"", (object)deviceID, (object)("/sdcard/" + path));
            string cmdCommand2 = string.Format("adb -s " + deviceID + " pull /sdcard/" + path + " " + str2);
            ADBHelper.ExecuteCMD(cmdCommand1);
            ADBHelper.ExecuteCMD(cmdCommand2);
            Bitmap bitmap1 = (Bitmap)null;
            try
            {
                using (Bitmap bitmap2 = new Bitmap(filename))
                    bitmap1 = new Bitmap((Image)bitmap2);
            }
            catch
            {
            }
            if (isDeleteImageAfterCapture)
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                }
                try
                {
                    ADBHelper.ExecuteCMD(string.Format("adb -s " + deviceID + " shell \"rm /sdcard/" + path + "\""));
                }
                catch
                {
                }
            }
            return bitmap1;
        }
        public static void Key(string deviceID, ADBKeyEvent key) => ADBHelper.ExecuteCMD(string.Format(ADBHelper.KEY_DEVICES, (object)deviceID, (object)key));
    }
}
