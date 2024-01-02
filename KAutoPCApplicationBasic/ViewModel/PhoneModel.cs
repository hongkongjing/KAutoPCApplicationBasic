using KAutoPCApplicationBasic.Model;
using KAutoPCApplicationBasic.Util;
using KAutoPCApplicationBasic.Utils;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using static PInvoke.User32;

namespace KAutoPCApplicationBasic.ViewModel
{
    public class PhoneModel
    {
        public string? StageLogs { get; set; }
        public string? AppRunning { get; set; }
        public bool IsRunning { get; set; } = false;
        public DevicesInfo Devices { get; set; } = new DevicesInfo();
        IPredictor yolo;
        public int? Index { get; set; }

        LDPlayer ld = new LDPlayer();
        public PhoneModel(DevicesInfo device) {
            this.Devices = device;
            yolo = YoloV8Predictor.Create(Config.AIModelPath,Config.LabelArray);
        }
        
        public async void Dowork()
        {
            await Task.Run(() => {
                //ld.Open_App("index", Devices.LD_Index.ToString(), "com.dots.connect.game.one");
                Task.Delay(500);
                while (true)
                {
                    Thread.Sleep(5000);
                    var matinput = Capture();
                    var resultpredict =  yolo.Predict(matinput);
                    if (resultpredict == null) {GC.Collect(); continue; }
                    if (resultpredict?.Length == 0) { GC.Collect(); continue; }
                    matinput.SaveImage("C:\\Users\\Bon\\Desktop\\Result\\Screenshot" +DateTime.Now.ToString("hhmmss")+".jpg");
                    var resultpredict2 = Sort(resultpredict);
                    if (resultpredict2.Any(x=>x.Label.Name.StartsWith("OpenTreasure")))
                    {
                        HandleClick(resultpredict2.FirstOrDefault(x => x.Label.Name.StartsWith("OpenTreasure")));
                    }
                    else if (resultpredict2.Any(x => x.Label.Name.StartsWith("GetMoney")))
                    {
                        HandleClick(resultpredict2.FirstOrDefault(x => x.Label.Name.StartsWith("GetMoney")));
                    }
                    else if (resultpredict2.Any(x => x.Label.Name.StartsWith("SkipAd")))
                    {
                        HandleClick(resultpredict2.FirstOrDefault(x => x.Label.Name.StartsWith("SkipAd")));
                    }
                    else if (resultpredict2.Any(x => x.Label.Name.StartsWith("Finish")))
                    {
                        HandleClick(resultpredict2.FirstOrDefault(x => x.Label.Name.StartsWith("Finish")));
                    }
                    else if (resultpredict2.Any(x => x.Label.Name.StartsWith("Browser")))
                    {
                        HandleClickOnPos(196,212);
                    }
                    GC.Collect();
                }
                
            });
        }
        public Mat Capture()
        {
            Mat mat = new Mat();
            try
            {
                mat = ScreenCapture.GetScreenshot(this.Devices.HandleWindow).ToMat();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return mat;           
        }
        public void HandleClick(Prediction predict)
        {
            //xPos = maxLoc.X + input.Width / 2;
            //yPos = maxLoc.Y + input.Height / 2;
            var xPos = (int)predict.Rectangle.X + (int)predict.Rectangle.Width/2;
            var yPos = ((int)predict.Rectangle.Y - 34) + (int)predict.Rectangle.Height/2;
            WindowHandlerHelper.ControlLClick(this.Devices.BindWindowHandle, new System.Drawing.Point(xPos, yPos));
        }
        public void HandleClickOnPos(int x, int y)
        {
            WindowHandlerHelper.ControlLClick(this.Devices.BindWindowHandle, new System.Drawing.Point(x, y));
        }
        public Prediction[] Sort(Prediction[] listPre)
        {
            var list = new List<Prediction>();
            var list1 = listPre.GroupBy(x => x.Label.Name);
            foreach (var item in list1)
            {
                list.Add(item.FirstOrDefault(x =>x.Score == item.Max(y=>y.Score)));
            }
            return list.ToArray();
        }
    }
}
