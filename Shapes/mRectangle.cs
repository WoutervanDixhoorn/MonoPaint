using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint
{
    public class mRectangle : aShape
    {
        public mRectangle(int iWidth, int iHeight, Color? iColor = null) : 
        base(iWidth, iHeight, iColor)
        {
        }

        public override bool Contains(int iX, int iY)
        {
            if(iX > X && iX <  X + Width &&
               iY > Y &&  iY < Y + Height)
            {
                return true;
            }
            return false;
        }

        public override void Load()
        {
            shapeData = new Color[(width*height)];

            for(int i = 0; i < shapeData.Length; i++)
                shapeData[i] = color;

            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width, height); 
            shapeTexture.SetData(shapeData);

            //Load border
            if(drawBorder){
                int borderWidth = width+(borderSize*2), borderHeight = height+(borderSize*2);
                borderData = new Color[((borderWidth)*(borderHeight))];

                for(int i = 0; i < borderHeight; i++){
                    for(int j = 0; j < borderWidth; j++){
                        if(j >= i*borderWidth - borderSize || j < borderSize){
                            borderData[i*borderWidth+j] = borderColor;
                        }else if(j >= borderWidth - borderSize){
                            borderData[i*borderWidth+j] = borderColor;
                        }else if(i >= borderHeight - borderSize || i < borderSize){
                            borderData[i*borderWidth+j] = borderColor;
                        }                 
                    }
                }
                
                borderTexture = new Texture2D(ContentHandler.Instance.Graphics, borderWidth, borderHeight); 
                borderTexture.SetData(borderData);
            }

            if(transforming)
            {
                generateTransformRect();
            }
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