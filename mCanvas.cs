using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame;

using Input;
using MonoPaint.Shapes;


namespace MonoPaint
{
    public class mCanvas
    {   
        RenderTarget2D renderTarget;

        List<aShape> shapes;

        public mCanvas()
        {
            shapes = new List<aShape>();
            
            aShape Ellps1 = new mEllipse(100, 150, Color.HotPink);
            Ellps1.X = 50; Ellps1.Y = 50;
            shapes.Add(Ellps1);

            aShape Rect1 = new mRectangle(75, 100, Color.BurlyWood);
            Rect1.X = 450; Rect1.Y = 200;
            shapes.Add(Rect1);

            Console.WriteLine("Initialized canvas with: " + shapes.Count + " shapes");
        }

        public void Load()
        {
            GraphicsDevice gd = ContentHandler.Instance.Graphics;
            renderTarget = new RenderTarget2D(
                gd,
                gd.PresentationParameters.BackBufferWidth,
                gd.PresentationParameters.BackBufferHeight,
                false,
                gd.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            foreach(aShape shape in shapes)
            {
                shape.Load();
            }
        }

        public void Unload()
        {
            foreach(aShape shape in shapes)
            {
                shape.Unload();
            }
        }

        int zoom = 0;
        bool leftClicked = false;
        public void Update()
        {
            int mX = InputManger.CurrentMouseState.X;
            int mY = InputManger.CurrentMouseState.Y;
            
            //Check AABB
            foreach(aShape s in shapes)
            {
                if(InputManger.IsPressed(MouseInput.LeftButton))
                {
                    leftClicked = true;
                }else if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton))
                {
                    if(mX > s.X && mX <  s.X + s.Width &&
                       mY > s.Y &&  mY < s.Y + s.Height)
                    {
                        s.Selected = !s.Selected;
                    }else if(!InputManger.IsKeyDown(Keys.LeftShift)){
                         s.Selected = false;
                    }
                }
            }

            if(InputManger.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;

            zoom += (InputManger.GetMouseScroll()) / 10;
        }

    private void DrawSceneToTexture(RenderTarget2D renderTarget, SpriteBatch iSpriteBatch)
    {
        GraphicsDevice gd = ContentHandler.Instance.Graphics;
        // Set the render target
        gd.SetRenderTarget(renderTarget);
    
        gd.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
    
        // Draw the scene
        gd.Clear(Color.CornflowerBlue);
        foreach(aShape shape in shapes)
        {
            shape.Draw(iSpriteBatch);
        }
    
        // Drop the render target
        gd.SetRenderTarget(null);
    }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.LinearClamp, DepthStencilState.Default,
            RasterizerState.CullNone);

            DrawSceneToTexture(renderTarget, iSpriteBatch);
            iSpriteBatch.Draw(renderTarget, new Rectangle(10, 10, 780, 460), Color.White);

            iSpriteBatch.End();
        }

    }
}