using System.Security.Cryptography.X509Certificates;
using System;
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

        public mPlayground()
        {
            screen = new Screen(width, height);

            layers = new List<mCanvas>();
            layers.Add(new mCanvas(width, height));

            shapeDrawer = new ShapeDrawer(this);
        }

        public void AddShape(aShape shape)
        {
            layers[0].Shapes.Add(shape);
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
        }

        void UpdateInput()
        {
            shapeDrawer.UpdateInput(); 
        }

    }
}