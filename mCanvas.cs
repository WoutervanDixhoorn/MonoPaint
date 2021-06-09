using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;
using MonoPaint.CompositeVisitor;

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

            //Test composite
            mRectangle rect1 = new mRectangle(30, 30, Color.Blue);
            rect1.X = 100; rect1.Y = 100;
            mRectangle rect2 = new mRectangle(40, 50, Color.Aqua);
            rect2.X = 150; rect2.Y = 100;

            ShapeComposite group = new ShapeComposite();
            group.Add(rect1); group.Add(rect2);
            shapes.Add(group);

            group.Accept(new ShapeVisitorMove(400, 100));

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