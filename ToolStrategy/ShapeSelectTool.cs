using System.Net.NetworkInformation;
using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Commands;
using MonoPaint.Shapes;
using MonoPaint.Decorator;
using MonoPaint.UI;

namespace MonoPaint.ToolStrategy
{
    public class ShapeSelectTool : IShapeTool
    {
        
        mPlayground playground;

        mRectangle selectionRect;

        UITextBox textBox;

        public ShapeSelectTool(mPlayground iPlayground)
        {
            playground = iPlayground;
            selectionRect = new mRectangle(1,1, Color.LightBlue);
            selectionRect.BorderColor = Color.MidnightBlue;
            selectionRect.BorderSize = 2;
            selectionRect.DrawBorder = true;
            selectionRect.Selected = true;
            selectionRect.Load();
        }

        bool leftClicked = false;
        bool selecting = false;
        public void Update()
        {
            if(textBox != null)
            {
                textBox.Update();
            }

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
            
           if(leftClicked && !selecting && InputManger.IsKeyDown(Keys.LeftControl))
            {
                selecting = true;

                int mX = InputManger.CurrentMouseState.X;
                int mY = InputManger.CurrentMouseState.Y;

                selectionRect.X = mX;
                selectionRect.Y = mY;

            }else if(leftClicked && selecting && InputManger.IsKeyDown(Keys.LeftControl))
            {
                int newWidth = Util.Clamp(InputManger.CurrentMouseState.X - selectionRect.X, 1, playground.Width);
                int newHeight = Util.Clamp(InputManger.CurrentMouseState.Y - selectionRect.Y, 1, playground.Height);

                Console.WriteLine("Width: " + newWidth);

                selectionRect.Width = newWidth;
                selectionRect.Height = newHeight;
                selectionRect.Load();
            }else if(!leftClicked && selecting && InputManger.IsKeyDown(Keys.LeftControl))
            {
                selecting = false;

                foreach(mCanvas c in playground.Canvases)
                {
                    c.ForAllShapes((aShape iShape) => {
                        if(selectionRect.Intersects(iShape)){
                            iShape.Selected = true;
                        }  
                    });
                }

                if(selectionRect.Width > 0)
                {
                    selectionRect.Width = 1;
                    selectionRect.Height = 1;
                    selectionRect.Load();
                }
            }else{
                foreach(mCanvas c in playground.Canvases)
                {
                    c.ForAllShapes(SelectShape);
                }
            }

            //For adding text
            if(InputManger.IsKeyPressed(Keys.Up))
            {
                AddText(TextPos.Top);
            }else if(InputManger.IsKeyPressed(Keys.Left))
            {
                AddText(TextPos.Left);
            }else if(InputManger.IsKeyPressed(Keys.Down))
            {
                AddText(TextPos.Bottom);
            }else if(InputManger.IsKeyPressed(Keys.Right))
            {
                AddText(TextPos.Right);
            }

            if(InputManger.IsKeyPressed(Keys.Delete) || InputManger.IsKeyPressed(Keys.Back))
            {
                deleteShapes();
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

        public void Draw(SpriteBatch iSpriteBatch)
        {
            if(textBox != null)
            {
                textBox.Draw(iSpriteBatch);
            }

            if(leftClicked && InputManger.IsKeyDown(Keys.LeftControl))
                selectionRect.Draw(iSpriteBatch, 0.8f);
        }

        public void Reset()
        {
            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    shape.Selected = false;
                });
            }
        }
        
        public void AddText(TextPos textPos)
        {
            if(getFirstSelectedShape() == null)
                return;

            Vector2 pos = getFirstSelectedShape().Position;

            textBox = UIBasics.BasicTextbox((int)pos.X + 3, (int)pos.Y - 35, (s) => {
                addText(s, textPos);
            });
        }

        void addText(string text, TextPos textPos)
        {
            foreach(mCanvas c in playground.Canvases)
            {
                foreach(aShape shape in c.Shapes){
                    if(shape.Selected){
                        playground.ExecuteCommand(new AddTextCommand(playground, shape, text, textPos));
                        break;
                    }  
                }     
            }

            textBox = null;
        }

        aShape getFirstSelectedShape()
        {
            foreach(mCanvas c in playground.Canvases)
            {
                foreach(aShape s in c.Shapes)
                {
                    if(s.Selected)
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        void deleteShapes()
        {
            List<DeleteCommand> deletingShapes = new List<DeleteCommand>();

            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                   if(shape.Selected)
                   {
                        deletingShapes.Add(new DeleteCommand(shape, playground));
                   }
                });
            }

            foreach(DeleteCommand d in deletingShapes)
            {
                playground.ExecuteCommand(d);
            }
        }

    }
}