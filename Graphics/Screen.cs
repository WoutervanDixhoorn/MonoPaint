using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint.Graphics
{
    public class Screen
    {
        RenderTarget2D screenTarget;
        
        int width, height;

        public int Width{ private set{ width = value; } get{ return width; }}
        public int Height{ private set{ height = value; } get{ return height; }}

        bool isSet;

        public Screen(int iWidth, int iHeight)
        {   
            width = iWidth; height = iHeight;

            GraphicsDevice gd = ContentHandler.Instance.Graphics;
            screenTarget = new RenderTarget2D(
                            gd,
                            width, height,
                            false, 
                            gd.PresentationParameters.BackBufferFormat,
                            DepthFormat.Depth24);

            isSet = false;
        }

        public void Set()
        {
            if(isSet)
            {
                throw new System.Exception("RenderTarget already set!");
            }

            ContentHandler.Instance.Graphics.SetRenderTarget(screenTarget);
        }

        public void Unset()
        {
            if(isSet)
            {
                throw new System.Exception("RenderTarget not set!");
            }

            ContentHandler.Instance.Graphics.SetRenderTarget(null);
        }

        public void Present(SpriteBatch iSpriteBatch)
        {      
            ContentHandler.Instance.Graphics.Clear(Color.HotPink);
                  
            iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.LinearClamp, DepthStencilState.Default,
            RasterizerState.CullNone);

            iSpriteBatch.Draw(screenTarget, new Rectangle(0,0, width, height), Color.White);

            iSpriteBatch.End();
        }
    }
}