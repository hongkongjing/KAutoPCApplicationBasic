using KAutoPCApplicationBasic.Model;
using KAutoPCApplicationBasic.Util;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ld.Open_App("index", Devices.LD_Index.ToString(), "com.dots.connect.game.one");
                Task.Delay(500);
                while (true)
                {
                    Task.Delay(2000);
                    var imageCapture = ADBHelper.ScreenShoot(Devices.Adb_id_Name, true);
                    if (imageCapture != null)
                    {
                        var resultpredict =  yolo.Predict(imageCapture.ToMat().Clone());
                        if (resultpredict != null)
                        {
                            if (resultpredict.Any(x => x.Label.Name == "Finish")||resultpredict.Any(x=>x.Label.Name== "SkipAd"))
                            {
                                var finalResult = resultpredict.Where(m=>m.Label.Name is "Finish" or "SkipAd").First(x => x.Score == resultpredict.Max(y => y.Score));
                                ADBHelper.Tap(Devices.Adb_id_Name,(int)finalResult.Rectangle.X + (int)finalResult.Rectangle.Width/2, (int)finalResult.Rectangle.Y + (int)finalResult.Rectangle.Height / 2);
                            }
                        }
                        
                        
                    }
                }
            });
        }

    }
}
