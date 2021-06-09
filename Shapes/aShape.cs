using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoPaint.Shapes;
using MonoPaint.Graphics;
using MonoPaint.CompositeVisitor;

namespace MonoPaint
{
    public struct SelectionRectangle
    {
        public int Padding;

        public mRectangle SelectRect;

        public mEllipse TopLeft;
        public mEllipse TopRight;
        public mEllipse BottomLeft;
        public mEllipse BottomRight;

        public void Draw(SpriteBatch iSpriteBatch)
        {
                SelectRect.Draw(iSpriteBatch);
                TopLeft.Draw(iSpriteBatch);
                TopRight.Draw(iSpriteBatch);
                BottomLeft.Draw(iSpriteBatch);
                BottomRight.Draw(iSpriteBatch);
        }
    }
    
    public abstract class aShape
    {   
        public string ShapeName;

        protected iShapeDrawer shapeDrawer;

        protected Texture2D shapeTexture;
        protected Color[] shapeData;
        protected Color color;

        protected Texture2D borderTexture;
        protected Color[] borderData;
        protected Color borderColor;

        protected SelectionRectangle selectionRect;

        protected int width;
        protected int height;
        protected int borderSize;
        protected Vector2 position;
        
        protected bool visible;
        protected bool selected;
        protected bool transforming;
        protected bool hovered;
        protected bool drawBorder;

        public virtual Color Color{
            set { color = value; }
            get { return color;}
        }
    
        public virtual Color BorderColor{
            set { borderColor = value; }
        }

        public virtual int Width
        {
            get { return width; }
            set { width = value; }
        }

        public virtual int Height
        {
            get { return height; }
            set { height = value; }
        }

        public virtual int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; }
        }

        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual int X
        {
            get { return (int)position.X; }
            set { position.X = value; }
        }

        public virtual int Y
        {
            get { return (int)position.Y; }
            set { position.Y = value; }
        }

        public virtual Vector2 TopLeft
        {
            get { return new Vector2(X, Y); }
        }

        public virtual Vector2 TopRight
        {
            get { return new Vector2(X + width, Y);}
        }

        public virtual Vector2 BottomLeft
        {
            get { return new Vector2(X, Y + height); }
        }

        public virtual Vector2 BottomRight
        {
            get { return new Vector2(X + width, Y + height); }
        }

        public virtual SelectionRectangle SelectionRect
        {
            get { return selectionRect; }
        }

        public virtual bool Visible
        {
            get { return visible; }
            set { visible = value;}
        }

        public virtual bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public virtual bool Transforming
        {
            get { return transforming; }
            set { transforming = value;
             if(transforming && selectionRect.SelectRect == null){
                 GenerateTransformRect();
               }}
        }

        public virtual bool Hovered
        {
            get { return hovered; }
            set { hovered = value; }
        }

        public virtual bool DrawBorder
        {
            get { return drawBorder; }
            set { drawBorder = value; }
        }

        public virtual iShapeDrawer ShapeDrawer
        {
            get { return shapeDrawer; }
        }

        public aShape(int iWidth = 1, int iHeight = 1, Color? iColor = null)
        {
            if(iWidth <= 0 || iHeight <= 0)
            {
                throw new NotSupportedException("Cant draw shapes with a width or height lower then 1 yet");
            }

            visible = true;

            width = iWidth;
            height = iHeight;
            borderSize = 3;
            drawBorder = false;
            position = new Vector2(0, 0);
            color = iColor ?? Color.HotPink;
            borderColor = Color.Black;
        }

        public virtual bool Intersects(aShape iShape)
        {
            if((  TopLeft.X  >  iShape.BottomRight.X )||(BottomRight.X  <  iShape.TopLeft.X)
            ||(TopLeft.Y > iShape.BottomRight.Y )||(BottomRight.Y  <  iShape.TopLeft.Y))
            {
                return false;
            }
            return true;
        }
        public abstract bool Contains(int iX, int iY);

        public virtual void Load()
        {
            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width, height, false, SurfaceFormat.Color);
            shapeData = shapeDrawer.GetData(width, height, color);            
            if(shapeData == null)
            {
                //Nothing ToDo
            }else{
                shapeTexture.SetData(shapeData);

                if(drawBorder){
                    int borderWidth = width+(borderSize*2), borderHeight = height+(borderSize*2);
                    borderTexture = new Texture2D(ContentHandler.Instance.Graphics, borderWidth, borderHeight, false, SurfaceFormat.Color);
                    borderTexture.SetData(shapeDrawer.GetBorderData(width, height, borderSize, borderColor));
                }

                if(transforming)
                {
                    GenerateTransformRect();
                }
            }
        }

        public virtual void Unload()
        {
            shapeData = null;
            borderData = null;

            if(shapeTexture != null)
                shapeTexture.Dispose();
            if(borderTexture != null)
                borderTexture.Dispose();
        }

        public abstract void LoadWhileDrawing(); //NOTE: This is temp code for drawing faster ellipse
       
        public virtual void Draw(SpriteBatch iSpriteBatch, float iAlpha = 1)
        {
            if(!visible)
                return;

            if(shapeTexture == null)
            {
                //Nothing ToDo
            }else{

                if(hovered || selected)
                    iAlpha = 0.5f;

                iSpriteBatch.Draw(shapeTexture, position, Color.White * iAlpha); 
                if(drawBorder){
                    if(borderTexture == null)
                        Load();
                    iSpriteBatch.Draw(borderTexture, position - new Vector2(borderSize), Color.White * iAlpha); 
                }

                if(transforming)
                {
                    selectionRect.Draw(iSpriteBatch);
                }   
            }
        }

        /*private funcs*/
        public void GenerateTransformRect()
        {
            selectionRect = new SelectionRectangle();
            selectionRect.Padding = 9;
            int padding = selectionRect.Padding;

            int width = Width + (padding * 2);
            int height = Height + (padding * 2);

            mRectangle rect = new mRectangle(width, height, Color.Transparent);
            rect.X = X - padding; rect.Y = Y - padding;
            rect.BorderSize = 2;
            rect.DrawBorder = true;
            rect.Load();

            int handleSize = 10;
            int correction = handleSize/2;//\(handleSize + (handleSize/2));

            mEllipse topleft = new mEllipse(handleSize, handleSize, Color.Black);
            topleft.X = rect.X - correction; topleft.Y = rect.Y - correction;
            topleft.Load();
            selectionRect.TopLeft = topleft;

            mEllipse topright = new mEllipse(handleSize, handleSize, Color.Black);
            topright.X = rect.X + rect.Width - correction; topright.Y = rect.Y - correction;
            topright.Load();
            selectionRect.TopRight = topright;

            mEllipse bottomleft = new mEllipse(handleSize, handleSize, Color.Black);
            bottomleft.X = rect.X - correction; bottomleft.Y = rect.Y + rect.Height - correction;
            bottomleft.Load();
            selectionRect.BottomLeft = bottomleft;

            mEllipse bottomright = new mEllipse(handleSize, handleSize, Color.Black);
            bottomright.X = rect.X + rect.Width - correction; bottomright.Y = rect.Y + rect.Height - correction;
            bottomright.Load();
            selectionRect.BottomRight = bottomright;

            selectionRect.SelectRect = rect;

        }
    
        public abstract void Accept(IShapeVisitor shapeVisitor);
    }
}