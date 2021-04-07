using System;
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
        protected bool selected;
        protected bool hovered;
        protected Color color;
        protected Color[] shapeData;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int X
        {
            get { return (int)position.X; }
            set { position.X = value + width; }
        }

        public int Y
        {
            get { return (int)position.Y; }
            set { position.Y = value + height; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool Hovered
        {
            get { return hovered; }
            set { hovered = value; }
        }

        public aShape(int iWidth, int iHeight, Color? iColor = null)
        {
            if(iWidth <= 0 || iHeight <= 0)
            {
                throw new NotSupportedException("Cant draw shapes with a width or height lower then 1 yet");
            }

            width = iWidth;
            height = iHeight;
            position = new Vector2(0, 0);
            color = iColor ?? Color.HotPink;
        }
        
        public abstract bool Contains(int iX, int iY);
        public abstract void Load();
        public abstract void Reload();
        public abstract void Unload();
        public void Draw(SpriteBatch iSpriteBatch, float iAlpha = 1)
        {
            if(shapeTexture == null)
            {
                throw new System.NullReferenceException("shapeTexture is null");
            }

            if(hovered || selected)
                iAlpha = 0.5f;

            iSpriteBatch.Draw(shapeTexture, position, Color.White * iAlpha); 
        }

    }
}