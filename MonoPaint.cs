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

        Vector2 baseScreenSize = new Vector2(800, 480);

        //PaintVars
        mPlayground monoPlayground;

        public MonoPaint()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true; 

            Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowResize);

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            monoPlayground = new mPlayground();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentHandler.Instance.Load(Content, GraphicsDevice, graphics);

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            monoPlayground.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        void WindowResize(object sender, EventArgs e)
        {
            int nWidth = Window.ClientBounds.Width, nHeight = Window.ClientBounds.Height;

            graphics.PreferredBackBufferWidth = nWidth;
            graphics.PreferredBackBufferHeight = nHeight;
            graphics.ApplyChanges();

            monoPlayground.Resize();

            Console.WriteLine("[Resize] Width: " + nWidth + " Height: " + nHeight);
        }
    }
}
