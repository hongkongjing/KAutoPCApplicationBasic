using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace KAutoPCApplicationBasic.Util
{
    public static class Utils
    {
        public static float[] Xywh2xyxy(float[] source)
        {
            var result = new float[4];

            result[0] = source[0] - source[2] / 2f;
            result[1] = source[1] - source[3] / 2f;
            result[2] = source[0] + source[2] / 2f;
            result[3] = source[1] + source[3] / 2f;

            return result;
        }

        public static Mat ResizeImage(Mat image,int target_width,int target_height)
        {
            return image.Resize(new OpenCvSharp.Size(target_width,target_height));
        }

        public static Tensor<float> ExtractPixels(Mat mat)
        {
            // Create a tensor with the same shape as the Mat
            var tensor = new DenseTensor<float>(new[] { 1, 3, mat.Height, mat.Width });

            // Save the Mat as a byte array in memory
            using (var input = mat.Clone()) {
            for (int y = 0;y< mat.Height;y++)
                {
                    for (int x = 0;x< mat.Width; x++)
                    {
                        tensor[0, 0, y, x] = (mat.At<Vec3b>(y, x)).Item2 / 255.0F;
                        tensor[0, 1, y, x] = (mat.At<Vec3b>(y, x)).Item1 / 255.0F;
                        tensor[0, 2, y, x] = (mat.At<Vec3b>(y, x)).Item0 / 255.0F;
                    }
                }
            }
            return tensor;
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static float Sigmoid(float value)
        {
            return 1 / (1 + (float)Math.Exp(-value));
        }
    }
}
