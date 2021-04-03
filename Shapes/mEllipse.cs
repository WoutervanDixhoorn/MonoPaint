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

            float diam = width / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    int index = x * width + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        shapeData[index] = Color.White;
                    }
                    else
                    {
                        shapeData[index] = Color.Transparent;
                    }
                }
            }

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

        public override string ToString()
        {
            return "[Ellipse]\n" + "Width: " + width + "\nHeight: " + height + "\nColor: " + color;
        }


    }
}