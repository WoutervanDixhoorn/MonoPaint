using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;

namespace MonoPaint
{
    public class mPlayground
    {
        List<mCanvas> layers;

        int width = 800, height = 480;

        bool resizePending = false;

        public mPlayground()
        {
            layers = new List<mCanvas>();

            //Push first layer
            layers.Add(new mCanvas(width, height));
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
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.LinearClamp, DepthStencilState.Default,
            RasterizerState.CullNone);

            foreach(mCanvas c in layers)
            {
                c.Draw(iSpriteBatch);
            }

            iSpriteBatch.End();
        }

        public void Resize()
        {
            foreach(mCanvas c in layers)
            {
                c.Resize();
            }
        }

    }
}