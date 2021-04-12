using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Commands;
using MonoPaint.Shapes;

namespace MonoPaint.ToolStrategy
{

    public class ShapeTransformTool : IShapeTool
    {
        mPlayground playground;

        aShape transformingShape;
        SelectionRectangle rect;

        public ShapeTransformTool(mPlayground iPlayground)
        {
            playground = iPlayground;
        }

        bool leftClicked = false;

        int startX, startY, endX, endY;
        public void Update()
        {   
            int mX = InputManger.CurrentMouseState.X;
            int mY = InputManger.CurrentMouseState.Y;

            if(transformingShape != null){
                rect = transformingShape.SelectionRect;
            }

            ToggleHover();

            if(!leftClicked && InputManger.IsPressed(MouseInput.LeftButton))
            {
                SelectShapes();
            }else if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton) && transformingShape != null)
            {
                if(rect.BottomRight.Selected)
                {
                    rect.BottomRight.Selected = false;

                    int xDifShape = endX - transformingShape.X; 
                    int yDifShape = endY - transformingShape.Y;

                    int newWidth = Util.Clamp(xDifShape - rect.Padding, 1, playground.Width);
                    int newHeight = Util.Clamp(yDifShape - rect.Padding, 1, playground.Width);

                    //Create and execute command
                    playground.ExecuteCommand(new ResizeCommand(transformingShape, newWidth, newHeight));     
                }

            }else if(leftClicked && transformingShape != null){
                    ChangeWithBottomLeft(mX, mY);
            }

            if(InputManger.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManger.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
        }

        public void Reset()
        {
            if(transformingShape != null)
                transformingShape.Transforming = false;

            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    shape.Hovered = false;
                });
            }
        }

        void ToggleHover()
        {
            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    if(shape.Contains(InputManger.CurrentMouseState.X, InputManger.CurrentMouseState.Y)){
                        shape.Hovered = true;
                    }else{
                        shape.Hovered = false;
                    }
                });
            }
        }

        void SelectShapes()
        {
            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    if(shape.Hovered)
                    {        
                        if(transformingShape != null)
                            transformingShape.Transforming = false;                    
                        transformingShape = shape;
                        transformingShape.Transforming = true;
                    }
                });
            }
        }

        void ChangeWithBottomLeft(int mX, int mY)
        {
            if(rect.BottomRight.Contains(mX, mY) || rect.BottomRight.Selected)
            {
                Console.WriteLine("-Change Size-");

                rect.BottomRight.Selected = true;

                endX = mX; 
                endY = mY;

                int xDif = endX - rect.SelectRect.X; 
                int yDif = endY - rect.SelectRect.Y;

                //Update selection rect
                rect.SelectRect.Width = Util.Clamp(xDif, 1, playground.Width);;
                rect.SelectRect.Height = Util.Clamp(yDif, 1, playground.Width);;
                rect.SelectRect.Load();

                //Update bottomRight
                rect.BottomRight.X = endX - (rect.BottomRight.Width/2);
                rect.BottomRight.Y = endY - (rect.BottomRight.Height/2);

                //Update bottomLeft
                rect.BottomLeft.Y = endY - (rect.BottomLeft.Height/2);

                //Update topRight
                rect.TopRight.X = endX - (rect.TopRight.Width/2);
                
            }
        }

    }
}