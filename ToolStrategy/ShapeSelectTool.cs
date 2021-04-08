using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;

namespace MonoPaint.ToolStrategy
{
    public class ShapeSelectTool : IShapeTool
    {
        
        mPlayground playground;

        public ShapeSelectTool(mPlayground iPlayground)
        {
            playground = iPlayground;
        }

        bool leftClicked = false;
        public void Update()
        {
            /*TODO: Move to 'UI' or/and keyboard shortcut*/
            if(InputManger.IsKeyPressed(Keys.B))
            {
                foreach(mCanvas c in playground.Canvases)
                {
                    c.ForAllShapes((aShape shape) => {
                        if(shape.Selected)
                            shape.DrawBorder = !shape.DrawBorder;
                    });
                }
            }
            
            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes(SelectShape);
            }

            if(InputManger.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManger.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }


        void SelectShape(aShape iShape)
        {          
            int x = InputManger.CurrentMouseState.X;
            int y = InputManger.CurrentMouseState.Y;

            if(iShape.Contains(x, y))
            {
                iShape.Hovered = true;
            }else{
                iShape.Hovered = false;
            }

            if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton))
            {
                if(iShape.Hovered)
                {
                    iShape.Selected = !iShape.Selected;
                }else if(!InputManger.IsKeyDown(Keys.LeftShift)){
                    iShape.Selected = false;
                }
            }
        }
    }
}