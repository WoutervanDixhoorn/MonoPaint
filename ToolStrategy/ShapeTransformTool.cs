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
        aShape drawingShape;
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
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            if(!transforming)
                HoverShapes();

            SelectShapes();

            if(transformingShape != null){
                rect = transformingShape.SelectionRect;
            }


            if(leftClicked && InputManager.IsReleased(MouseInput.LeftButton) && transformingShape != null)
            {
                UpdateShapes();
            }else if(leftClicked && transformingShape != null){
                ChangeWithBottomLeft(mX, mY);
                ChangeWithBottomRight(mX, mY);
                ChangeWithTopLeft(mX, mY);
                ChangeWithTopRight(mX, mY);
            }

            if(InputManager.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManager.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            if(drawingShape != null)
                drawingShape.Draw(iSpriteBatch);
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

        void HoverShapes()
        {
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    if(shape.Contains(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y)){
                        shape.Hovered = true;
                    }else{
                        shape.Hovered = false;
                    }
                });
            }
        }

        void SelectShapes()
        {
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            if(!leftClicked && InputManager.IsPressed(MouseInput.LeftButton)){
                foreach(mCanvas c in playground.Canvases)
                {
                    c.ForAllShapes((aShape shape) => {
                        if(shape.Hovered)
                        {    
                            transforming = true;

                            if(transformingShape != null)
                                transformingShape.Transforming = false;                    
                            transformingShape = shape;
                            transformingShape.Transforming = true;

                            //Draw new size
                            if(shape.ShapeName == "ellipse")
                            {
                                drawingShape = new mEllipse(transformingShape.Width, transformingShape.Height, transformingShape.Color);
                            }else{
                                drawingShape = new mRectangle(transformingShape.Width, transformingShape.Height, transformingShape.Color);    
                            }
                            
                            drawingShape.X = transformingShape.X;
                            drawingShape.Y = transformingShape.Y;
                            drawingShape.LoadWhileDrawing();
                        }
                    });
                }
            }

            //Quick fix for resizing shapes ontop or under shapes
            if(leftClicked && transforming)
            {
                bool overTransformingShape = transformingShape.Contains(mX, mY);

                if(!overTransformingShape)
                    transforming = false;
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
                int newHeight = Util.Clamp(yDifShape - rect.Padding, 1, playground.Height);

                //Create and execute command
                playground.ExecuteCommand(new ResizeCommand(transformingShape, newWidth, newHeight));     
            }else if(rect.BottomLeft.Selected){
                rect.BottomLeft.Selected = false;

                int xDifShape = -(endX - transformingShape.X); 
                int yDifShape = endY - (transformingShape.Y);

                int newWidth = Util.Clamp((xDifShape + transformingShape.Width) - rect.Padding, 1, playground.Width);
                int newHeight = Util.Clamp(yDifShape - rect.Padding, 1, playground.Height);

                int newX = transformingShape.X - (xDifShape - rect.Padding);
                int newY = transformingShape.Y;

                //Create and execute command
                playground.ExecuteCommand(new ResizeCommand(transformingShape, newWidth, newHeight, newX, newY));     
            }else if(rect.TopLeft.Selected){
                rect.TopLeft.Selected = false;

                int xDifShape = -(endX - transformingShape.X); 
                int yDifShape = -(endY - transformingShape.Y);

                int newWidth = Util.Clamp((xDifShape + transformingShape.Width) - rect.Padding, 1, playground.Width);
                int newHeight = Util.Clamp((yDifShape + transformingShape.Height) - rect.Padding, 1, playground.Height);

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
                int newHeight = Util.Clamp((yDifShape + transformingShape.Height) - rect.Padding, 1, playground.Height);

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

                #region Update Helper Rect
                int xDifShape = -(endX - transformingShape.X); 
                int yDifShape = -(endY - transformingShape.Y);
                int drawWidth = Util.Clamp((xDifShape + transformingShape.Width) - rect.Padding, 1, playground.Width);
                int drawHeight = Util.Clamp((yDifShape + transformingShape.Height) - rect.Padding, 1, playground.Height);
                int newX = transformingShape.X - (xDifShape - rect.Padding);
                int newY = transformingShape.Y - (yDifShape - rect.Padding);

                updateDrawingShape(newX, newY, drawWidth, drawHeight);
                #endregion
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
                
                #region Update Helper Rect
                int xDifShape = endX - transformingShape.X; 
                int yDifShape = -(endY - transformingShape.Y);
                int drawWidth = Util.Clamp(xDifShape - rect.Padding, 1, playground.Width);
                int drawHeight = Util.Clamp((yDifShape + transformingShape.Height) - rect.Padding, 1, playground.Height);
                int newX = transformingShape.X;
                int newY = transformingShape.Y - (yDifShape - rect.Padding);

                updateDrawingShape(newX, newY, drawWidth, drawHeight);
                #endregion
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
                
                #region Update Helper Rect
                int xDifShape = endX - transformingShape.X; 
                int yDifShape = endY - transformingShape.Y;
                int newWidth = Util.Clamp(xDifShape - rect.Padding, 1, playground.Width);
                int newHeight = Util.Clamp(yDifShape - rect.Padding, 1, playground.Height);

                updateDrawingShape(transformingShape.X, transformingShape.Y, newWidth, newHeight);
                #endregion
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
                
                #region Update Helper Rect
                int xDifShape = -(endX - transformingShape.X); 
                int yDifShape = endY - (transformingShape.Y);
                int newWidth = Util.Clamp((xDifShape + transformingShape.Width) - rect.Padding, 1, playground.Width);
                int newHeight = Util.Clamp(yDifShape - rect.Padding, 1, playground.Height);
                int newX = transformingShape.X - (xDifShape - rect.Padding);
                int newY = transformingShape.Y;

                updateDrawingShape(newX, newY, newWidth, newHeight);
                #endregion
            }
        }

        void updateDrawingShape(int iX, int iY, int iWidth, int iHeight)
        {
            drawingShape.X = iX;
            drawingShape.Y = iY;
            drawingShape.Width = iWidth;
            drawingShape.Height = iHeight; 
            drawingShape.LoadWhileDrawing();
        }
    }
}