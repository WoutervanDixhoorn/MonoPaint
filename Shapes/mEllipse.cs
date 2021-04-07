using System.Threading;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint.Shapes
{
    public class mEllipse : aShape
    {

        Ellipse ellipse;

        Ellipse borderEllipse;
        Ellipse maskEllipse;

        public int Padding
        {
            get{ return ellipse.GetPadding(); }
            set{ ellipse.SetPadding(value); }
        }

        public mEllipse(int iWidth, int iHeight, Color? iColor = null) : 
        base(iWidth, iHeight, iColor)
        {
            ellipse = new Ellipse(iWidth, iHeight);
        }

        public override void Load()
        { 
            ellipse = new Ellipse(width, height);

            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width + 1 + Padding, height + 1 + Padding, false, SurfaceFormat.Color);
            shapeData = new Color[shapeTexture.Width * shapeTexture.Height];

            for(int x = 0; x < width + Padding; x++)
            {    
                for(int y = 0; y < height + Padding; y++)
                {
                    if(ellipse.IsInEllipse(x, y))
                    {
                        shapeData[y * shapeTexture.Width + x] = color; 
                    }else{
                        shapeData[y * shapeTexture.Width + x] = Color.Transparent;
                    }
                }   
            }

            shapeTexture.SetData(shapeData); 

            if(drawBorder){
                int borderWidth = width+(borderSize*2), borderHeight = height+(borderSize*2);
                borderTexture = new Texture2D(ContentHandler.Instance.Graphics, borderWidth, borderHeight, false, SurfaceFormat.Color);
                borderData = new Color[borderTexture.Width * borderTexture.Height];

                borderEllipse = new Ellipse(borderWidth, borderHeight);
                maskEllipse = new Ellipse(width, height);
                maskEllipse.SetPadding(borderSize);

                for(int x = 0; x < borderWidth; x++)
                {    
                    for(int y = 0; y < borderHeight; y++)
                    {
                        if(borderEllipse.IsInEllipse(x,y) && !maskEllipse.IsInEllipse(x,y))
                        {
                            borderData[y * borderTexture.Width + x] = borderColor; 
                        }else{
                            borderData[y * borderTexture.Width + x] = Color.Transparent;
                        }
                    }   
                }

                borderTexture.SetData(borderData);
            }

        }

        public Tuple<Ellipse, Ellipse> GetBorderEllipses()
        {
            return Tuple.Create<Ellipse, Ellipse>(borderEllipse, maskEllipse);
        }

        public override bool Contains(int iX, int iY)
        {
            //if(iX > X && iX <  X + Width &&
            //   iY > Y &&  iY < Y + Height)
            //{
                //TODO: Normalize iX and iY and check ellipse 
                int normX = iX - X;
                int normY = iY - Y;
                if(ellipse.IsInEllipse(normX, normY))
                    return true;
            //}
            return false;
        }

        public override void Unload()
        {
            if(shapeTexture != null)
                shapeTexture.Dispose();
        }

        public override string ToString()
        {
            return "[Ellipse]\n" + "Width: " + width + "\nHeight: " + height + "\nColor: " + color;
        }


    }
}