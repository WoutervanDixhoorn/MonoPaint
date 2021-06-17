using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Commands;
using MonoPaint.Shapes;

namespace MonoPaint.ToolStrategy
{
    public class ShapeDrawTool : IShapeTool
    {

        mPlayground playground;

        enum eShape{
            Rect,
            Ellps
        }

        eShape curShape = eShape.Rect;
        aShape currentShape;

        bool drawing;

        public ShapeDrawTool(mPlayground iPlayground)
        {
            playground = iPlayground;
            curShape = eShape.Rect;
            currentShape = null;

            drawing = false;
        }

        bool leftClicked = false;
        int xPos1, yPos1, xPos2, yPos2 = 0;
        int rWidth, rHeight;
        public void Update()
        {

            if(InputManager.IsKeyPressed(Keys.Q))
            {
                if(curShape == eShape.Rect)
                {
                    curShape = eShape.Ellps;
                }else{
                    curShape = eShape.Rect;
                }
            }            

            if(!leftClicked && InputManager.IsPressed(MouseInput.LeftButton))
            {   
                if(curShape == eShape.Rect)
                    currentShape = new mRectangle(1,1, Color.Red);

                if(curShape == eShape.Ellps)
                    currentShape = new mEllipse(1,1, Color.Red);

                xPos1 = InputManager.CurrentMouseState.X;
                yPos1 = InputManager.CurrentMouseState.Y;

                currentShape.X = xPos1; currentShape.Y = yPos1;
                currentShape.LoadWhileDrawing();

                drawing = true;

                Console.WriteLine("Start drawing at: [" + xPos1 + " | " + yPos1 + "]");
            }else if(leftClicked && InputManager.IsReleased(MouseInput.LeftButton))
            {
                currentShape.Width = Util.Clamp(rWidth, 1, playground.Width);
                currentShape.Height = Util.Clamp(rHeight, 1, playground.Height);

                currentShape.Load();
                playground.ExecuteCommand(new DrawCommand(currentShape, playground));

                drawing = false;

                Console.WriteLine("Added: " + currentShape.ToString());
            }else if(leftClicked)
            {
                xPos2 = InputManager.CurrentMouseState.X;
                yPos2 = InputManager.CurrentMouseState.Y;

                rWidth = xPos2 - xPos1; 
                rHeight = yPos2 - yPos1;
            
                currentShape.Width = Util.Clamp(rWidth, 1, playground.Width);
                currentShape.Height = Util.Clamp(rHeight, 1, playground.Height);

                currentShape.LoadWhileDrawing();
            }

            if(InputManager.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManager.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            if(currentShape != null && drawing)
                currentShape.Draw(iSpriteBatch);
        }

        public void Reset()
        {
            /*Reset shapes if needed*/
        }

    }
}