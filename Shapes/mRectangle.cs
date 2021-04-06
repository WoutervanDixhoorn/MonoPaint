using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint
{
    public class mRectangle : aShape
    {
        public mRectangle(int iWidth, int iHeight, Color? iColor = null)
        {
            if(iWidth <= 0 || iHeight <= 0)
            {
                throw new NotSupportedException("Cant draw shapes with a width or height lower then 1 yet");
            }

            width = iWidth;
            height = iHeight;
            position = new Vector2(0, 0);
            color = iColor ?? Color.HotPink;
        }
        
        public override void Load()
        {
            shapeData = new Color[(width*height)];

            for(int i = 0; i < shapeData.Length; i++)
                shapeData[i] = color;

            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width, height); 
            shapeTexture.SetData(shapeData);
        }

        public override void Reload()
        {
            Unload();

            Load();
        }
        public override void Unload()
        {
            shapeData = null;

            if(shapeTexture != null)
                shapeTexture.Dispose();
        }

        public override string ToString()
        {
            return "[Rect]\n" + "Width: " + width + "\nHeight: " + height + "\nColor: " + color;
        }

    }

}