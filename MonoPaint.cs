using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;

namespace MonoPaint
{
    public class MonoPaint : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Vector2 baseScreenSize = new Vector2(1280, 720);

        //PaintVars
        mPlayground monoPlayground;

        public MonoPaint()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = false; 
        }

        protected override void Initialize()
        {
            ContentHandler.Instance.Load(Content, GraphicsDevice, graphics);
            graphics.PreferredBackBufferWidth = (int)baseScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)baseScreenSize.Y;
            graphics.ApplyChanges();

            monoPlayground = new mPlayground();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            monoPlayground.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManger.Update();

            monoPlayground.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            monoPlayground.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
