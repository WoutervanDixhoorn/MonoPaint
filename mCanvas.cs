using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;

namespace MonoPaint
{
    public class mCanvas
    {   
        List<aShape> shapes;
        public List<aShape> Shapes{ get{ return shapes; }
                                    set{ shapes = value; } }

        int width, height;
        public int Width{ private set{ width = value; } get{ return width; }}
        public int Height{ private set{ height = value; } get{ return height; }}

        public mCanvas(int iWidth, int iHeight)
        {
            shapes = new List<aShape>();

            width = iWidth;
            height = iHeight;

            mEllipse ellipse = new mEllipse(100, 100, Color.Blue);
            ellipse.X = 150; ellipse.Y = 200;
            ellipse.Padding = 10;
            shapes.Add(ellipse);

            Console.WriteLine("Initialized canvas with: " + shapes.Count + " shapes");
        }

        public void ForAllShapes(Action<aShape> iShapeFunction)
        {
            foreach(aShape shape in Shapes)
            {
                iShapeFunction(shape);
            }
        }

        public void ForAllShapes(Action<aShape, SpriteBatch> iShapeFunction, SpriteBatch iSpriteBatch)
        {
            foreach(aShape shape in Shapes)
            {
                iShapeFunction(shape, iSpriteBatch);
            }
        }

        public void Load()
        {
            foreach(aShape shape in shapes)
            {
                shape.Load();
            }
        }

        public void Unload()
        {
            foreach(aShape shape in shapes)
            {
                shape.Unload();
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            foreach(aShape shape in shapes)
            {
                shape.Draw(iSpriteBatch);
            }
        }
    }

}