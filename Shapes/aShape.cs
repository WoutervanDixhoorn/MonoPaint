using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint
{
    public abstract class aShape
    {
        //TODO: Create a base constructor that can be called from any class implementing 'aShape'
        protected Texture2D shapeTexture;
        protected int width;
        protected int height;
        protected Vector2 position;
        protected Color color;
        protected Color[] shapeData;

        public int Width
        {
            get { return width; }
            set { width = value; Reload(); }
        }

        public int Height
        {
            get { return height; }
            set { height = value; Reload(); }
        }

        public int X
        {
            get { return (int)position.X; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return (int)position.Y; }
            set { position.Y = value; }
        }

        public abstract void Load();
        public abstract void Reload();
        public abstract void Unload();
        public abstract void Draw(SpriteBatch iSpriteBatch, float iAlpha = 1);

    }
}