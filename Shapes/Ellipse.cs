using System.Numerics;
using System;
namespace MonoPaint.Shapes
{
    public struct Ellipse
    {
        public int width;
        public int height;

        int padding;

        public Ellipse(int iWidth, int iHeight)
        {
            width = iWidth;
            height = iHeight;
            padding = 0;
        }

        public void SetPadding(int iPadding)
        {
            padding = iPadding;
        }

        public int GetPadding()
        {
            return padding;
        }

        public bool IsInEllipse(int iX, int iY)
        {
            int xRadius = width / 2;
            int yRadius = height / 2;

            double resultLeft = (Math.Pow((iX) - (xRadius + padding), 2) / Math.Pow(xRadius, 2));
            double resultRight = (Math.Pow((iY) - (yRadius + padding), 2) / (Math.Pow(yRadius, 2)));
            double result = resultLeft + resultRight;

            return result <= 1;
        }

    }
}