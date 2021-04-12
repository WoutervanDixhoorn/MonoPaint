using System.ComponentModel;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;
using MonoPaint.Commands;
using MonoPaint.Graphics;
using MonoPaint.ToolStrategy;

namespace MonoPaint
{
    public class mPlayground
    {
        Screen screen;
        List<mCanvas> layers;

        CommandHandler commandHandler;

        IShapeTool shapeTool;

        int width = 1280, height = 720;//TODO: Dont hard code width and height here

        public List<mCanvas> Canvases
        {
            get { return layers; }
        }

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
            [Description("Transform")]TransformTool
        }

        Tool currentTool;

        public mPlayground()
        {
            screen = new Screen(width, height);

            layers = new List<mCanvas>();
            layers.Add(new mCanvas(width, height));

            commandHandler = new CommandHandler();

            currentTool = Tool.DrawTool;

            shapeTool = new ShapeDrawTool(this);
            
            tempFont = ContentHandler.Instance.Content.Load<SpriteFont>("TempFont");
        }

        public void AddShape(aShape shape)
        {
            layers[0].Shapes.Add(shape);//TODO: Add to selected layer
        }

        public void RemoveShape(aShape shape)
        {
            layers[0].Shapes.Remove(shape);
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

            shapeTool.Draw(iSpriteBatch);

            iSpriteBatch.End();

            screen.Unset();
            screen.Present(iSpriteBatch);

            /*Move to UI*/
            //Draw temp tool view
            iSpriteBatch.Begin();
            iSpriteBatch.DrawString(tempFont, "Current Tool: " + currentTool.GetDescription(), new Vector2(1000, 700), Color.Black);
            iSpriteBatch.End();
        }

        public void ExecuteCommand(ICommand iCommand)
        {
            commandHandler.Execute(iCommand);
        }

        void UpdateInput()
        {
            /*TODO: Move to UI*/
            if(InputManger.IsKeyPressed(Keys.Space))
            {
                shapeTool.Reset();
                if(currentTool == Tool.DrawTool){
                    currentTool = Tool.SelectTool;
                    shapeTool = new ShapeSelectTool(this);
                }
                else if(currentTool == Tool.SelectTool){
                    currentTool = Tool.TransformTool;
                    shapeTool = new ShapeTransformTool(this);
                }else if(currentTool == Tool.TransformTool)
                {
                    currentTool = Tool.DrawTool;
                    shapeTool = new ShapeDrawTool(this);
                }
            }

            if(InputManger.IsKeyReleased(Keys.Z))
            {
                commandHandler.Undo();
            }

            shapeTool.Update(); 
        }
    }
}