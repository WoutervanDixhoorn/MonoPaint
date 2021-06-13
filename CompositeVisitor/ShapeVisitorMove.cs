using MonoPaint.Shapes;
using MonoPaint.Decorator;

namespace MonoPaint.CompositeVisitor
{
    public class ShapeVisitorMove : IShapeVisitor
    {
        int deltaX = 0;
        int deltaY = 0;

        public ShapeVisitorMove(int iDeltaX, int iDeltaY)
        {
            deltaX = iDeltaX;
            deltaY = iDeltaY;
        }
        
        public void Visit(aShape shape)
        {
            shape.X += deltaX;
            shape.Y += deltaY;
        }

        public void Visit(ShapeTextDecorator shape)
        {
            shape.X += deltaX;
            shape.Y += deltaY;
        }

        public void Visit(ShapeComposite group)
        {
            group.X += deltaX;
            group.Y += deltaY;
        }

        public void Visit(mRectangle rectangle)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }

        public void Visit(mEllipse ellipse)
        {
            ellipse.X += deltaX;
            ellipse.Y += deltaY;
        }

    }
}