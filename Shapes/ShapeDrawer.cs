using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;

namespace MonoPaint.Shapes
{
    public class ShapeDrawer
    {
        enum eShape{
            Rect,
            Ellps
        }

        eShape curShape = eShape.Rect;
        aShape currentShape;

        mPlayground playground;

        public ShapeDrawer(mPlayground iPlayground)
        {
            playground = iPlayground;
            curShape = eShape.Rect;
            currentShape = null;
        }

        public aShape GetCurrentShape()
        {
            return currentShape;
        }

        bool leftClicked = false;
        int xPos1, yPos1, xPos2, yPos2 = 0;
        int rWidth, rHeight;
        public void UpdateInput()
        {     
            
            //TODO: Move to playgound where you can select other tools aswell
            if(InputManger.IsKeyPressed(Keys.Q))
            {
                if(curShape == eShape.Rect)
                {
                    curShape = eShape.Ellps;
                }else{
                    curShape = eShape.Rect;
                }
            }            

            if(!leftClicked && InputManger.IsPressed(MouseInput.LeftButton))
            {
                if(curShape == eShape.Rect)
                    currentShape = new mRectangle(1,1, Color.Red);

                if(curShape == eShape.Ellps)
                    currentShape = new mEllipse(1,1, Color.Red);

                xPos1 = InputManger.CurrentMouseState.X;
                yPos1 = InputManger.CurrentMouseState.Y;
                leftClicked = true;

                currentShape.X = xPos1; currentShape.Y = yPos1;
                currentShape.Load();

                playground.AddShape(currentShape);

                Console.WriteLine("Start drawing at: [" + xPos1 + " | " + yPos1 + "]");
            }else if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton))
            {
                leftClicked = false;

                currentShape.Width = Util.Clamp(rWidth, 1, playground.Width);
                currentShape.Height = Util.Clamp(rHeight, 1, playground.Height);

                currentShape.Load();
                Console.WriteLine("Added: " + currentShape.ToString());
            }else if(leftClicked)
            {
                xPos2 = InputManger.CurrentMouseState.X;
                yPos2 = InputManger.CurrentMouseState.Y;

                rWidth = xPos2 - xPos1; 
                rHeight = yPos2 - yPos1;
            
                currentShape.Width = Util.Clamp(rWidth, 1, playground.Width);
                currentShape.Height = Util.Clamp(rHeight, 1, playground.Height);

                currentShape.Load();
            }

        }

    }
}