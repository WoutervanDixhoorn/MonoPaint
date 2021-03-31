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
            shapeData = new Color[(width*height)];
/*
            const double PI = 3.1415926535;
            double i, angle, x1, y1;

            for(i = 0; i < 360; i += 0.1)
            {
                    angle = i;
                    x1 = (width/2) * Math.Cos(angle * PI / 180);
                    y1 = (height/2) * Math.Sin(angle * PI / 180);
                    shapeData(x + x1, y + y1, color);
            }
*/

            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width, height); 
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

        public override void Draw(SpriteBatch spriteBatch, float iAlpha = 1)
        {
            spriteBatch.Draw(shapeTexture, position, Color.White * iAlpha);          
        }

        public override string ToString()
        {
            return "[Ellipse]\n" + "Width: " + width + "\nHeight: " + height + "\nColor: " + color;
        }


    }
}