using System.ComponentModel;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.UI;
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

        int width = 1280, height = 670;//TODO: Dont hard code width and height here

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

        //UI
        UIButton drawButton;
        UIButton selectButton;
        UIButton transformButton;

        public mPlayground()
        {
            screen = new Screen(width, height);

            layers = new List<mCanvas>();
            layers.Add(new mCanvas(width, height));

            commandHandler = new CommandHandler();

            currentTool = Tool.DrawTool;

            shapeTool = new ShapeDrawTool(this);
            
            tempFont = ContentHandler.Instance.Content.Load<SpriteFont>("TempFont");

            //UI
            drawButton = new UIButton(5, 675, 80, 40);
            drawButton.Text = "Draw";
            drawButton.Color = Color.LightBlue;
            drawButton.BorderColor = Color.LightGreen;
            drawButton.OnPress = () => { shapeTool.Reset(); shapeTool = new ShapeDrawTool(this); currentTool = Tool.DrawTool; 
                                       drawButton.Border = true; selectButton.Border = false; transformButton.Border = false;};
        
            selectButton = new UIButton(90, 675, 80, 40);
            selectButton.Text = "Select";
            selectButton.Color = Color.LightBlue;
            selectButton.BorderColor = Color.LightGreen;
            selectButton.OnPress = () => { shapeTool.Reset(); shapeTool = new ShapeSelectTool(this); currentTool = Tool.SelectTool; 
                                         drawButton.Border = false; selectButton.Border = true; transformButton.Border = false;};
            
            transformButton = new UIButton(175, 675, 80, 40);
            transformButton.Text = "Resize";
            transformButton.Color = Color.LightBlue;
            transformButton.BorderColor = Color.LightGreen;
            transformButton.OnPress = () => { shapeTool.Reset(); shapeTool = new ShapeTransformTool(this); currentTool = Tool.TransformTool; 
                                            drawButton.Border = false; selectButton.Border = false; transformButton.Border = true;};

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

            drawButton.Load();
            selectButton.Load();
            transformButton.Load();
        }

        public void Unload()
        {
            foreach(mCanvas c in layers)
            {
                c.Unload();
            }

            drawButton.Unload();
            selectButton.Unload();
            transformButton.Unload();
        }

        public void Update()
        {
            foreach(mCanvas c in layers)
            {
                c.Update();
            }

            UpdateInput();

            drawButton.Update();
            selectButton.Update();
            transformButton.Update();
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

            iSpriteBatch.Begin();
            foreach(mCanvas c in layers)
            {
                c.ForAllShapes((aShape iShape) => {
                    if(iShape.Transforming)
                        iShape.SelectionRect.Draw(iSpriteBatch);
                });
            }
            iSpriteBatch.End();

            /*Move to UI*/
            //Draw temp tool view
            iSpriteBatch.Begin();
            iSpriteBatch.DrawString(tempFont, "Current Tool: " + currentTool.GetDescription(), new Vector2(1000, 700), Color.White);
            
            drawButton.Draw(iSpriteBatch);
            selectButton.Draw(iSpriteBatch);
            transformButton.Draw(iSpriteBatch);

            iSpriteBatch.End();
        }

        public void ExecuteCommand(ICommand iCommand)
        {
            commandHandler.Execute(iCommand);
        }

        void UpdateInput()
        {
            int mX = InputManger.CurrentMouseState.X;
            int mY = InputManger.CurrentMouseState.Y;

            if(screen.IsOnScreen(mX, mY)){
                if(InputManger.IsKeyDown(Keys.LeftControl) && InputManger.IsKeyPressed(Keys.Z))
                {
                    commandHandler.Undo();
                }

                shapeTool.Update(); 
            }
        }
    }
}