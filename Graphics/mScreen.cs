using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint.Graphics
{
    public class mScreen
    {
        RenderTarget2D screenTarget;

        int x, y;        
        int width, height;

        public int X{ set{ x = value; } get{ return x; }}
        public int Y{ set{ y = value; } get{ return y; }}

        public int Width{ private set{ width = value; } get{ return width; }}
        public int Height{ private set{ height = value; } get{ return height; }}

        bool isSet;

        public mScreen(int iWidth, int iHeight)
        {   
            x = 0; y = 0;
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
            ContentHandler.Instance.Graphics.Clear(Color.White);
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
            ContentHandler.Instance.Graphics.Clear(Color.Black);
                  
            iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.LinearClamp, DepthStencilState.Default,
            RasterizerState.CullNone);
            
            iSpriteBatch.Draw(screenTarget, new Rectangle(x,y, width, height), Color.White);

            iSpriteBatch.End();
        }

        public bool IsOnScreen(int iX, int iY)
        {
            return (iX > x && iX < x + width &&
                    iY > y && iY < y + height);
        }
    }
}