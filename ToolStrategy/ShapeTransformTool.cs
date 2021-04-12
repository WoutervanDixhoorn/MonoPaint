using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;

namespace MonoPaint.ToolStrategy
{

    //TODO: Maybe give each shape there own SlectionRect object;

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
            }else if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton) && transformingShape != null)
            {
                if(rect.BottomRight.Selected)
                {
                    rect.BottomRight.Selected = false;
                }
            }else if(leftClicked && transformingShape != null){
                    ChangeWidthBottomLeft(mX, mY);
            }

            if(InputManger.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManger.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }

        public void Reset()
        {
            if(transformingShape != null)
                transformingShape.Transforming = false;

            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    shape.Selected = false;
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
        
        void ChangeWidthBottomLeft(int mX, int mY)
        {
            if(rect.BottomRight.Contains(mX, mY) || rect.BottomRight.Selected)
            {
                Console.WriteLine("-Change Size-");

                rect.BottomRight.Selected = true;

                endX = mX; 
                endY = mY;

                int xDif = endX - rect.SelectRect.X; 
                int yDif = endY - rect.SelectRect.Y;

                //Update selectionRectangle
                {
                //Update selection rect
                rect.SelectRect.Width = xDif;
                rect.SelectRect.Height = yDif;
                rect.SelectRect.Load();

                //Update bottomRight
                rect.BottomRight.X = endX - (rect.BottomRight.Width/2);
                rect.BottomRight.Y = endY - (rect.BottomRight.Height/2);

                //Update bottomLeft
                rect.BottomLeft.Y = endY - (rect.BottomLeft.Height/2);

                //Update topRight
                rect.TopRight.X = endX - (rect.TopRight.Width/2);
                }
                
                //Update transformingShape;
                int xDifShape = endX - transformingShape.X; 
                int yDifShape = endY - transformingShape.Y;

                transformingShape.Width = xDifShape - rect.Padding;
                transformingShape.Height = yDifShape - rect.Padding;
                transformingShape.Load();

            }
        }

    }
}