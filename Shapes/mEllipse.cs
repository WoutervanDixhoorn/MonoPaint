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

            if(transforming)
            {
                generateTransformRect();
            }
        }

        public override void LoadWhileDrawing()
        {
            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width + 1 + Padding, height + 1 + Padding, false, SurfaceFormat.Color);
            shapeData = new Color[shapeTexture.Width * shapeTexture.Height];

            /*Algorithm from: http://members.chello.at/~easyfilter/bresenham.html*/
            int x0 = 0, y0 = height, x1 = width, y1 = 0;

            int a = Math.Abs(x1-x0), b = Math.Abs(y1-y0), b1 = b&1; /* values of diameter */
            long dx = 4*(1-a)*b*b, dy = 4*(b1+1)*a*a; /* error increment */
            long err = dx+dy+b1*a*a, e2; /* error of 1.step */

            if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
            if (y0 > y1) y0 = y1; /* .. exchange them */
            y0 += (b+1)/2; y1 = y0-b1;   /* starting pixel */
            a *= 8*a; b1 = 8*b*b;

            do {
                shapeData[y0 * shapeTexture.Width + x1] = Color.Black; /*   I. Quadrant */
                shapeData[y0 * shapeTexture.Width + x0] = Color.Black; /*  II. Quadrant */
                shapeData[y1 * shapeTexture.Width + x0] = Color.Black; /* III. Quadrant */
                shapeData[y1 * shapeTexture.Width + x1] = Color.Black; /*  IV. Quadrant */

                e2 = 2*err;
                if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */ 
                if (e2 >= dx || 2*err > dy) { x0++; x1--; err += dx += b1; } /* x step */
            } while (x0 <= x1);
            
            while (y0-y1 < b) {  /* too early stop of flat ellipses a=1 */
                shapeData[y0 * shapeTexture.Width + (x0-1)] = Color.Black; /*   I. Quadrant */
                shapeData[y0++ * shapeTexture.Width + (x1+1)] = Color.Black; /*  II. Quadrant */
                shapeData[y1 * shapeTexture.Width + (x0-1)] = Color.Black; /* III. Quadrant */
                shapeData[y1-- * shapeTexture.Width + (x1+1)] = Color.Black; /*  IV. Quadrant */
            }

            shapeTexture.SetData(shapeData);
        }

        public override bool Contains(int iX, int iY)
        {
                int normX = iX - X;
                int normY = iY - Y;
                if(ellipse.IsInEllipse(normX, normY))
                    return true;
            
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