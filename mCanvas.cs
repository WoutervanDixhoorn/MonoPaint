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

        public mCanvas()
        {
            shapes = new List<aShape>();
            
            aShape Ellps1 = new mEllipse(50, 50, Color.HotPink);
            Ellps1.X = 250; Ellps1.Y = 400;
            shapes.Add(Ellps1);

            aShape Rect1 = new mRectangle(75, 100, Color.BurlyWood);
            Rect1.X = 450; Rect1.Y = 200;
            shapes.Add(Rect1);

            Console.WriteLine("Initialized canvas with: " + shapes.Count + " shapes");
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
            if(InputManger.IsPressed(MouseInput.LeftButton))
            {
                Console.WriteLine("Mouse Left pressed");
            }
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            iSpriteBatch.Begin();

            foreach(aShape shape in shapes)
            {
                shape.Draw(iSpriteBatch);
            }

            iSpriteBatch.End();
        }

    }
}