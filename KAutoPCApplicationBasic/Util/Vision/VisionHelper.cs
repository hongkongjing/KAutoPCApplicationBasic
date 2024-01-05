using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAutoPCApplicationBasic.Util
{
    public class VisionHelper
    {
        const int WidthStandard = 360;
        const int HeightStandard = 640;
        //public static Mat Crop(Mat matimage)
        //{
            
        //    if (matimage == null) return new Mat();
        //    if (matimage.Height >= 34)
        //    {
        //        var xPos = 0;
        //        var yPos = matimage.Height - HeightStandard;

        //        return matimage.Clone(new Rect(xPos,yPos,WidthStandard,HeightStandard));
        //    }
        //    else if (matimage.Height < 640)
        //    {
        //        return new Mat();
        //    }
        //    return matimage;
        //}
        public static Mat Resize(Mat input)
        {
            var size = new OpenCvSharp.Size(217, 346);
            return input.Resize(size);
        }
        public static void SaveImage(Bitmap bitmap,string LDname = "")
        {
            var savepath = AppDomain.CurrentDomain.BaseDirectory + @"Screenshots";
            var mat = bitmap.ToMat();
            if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
            if (mat.Cols > 0)
            {
                var mat2 = VisionHelper.Resize(mat.Clone());
                mat2.SaveImage($"{savepath}\\Capture{LDname}{DateTime.Now.ToString("HHmmss")}.jpg");
            }
            
            
            GC.Collect();
        }
    }
}
