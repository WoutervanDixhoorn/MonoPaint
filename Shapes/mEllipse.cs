using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint.Shapes
{
    public class mEllipse : aShape
    {
        public mEllipse(int iWidth, int iHeight, Color? iColor = null)
        {
            width = iWidth;
            height = iHeight;
            position = new Vector2(0, 0);
            color = iColor ?? Color.HotPink;
        }

        public override void Load()
        { 
            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width + 1, height + 1, false, SurfaceFormat.Color);
            shapeData = new Color[shapeTexture.Width * shapeTexture.Height];

            for(int x = 0; x < width; x++)
            {    
                for(int y = 0; y < height; y++)
                {
                    if(IsInEllipse(x, y))
                        shapeData[y * shapeTexture.Width + x] = color; 
                }   
            }

            shapeTexture.SetData(shapeData);
        }

        public override void Reload()
        {
            Unload();

            shapeData = new Color[(width*height)];

            for(int i = 0; i < shapeData.Length; i++)
                shapeData[i] = color;

            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width, height); 
            shapeTexture.SetData(shapeData);
        }
        public override void Unload()
        {
            shapeTexture.Dispose();
        }

        bool IsInEllipse(int iX, int iY)
        {
            int xRadius = width / 2;
            int yRadius = height / 2;

            double resultLeft = (Math.Pow(iX - xRadius, 2) / (Math.Pow(xRadius, 2)));
            double resultRight = (Math.Pow(iY - yRadius, 2) / (Math.Pow(yRadius, 2)));
            double result = resultLeft + resultRight;

            return result <= 1;
        }

        public override string ToString()
        {
            return "[Ellipse]\n" + "Width: " + width + "\nHeight: " + height + "\nColor: " + color;
        }


    }
}