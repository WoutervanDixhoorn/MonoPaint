using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;

namespace MonoPaint
{
    public class mCanvas
    {   
        RenderTarget2D renderTarget;
        List<aShape> shapes;

        bool resizePending;

        int width, height;

        public int Width{ private set{ width = value; } get{ return width; }}
        public int Height{ private set{ height = value; } get{ return height; }}

        public mCanvas(int iWidth, int iHeight)
        {
            shapes = new List<aShape>();

            resizePending = false;

            width = iWidth;
            height = iHeight;

            mRectangle Rect1 = new mRectangle(100, 150, Color.Blue);
            Rect1.X = 50; Rect1.Y = 50;
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
           
            if(resizePending)
            {
                RenderTarget2D newTarget = new RenderTarget2D(
                gd,
                gd.PresentationParameters.BackBufferWidth,
                gd.PresentationParameters.BackBufferHeight,
                false,
                gd.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

                gd.SetRenderTarget(newTarget);

                gd.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
                
                iSpriteBatch.End();

                iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);
                iSpriteBatch.Draw(renderTarget, new Vector2(0,0), Color.White);
                iSpriteBatch.End();

                iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);
            }else{
                gd.SetRenderTarget(renderTarget);
                
                gd.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
        
                 // Draw the scene
                gd.Clear(Color.White);
                foreach(aShape shape in shapes)
                {
                    shape.Draw(iSpriteBatch);
                }
            }
        
            gd.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            DrawSceneToTexture(renderTarget, iSpriteBatch);
            iSpriteBatch.Draw(renderTarget, new Rectangle(0, 0, width, height), Color.White);
        }

        public void Resize()
        {
            resizePending = true;        
        }
    }

}