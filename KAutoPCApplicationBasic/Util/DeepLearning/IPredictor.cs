using OpenCvSharp;
using System;
using System.Drawing;

namespace KAutoPCApplicationBasic
{
    public interface IPredictor
        : IDisposable
    {
        string? InputColumnName { get; }
        string? OutputColumnName { get; }

        int ModelInputHeight { get; }
        int ModelInputWidth { get; }

        int ModelOutputDimensions { get; }

        Prediction[] Predict(Mat img);
    }
}
