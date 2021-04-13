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

        bool transforming;

        public ShapeTransformTool(mPlayground iPlayground)
        {
            playground = iPlayground;
            transforming = false;
        }

        bool leftClicked = false;

        int startX, startY, endX, endY;
        public void Update()
        {   
            int mX = InputManger.CurrentMouseState.X;
            int mY = InputManger.CurrentMouseState.Y;

            SelectShapes();

            if(transformingShape != null){
                rect = transformingShape.SelectionRect;
            }


            if(!leftClicked && InputManger.IsPressed(MouseInput.LeftButton))
            {
                
            }else if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton) && transformingShape != null)
            {
                UpdateShapes();
            }else if(leftClicked && transformingShape != null){
                ChangeWithBottomLeft(mX, mY);
                ChangeWithBottomRight(mX, mY);
                ChangeWithTopLeft(mX, mY);
                ChangeWithTopRight(mX, mY);
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

        void SelectShapes()
        {
            int mX = InputManger.CurrentMouseState.X;
            int mY = InputManger.CurrentMouseState.Y;

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

            if(!leftClicked && InputManger.IsPressed(MouseInput.LeftButton)){
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
        }

        void UpdateShapes()
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
                }else if(rect.BottomLeft.Selected){
                    rect.BottomLeft.Selected = false;

                    int xDifShape = -(endX - transformingShape.X); 
                    int yDifShape = endY - (transformingShape.Y);

                    int newWidth = Util.Clamp((xDifShape + transformingShape.Width) - rect.Padding, 1, playground.Width);
                    int newHeight = Util.Clamp(yDifShape - rect.Padding, 1, playground.Width);

                    int newX = transformingShape.X - (xDifShape - rect.Padding);
                    int newY = transformingShape.Y;

                    //Create and execute command
                    playground.ExecuteCommand(new ResizeCommand(transformingShape, newWidth, newHeight, newX, newY));     
                }else if(rect.TopLeft.Selected){
                    rect.TopLeft.Selected = false;

                    int xDifShape = -(endX - transformingShape.X); 
                    int yDifShape = -(endY - transformingShape.Y);

                    int newWidth = Util.Clamp((xDifShape + transformingShape.Width) - rect.Padding, 1, playground.Width);
                    int newHeight = Util.Clamp((yDifShape + transformingShape.Height) - rect.Padding, 1, playground.Width);

                    //TODO: Move x and y to command!!
                    int newX = transformingShape.X - (xDifShape - rect.Padding);
                    int newY = transformingShape.Y - (yDifShape - rect.Padding);

                    //Create and execute command
                    playground.ExecuteCommand(new ResizeCommand(transformingShape, newWidth, newHeight, newX, newY));     
                }else if(rect.TopRight.Selected){
                    rect.TopRight.Selected = false;

                    int xDifShape = endX - transformingShape.X; 
                    int yDifShape = -(endY - transformingShape.Y);

                    int newWidth = Util.Clamp(xDifShape - rect.Padding, 1, playground.Width);
                    int newHeight = Util.Clamp((yDifShape + transformingShape.Height) - rect.Padding, 1, playground.Width);

                    int newX = transformingShape.X;
                    int newY = transformingShape.Y - (yDifShape - rect.Padding);

                    //Create and execute command
                    playground.ExecuteCommand(new ResizeCommand(transformingShape, newWidth, newHeight, newX, newY));     
                }
        }

        void ChangeWithTopLeft(int mX, int mY)
        {     
            if(rect.TopLeft.Contains(mX, mY) || rect.TopLeft.Selected)
            {
                rect.TopLeft.Selected = true;

                endX = mX; 
                endY = mY;

                int newWidth = (rect.SelectRect.X - endX) + rect.SelectRect.Width; 
                int newHeight = (rect.SelectRect.Y - endY) + rect.SelectRect.Height;

                //Update selection rect
                rect.SelectRect.X = endX;
                rect.SelectRect.Y = endY;

                rect.SelectRect.Width = Util.Clamp(newWidth, 1, playground.Width);;
                rect.SelectRect.Height = Util.Clamp(newHeight, 1, playground.Height);;
                rect.SelectRect.Load();

                //Update topLeft
                rect.TopLeft.X = endX - (rect.TopLeft.Width/2);
                rect.TopLeft.Y = endY - (rect.TopLeft.Height/2);

                //Update bottomLeft
                rect.BottomLeft.X = endX - (rect.BottomLeft.Height/2);

                //Update topRight
                rect.TopRight.Y = endY - (rect.TopLeft.Height/2);
            }
        }
        void ChangeWithTopRight(int mX, int mY)
        {
             if(rect.TopRight.Contains(mX, mY) || rect.TopRight.Selected)
            {
                rect.TopRight.Selected = true;

                endX = mX; 
                endY = mY;

                int newWidth = endX - rect.SelectRect.X;  
                int newHeight = (rect.SelectRect.Y - endY) + rect.SelectRect.Height;

                //Update selection rect
                rect.SelectRect.Y = endY;

                rect.SelectRect.Width = Util.Clamp(newWidth, 1, playground.Width);;
                rect.SelectRect.Height = Util.Clamp(newHeight, 1, playground.Height);;
                rect.SelectRect.Load();

                //Update topRight
                rect.TopRight.X = endX - (rect.TopLeft.Width/2);
                rect.TopRight.Y = endY - (rect.TopLeft.Height/2);

                //Update bottomLeft
                rect.BottomRight.X = endX - (rect.BottomLeft.Height/2);

                //Update topLeft
                rect.TopLeft.Y = endY - (rect.TopLeft.Height/2);
            }
        }
        void ChangeWithBottomRight(int mX, int mY)
        {
            if(rect.BottomRight.Contains(mX, mY) || rect.BottomRight.Selected)
            {
                rect.BottomRight.Selected = true;

                endX = mX; 
                endY = mY;

                int xDif = endX - rect.SelectRect.X; 
                int yDif = endY - rect.SelectRect.Y;

                //Update selection rect
                rect.SelectRect.Width = Util.Clamp(xDif, 1, playground.Width);;
                rect.SelectRect.Height = Util.Clamp(yDif, 1, playground.Height);;
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
        void ChangeWithBottomLeft(int mX, int mY)
        {
            if(rect.BottomLeft.Contains(mX, mY) || rect.BottomLeft.Selected)
            {
                Console.WriteLine("-Change Size-");

                rect.BottomLeft.Selected = true;

                endX = mX; 
                endY = mY;

                int xDif = (rect.SelectRect.X - endX) + rect.SelectRect.Width; 
                int yDif = endY - rect.SelectRect.Y;

                //Update selection rect
                rect.SelectRect.X = endX;

                rect.SelectRect.Width = Util.Clamp(xDif, 1, playground.Width);
                rect.SelectRect.Height = Util.Clamp(yDif, 1, playground.Height);
                rect.SelectRect.Load();

                //Update bottomLeft
                rect.BottomLeft.X = endX - (rect.BottomRight.Width/2);
                rect.BottomLeft.Y = endY - (rect.BottomRight.Height/2);

                //Update bottomRight
                rect.BottomRight.Y = endY - (rect.BottomLeft.Height/2);

                //Update topRight
                rect.TopLeft.X = endX - (rect.TopRight.Width/2);
            }
        }
    }
}