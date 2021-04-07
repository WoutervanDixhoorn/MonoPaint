using System.ComponentModel;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;
using MonoPaint.Graphics;
namespace MonoPaint
{
    public class mPlayground
    {
        Screen screen;
        List<mCanvas> layers;

        ShapeDrawer shapeDrawer;

        int width = 640, height = 480;//TODO: Dont hard code width and height here

        public int Width {
            get { return width; }
        }

        public int Height {
            get { return height; }
        }

        SpriteFont tempFont;

        enum Tool{
            [Description("Drawing")]DrawTool,
            [Description("Selecting")]SelectTool,
        }

        Tool currentTool;

        public mPlayground()
        {
            screen = new Screen(width, height);

            layers = new List<mCanvas>();
            layers.Add(new mCanvas(width, height));

            currentTool = Tool.DrawTool;

            shapeDrawer = new ShapeDrawer(this);
            
            tempFont = ContentHandler.Instance.Content.Load<SpriteFont>("TempFont");
        }

        public void AddShape(aShape shape)
        {
            layers[0].Shapes.Add(shape);//TODO: Add to selected layer
        }

        public void Load()
        {
            foreach(mCanvas c in layers)
            {
                c.Load();
            }
        }

        public void Unload()
        {
            foreach(mCanvas c in layers)
            {
                c.Unload();
            }
        }

        public void Update()
        {
            foreach(mCanvas c in layers)
            {
                c.Update();
            }

            UpdateInput();
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            screen.Set();

            iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.LinearClamp, DepthStencilState.Default,
            RasterizerState.CullNone);

            foreach(mCanvas c in layers)
            {
                c.Draw(iSpriteBatch);
            }

            iSpriteBatch.End();

            screen.Unset();
            screen.Present(iSpriteBatch);

            //Draw temp tool view
            iSpriteBatch.Begin();
            iSpriteBatch.DrawString(tempFont, "Current Tool: " + currentTool.GetDescription(), new Vector2(1000, 700), Color.White);
            iSpriteBatch.End();
        }

        bool leftClicked = false;
        void UpdateInput()
        {
            if(InputManger.IsKeyPressed(Keys.Space))
            {
                if(currentTool == Tool.DrawTool)
                    currentTool = Tool.SelectTool;
                else if(currentTool == Tool.SelectTool)
                    currentTool = Tool.DrawTool;
            }

            if(currentTool == Tool.DrawTool)
                shapeDrawer.UpdateInput(); 
            else if(currentTool == Tool.SelectTool)
            {
                foreach(mCanvas c in layers)
                {
                    c.ForAllShapes(SelectShape);
                }
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