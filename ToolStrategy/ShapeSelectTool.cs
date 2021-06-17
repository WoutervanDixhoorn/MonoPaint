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

        bool typing;

        public ShapeSelectTool(mPlayground iPlayground)
        {
            playground = iPlayground;
            selectionRect = new mRectangle(1,1, Color.LightBlue);
            selectionRect.BorderColor = Color.MidnightBlue;
            selectionRect.BorderSize = 2;
            selectionRect.DrawBorder = true;
            selectionRect.Selected = true;
            selectionRect.Load();

            typing = false;
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
            if(InputManager.IsKeyPressed(Keys.B))
            {
                foreach(mCanvas c in playground.Canvases)
                {
                    c.ForAllShapes((aShape shape) => {
                        if(shape.Selected)
                            shape.DrawBorder = !shape.DrawBorder;
                    });
                }
            }
            
            if(InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyPressed(Keys.G))
            {
                List<aShape> selectedShapes = getSelectedShapes();
                ShapeComposite group = null;
                if(selectedShapes.Count > 0)
                {
                    group = new ShapeComposite();
                    foreach(aShape s in selectedShapes)
                    {
                        group.Add(s);
                    }
                }
                if(group != null){
                    deleteShapes(selectedShapes);
                    playground.AddShape(group);
                }

                Reset();
            }

            if(leftClicked && !selecting && InputManager.IsKeyDown(Keys.LeftControl))
            {
                selecting = true;

                int mX = InputManager.CurrentMouseState.X;
                int mY = InputManager.CurrentMouseState.Y;

                selectionRect.X = mX;
                selectionRect.Y = mY;

            }else if(leftClicked && selecting && InputManager.IsKeyDown(Keys.LeftControl))
            {
                int newWidth = Util.Clamp(InputManager.CurrentMouseState.X - selectionRect.X, 1, playground.Width);
                int newHeight = Util.Clamp(InputManager.CurrentMouseState.Y - selectionRect.Y, 1, playground.Height);

                Console.WriteLine("Width: " + newWidth);

                selectionRect.Width = newWidth;
                selectionRect.Height = newHeight;
                selectionRect.Load();
            }else if(!leftClicked && selecting && InputManager.IsKeyDown(Keys.LeftControl))
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


            if(!typing){
                //For adding text
                if(InputManager.IsKeyPressed(Keys.Up))
                {
                    typing = true;
                    AddText(TextPos.Top);
                }else if(InputManager.IsKeyPressed(Keys.Left))
                {
                    typing = true;
                    AddText(TextPos.Left);
                }else if(InputManager.IsKeyPressed(Keys.Down))
                {
                    typing = true;
                    AddText(TextPos.Bottom);
                }else if(InputManager.IsKeyPressed(Keys.Right))
                {
                    typing = true;
                    AddText(TextPos.Right);
                }
            }

            if(!typing && (InputManager.IsKeyPressed(Keys.Delete) || InputManager.IsKeyPressed(Keys.Back)))
            {
                deleteShapes();
            }

            if(InputManager.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManager.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }


        void SelectShape(aShape iShape)
        {          
            int x = InputManager.CurrentMouseState.X;
            int y = InputManager.CurrentMouseState.Y;

            if(iShape.Contains(x, y))
            {
                iShape.Hovered = true;
            }else{
                iShape.Hovered = false;
            }

            if(leftClicked && InputManager.IsReleased(MouseInput.LeftButton))
            {
                if(iShape.Hovered)
                {
                    iShape.Selected = !iShape.Selected;
                }else if(!InputManager.IsKeyDown(Keys.LeftShift)){
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

            if(leftClicked && InputManager.IsKeyDown(Keys.LeftControl))
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
            if(getFirstSelectedShape() == null){
                typing = false;
                return;
            }

            Vector2 pos = new Vector2(getFirstSelectedShape().X, getFirstSelectedShape().Y);

            textBox = UIBasics.BasicTextbox((int)pos.X + 3, (int)pos.Y - 35, (s) => {
                typing = false;
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

        List<aShape> getSelectedShapes()
        {
            List<aShape> selectedShapes = new List<aShape>();
            foreach(mCanvas c in playground.Canvases)
            {
                c.ForAllShapes((aShape shape) => {
                    if(shape.Selected)
                        selectedShapes.Add(shape);
                });
            }

            return selectedShapes;
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

        void deleteShapes(List<aShape> shapes)
        {
            foreach(mCanvas c in playground.Canvases)
            {
                for(int i = 0; i < shapes.Count; i++)
                {
                    if(c.Shapes.Contains(shapes[i]))
                    {
                        c.Shapes.Remove(shapes[i]);
                    }
                }
            }
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