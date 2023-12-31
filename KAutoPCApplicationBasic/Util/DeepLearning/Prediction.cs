using System.Drawing;

namespace KAutoPCApplicationBasic
{
    public class Prediction
    {
        public Label? Label { get; init; }
        public RectangleF Rectangle { get; init; }
        public float Score { get; init; }
    }
}
