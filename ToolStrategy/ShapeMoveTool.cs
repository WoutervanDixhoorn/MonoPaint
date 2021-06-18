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
    public class ShapeMoveTool : IShapeTool
    {
        
        mPlayground playground;

        aShape movingShape;
        aShape drawingShape;

        ShapeComposite group = null;

        public ShapeMoveTool(mPlayground iPlayground)
        {
            playground = iPlayground;
            movingShape = null;
        }

        bool leftClicked = false;
        bool moving = false;

        int deltaX = 0, deltaY = 0;
        public void Update()
        {
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            //Toggles there Hover var when mouse if over
            SelectShapes();

            if(leftClicked && !moving)
            {
                movingShape = getHovered();
                if(movingShape != null)
                {
                    moving = true;
                    //Temp shape for quick acces
                    aShape s = movingShape;

                    
                    if(s.GetType() == typeof(ShapeComposite)){
                        group = (ShapeComposite)s;
                        movingShape = group;
                    }
                        

                    //Setting up a temp shape to draw while moving
                    if(s.ShapeName == "rectangle")
                        drawingShape = new mRectangle(s.Width, s.Height, s.Color);
                    else if(s.ShapeName == "ellipse")
                        drawingShape = new mEllipse(s.Width, s.Height, s.Color);
                    else if(s.ShapeName == "group")
                    {
                        ShapeComposite drawingGroup =  new ShapeComposite();
                        drawingGroup.Add(group.GetChildren());
                        drawingGroup.Visible = true;
                        drawingShape = drawingGroup;
                        drawingShape.Visible = true;
                    }
                    
                    drawingShape.X = s.X;
                    drawingShape.Y = s.Y;
                    drawingShape.Load();
                    
                    deltaX = mX - s.X;
                    deltaY = mY - s.Y;

                    movingShape.Visible = false;
                }
            }

            if(leftClicked && moving)
            {
                drawingShape.X = mX - deltaX;
                drawingShape.Y = mY - deltaY;
            }else if(!leftClicked && moving)
            {   	
                moving = false;

                playground.ExecuteCommand(new MoveCommand(group != null ? group : movingShape, drawingShape.X, drawingShape.Y));
                if(group != null){
                    group = null;
                }

                movingShape.Visible = true;
            }

            if(InputManager.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManager.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }

        void SelectShapes()
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

        aShape getHovered()
        {
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            List<aShape> returnShape = new List<aShape>();

            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    if(shape.Hovered){
                        returnShape.Add(shape);
                    }
                });
            }
            
            if(returnShape.Count > 0)
                return returnShape[0];
            else
                return null;
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            if(drawingShape != null && moving)
            {
                drawingShape.Draw(iSpriteBatch, 0.5f);
            }
        }

        public void Reset()
        {
        }

    }
}