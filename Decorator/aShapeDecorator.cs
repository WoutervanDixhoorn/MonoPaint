using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using MonoPaint.Shapes;
using MonoPaint.Graphics;
using MonoPaint.CompositeVisitor;

namespace MonoPaint.Decorator
{
    public abstract class aShapeDecorator : aShape
    {
        
        public override Color Color{
            set { decoratedShape.Color = value; }
            get { return decoratedShape.Color; }
        }
    
        public override Color BorderColor{
            set { decoratedShape.BorderColor = value; }
        }

        public override int Width
        {
            get { return decoratedShape.Width; }
            set { decoratedShape.Width = value; }
        }

        public override int Height
        {
            get { return decoratedShape.Height; }
            set { decoratedShape.Height = value; }
        }

        public override int BorderSize
        {
            get { return decoratedShape.BorderSize; }
            set { decoratedShape.BorderSize = value; }
        }

        public override int X
        {
            get { return (int)decoratedShape.X; }
            set { decoratedShape.X = value; }
        }

        public override int Y
        {
            get { return decoratedShape.Y; }
            set { decoratedShape.Y = value; }
        }

        public override SelectionRectangle SelectionRect
        {
            get { return decoratedShape.SelectionRect; }
        }

        public override bool Visible
        {
            get { return decoratedShape.Visible; }
            set { decoratedShape.Visible = value;}
        }

        public override bool Selected
        {
            get { return decoratedShape.Selected; }
            set { decoratedShape.Selected = value; }
        }

        public override bool Transforming
        {
            get { return decoratedShape.Transforming; }
            set { decoratedShape.Transforming = value;
             if(decoratedShape.Transforming && decoratedShape.SelectionRect.SelectRect == null){
                 GenerateTransformRect();
               }}
        }

        public override bool Hovered
        {
            get { return decoratedShape.Hovered; }
            set { decoratedShape.Hovered = value; }
        }

        public override bool DrawBorder
        {
            get { return decoratedShape.DrawBorder; }
            set { decoratedShape.DrawBorder = value; }
        }

        protected aShape decoratedShape;

        public aShapeDecorator(aShape iDecoratedShape): base(1, 1, Color.Transparent)
        {
            decoratedShape = iDecoratedShape;
            shapeDrawer = decoratedShape.ShapeDrawer;
            ShapeName = decoratedShape.ShapeName;
        }

        public override void Load()
        {
            decoratedShape.Load();
        }

        public override void Unload()
        {
            decoratedShape.Unload();
        }

        public override void LoadWhileDrawing()
        {
            decoratedShape.LoadWhileDrawing();
        }

        public virtual void OnDraw(SpriteBatch iSpriteBatch)
        {
        }

        //TODO: Fix override bullshit with virtual
        public override void Draw(SpriteBatch iSpriteBatch, float iAlpha = 1)
        {
            decoratedShape.Draw(iSpriteBatch);
            OnDraw(iSpriteBatch);
        }

        public override bool Contains(int iX, int iY)
        {
            return decoratedShape.Contains(iX, iY);
        }

        public override void Accept(IShapeVisitor shapeVisitor)
        {
            shapeVisitor.Visit(decoratedShape);
        }

    }
}