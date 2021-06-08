using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoPaint.Shapes;

namespace MonoPaint.Graphics
{
    public class mEllipseDrawer : iShapeDrawer
    {
        private static mEllipseDrawer instance = null;

        public static mEllipseDrawer Instance
        {
            get { 
                if(instance == null){
                    instance = new mEllipseDrawer();
                } 

                return instance;
            }
        }

        private mEllipseDrawer()
        {
        }

        public Color[] GetData(int width, int height, Color color)
        {
            Ellipse ellipse = new Ellipse(width, height);
            Color[] shapeData = new Color[width * height];

            for(int x = 0; x < width + ellipse.GetPadding(); x++)
            {    
                for(int y = 0; y < height + ellipse.GetPadding(); y++)
                {
                    if(ellipse.IsInEllipse(x, y))
                    {
                        shapeData[y * width + x] = color; 
                    }else{
                        shapeData[y * width + x] = Color.Transparent;
                    }
                }   
            }

            return shapeData;
        }

        public Color[] GetBorderData(int width, int height, int borderSize, Color color)
        {
            int borderWidth = width+(borderSize*2), borderHeight = height+(borderSize*2);
            Color[] borderData = new Color[borderWidth * borderHeight];

            Ellipse borderEllipse = new Ellipse(borderWidth, borderHeight);
            Ellipse maskEllipse = new Ellipse(width, height);
            maskEllipse.SetPadding(borderSize);

            for(int x = 0; x < borderWidth; x++)
            {    
                for(int y = 0; y < borderHeight; y++)
                {
                    if(borderEllipse.IsInEllipse(x,y) && !maskEllipse.IsInEllipse(x,y))
                    {
                        borderData[y * borderWidth + x] = color; 
                    }else{
                        borderData[y * borderWidth + x] = Color.Transparent;
                    }
                }   
            }

            return borderData;
        }
    }
}