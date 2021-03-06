using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.IO;
using System.Security;
//TODO: Move File Dialog to abstacted class
#if Windows
using System.Windows.Forms;
#elif OSX
using AppKit;
#endif

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.UI;
using MonoPaint.Shapes;
using MonoPaint.Commands;
using MonoPaint.Graphics;
using MonoPaint.ToolStrategy;
using MonoPaint.FileParsing;
using MonoPaint.Decorator;

namespace MonoPaint
{
    public class mPlayground
    {
        mScreen screen;
        List<mCanvas> layers;

        CommandHandler commandHandler;

        IShapeTool shapeTool;

        int width = 1280, height = 670;//TODO: Dont hard code width and height here

        public List<mCanvas> Canvases
        {
            get { return layers; }
        }

        public List<aShape> Shapes
        {
            get { 
                List<aShape> list = new List<aShape>();

                foreach(mCanvas c in Canvases)
                {
                    c.ForAllShapes((aShape shape) => {
                        list.Add(shape);
                    });
                }

                return list;
            }
        }

        public int Width {
            get { return width; }
        }

        public int Height {
            get { return height; }
        }

        SpriteFont tempFont;

        //UI
        UIButton drawButton;
        UIButton selectButton;
        UIButton moveButton;
        UIButton transformButton;

        UIButton saveButton;
        UIButton openButton;

        public mPlayground()
        {
            screen = new mScreen(width, height);

            layers = new List<mCanvas>();
            layers.Add(new mCanvas(width, height));

            commandHandler = new CommandHandler();

            shapeTool = new ShapeDrawTool(this);

            tempFont = ContentHandler.Instance.Content.Load<SpriteFont>("TempFont");

            //UI
            drawButton = new UIButton(5, 675, 80, 40);
            drawButton.Text = "Draw";
            drawButton.Color = Color.LightBlue;
            drawButton.BorderColor = Color.LightGreen;
            drawButton.Border = true;
            drawButton.OnPress = () => { shapeTool.Reset(); shapeTool = new ShapeDrawTool(this);
                                       drawButton.Border = true; selectButton.Border = false; moveButton.Border = false; transformButton.Border = false;};
        
            selectButton = new UIButton(90, 675, 80, 40);
            selectButton.Text = "Select";
            selectButton.Color = Color.LightBlue;
            selectButton.BorderColor = Color.LightGreen;
            selectButton.OnPress = () => { shapeTool.Reset(); shapeTool = new ShapeSelectTool(this);
                                         drawButton.Border = false; selectButton.Border = true; moveButton.Border = false; transformButton.Border = false;};
            
            moveButton = new UIButton(175, 675, 80, 40);
            moveButton.Text = "Move";
            moveButton.Color = Color.LightBlue;
            moveButton.BorderColor = Color.LightGreen;
            moveButton.OnPress = () => { shapeTool.Reset(); shapeTool = new ShapeMoveTool(this);
                                         drawButton.Border = false; selectButton.Border = false; moveButton.Border = true; transformButton.Border = false;};

            transformButton = new UIButton(260, 675, 80, 40);
            transformButton.Text = "Resize";
            transformButton.Color = Color.LightBlue;
            transformButton.BorderColor = Color.LightGreen;
            transformButton.OnPress = () => { shapeTool.Reset(); shapeTool = new ShapeTransformTool(this);
                                            drawButton.Border = false; selectButton.Border = false; moveButton.Border = false; transformButton.Border = true;};            

            saveButton = new UIButton(1195, 675, 80, 40);
            saveButton.Text = "Save";
            saveButton.Color = Color.LightBlue;
            saveButton.BorderColor = Color.LightGreen;
            saveButton.OnPress = savePlayground;

            openButton = new UIButton(1110, 675, 80, 40);
            openButton.Text = "Open";
            openButton.Color = Color.LightBlue;
            openButton.BorderColor = Color.LightGreen;
            openButton.OnPress =  loadPlayground;
        }

        async void savePlayground()
        {
            var filePath = string.Empty;

#if Windows
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = "D:\\School-D\\Jaar 2 1\\Periode 3\\Design Patterns\\MonoPaint\\Saves";
                    saveFileDialog.Filter = "monoPaint files (*.mp)|";
                    saveFileDialog.FilterIndex = 2;
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = saveFileDialog.FileName;
                    }
                }
#elif OSX
        //NOTE: Nothing osx api not available!!
#endif

            if(filePath != string.Empty){
                int index = filePath.LastIndexOf('.');
                if(filePath.Substring(index+1) == "mp")
                {
                    await ShapeSerializer.Serialize(Shapes, filePath);
                }else{
                    await ShapeSerializer.Serialize(Shapes, filePath + ".mp");
                }
            }    
        }

        async void loadPlayground()
        {
            var filePath = string.Empty;

#if Windows
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "D:\\School-D\\Jaar 2 1\\Periode 3\\Design Patterns\\MonoPaint\\Saves";
                    openFileDialog.Filter = "monoPaint files (*.mp)|";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = openFileDialog.FileName;
                    }
                }
            }
#elif OSX
                filePath = "Saves/save.mp";
#endif

            if(filePath != string.Empty){
                if(File.Exists(filePath)){
                List<aShape> loadedShapes = ShapeSerializer.Deserialize(filePath);
                SetShapes(loadedShapes);
                }else{
                    Console.WriteLine("You need to save before you can open the save");
                }
            }

        }

        public void AddShape(aShape shape)
        {
            layers[0].Shapes.Add(shape);//TODO: Add to selected layer
        }

        public void RemoveShape(aShape shape)
        {
            layers[0].Shapes.Remove(shape);
        }

        public void SetShapes(List<aShape> iShapes)
        {
            layers[0].Shapes = iShapes;
        }

        public void Load()
        {
            foreach(mCanvas c in layers)
            {
                c.Load();
            }

            drawButton.Load();
            selectButton.Load();
            moveButton.Load();
            transformButton.Load();
            saveButton.Load();
            openButton.Load();
        }

        public void Unload()
        {
            foreach(mCanvas c in layers)
            {
                c.Unload();
            }

            drawButton.Unload();
            selectButton.Unload();
            moveButton.Unload();
            transformButton.Unload();
            saveButton.Unload();
            openButton.Unload();
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
            moveButton.Update();
            transformButton.Update();
            saveButton.Update();
            openButton.Update();
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

            drawButton.Draw(iSpriteBatch);
            selectButton.Draw(iSpriteBatch);
            moveButton.Draw(iSpriteBatch);
            transformButton.Draw(iSpriteBatch);
            saveButton.Draw(iSpriteBatch);
            openButton.Draw(iSpriteBatch);

            iSpriteBatch.End();
        }

        public void ExecuteCommand(ICommand iCommand)
        {
            commandHandler.Execute(iCommand);
        }

        void UpdateInput()
        {
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            //TODO: Maybe fix the whole microsoft.xna.. thing
            if(screen.IsOnScreen(mX, mY)){
                if(InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Z))
                {
                    commandHandler.Undo();
                }else if(InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    commandHandler.Redo();
                }

                shapeTool.Update(); 
            }
        }
    }
}