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
            int xRadius = width / 2;
            int yRadius = height / 2;
            
            float radius = Math.Min(xRadius, yRadius);
            float diameter = radius * 2;

            float sharpness = 1.0f;  
            
            Matrix transform = Matrix.CreateScale(xRadius / radius, yRadius / radius, 1f);
           
            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width, height, false, SurfaceFormat.Color);
            shapeData = new Color[shapeTexture.Width * shapeTexture.Height];
            Vector2 center = new Vector2(radius, radius);
            for (int colIndex = 0; colIndex < shapeTexture.Width; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < shapeTexture.Height; rowIndex++)
                {
                    Vector2 position = new Vector2(colIndex, rowIndex);
                    float distance = Vector2.Distance(center, position);

                    // hermite iterpolation
                    float x = distance / diameter;
                    float edge0 = (radius * sharpness) / (float)diameter;
                    float edge1 = radius / (float)diameter;
                    float temp = MathHelper.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
                    float result = temp * temp * (3.0f - 2.0f * temp);

                    shapeData[rowIndex * shapeTexture.Width + colIndex] = color * (1f - result);
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

        public override string ToString()
        {
            return "[Ellipse]\n" + "Width: " + width + "\nHeight: " + height + "\nColor: " + color;
        }


    }
}