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

        public List<aShape> Shapes{ get{ return shapes; } }

        int width, height;

        public int Width{ private set{ width = value; } get{ return width; }}
        public int Height{ private set{ height = value; } get{ return height; }}


        mRectangle Rect1;
        public mCanvas(int iWidth, int iHeight)
        {
            shapes = new List<aShape>();

            width = iWidth;
            height = iHeight;

            //TODO: Make function to draw rectangles
            Rect1 = new mRectangle(100, 150, Color.Blue);
            Rect1.X = 50; Rect1.Y = 50;
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

        bool leftClicked = false;
        public void Update()
        {
            /* TODO: Move AABB checking to shape and make class that handles shape drawing etc.
            int mX = InputManger.CurrentMouseState.X;
            int mY = InputManger.CurrentMouseState.Y;
            
            foreach(aShape s in shapes)
            {
                if(InputManger.IsPressed(MouseInput.LeftButton))
                {
                    leftClicked = true;
                }else if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton))
                {
                    if(mX > s.X && mX <  s.X + s.Width &&
                       mY > s.Y &&  mY < s.Y + s.Height)
                    {
                        s.Selected = !s.Selected;
                    }else if(!InputManger.IsKeyDown(Keys.LeftShift)){
                         s.Selected = false;
                    }
                }
            }

            if(InputManger.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
            */

            if(InputManger.IsKeyDown(Keys.Up))
            {
                Rect1.Height += 2;
            }else if(InputManger.IsKeyDown(Keys.Down))
            {
                Rect1.Height -= 2;
            }
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