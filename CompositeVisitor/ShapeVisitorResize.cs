using MonoPaint.Shapes;
using MonoPaint.Decorator;

namespace MonoPaint.CompositeVisitor
{
    public class ShapeVisitorResize : IShapeVisitor
    {
        int width = 0;
        int height = 0;
        int x = 0;
        int y = 0;

        public ShapeVisitorResize(int iWidth, int iHeight, int iX, int iY)
        {
            width = iWidth;
            height = iHeight;
            x = iX;
            y = iY;
        }
        
        public void Visit(aShape shape)
        {
            shape.Width = width;
            shape.Height = height;
            shape.X = x;
            shape.Y = y;
            shape.Load();
        }

        public void Visit(ShapeTextDecorator shape)
        {
            shape.Width = width;
            shape.Height = height;
            shape.X = x;
            shape.Y = y;
            shape.Load();
        }

        public void Visit(ShapeComposite group)
        {
            group.Width = width;
            group.Height = height;
            group.X = x;
            group.Y = y;
            group.Load();
        }

        public void Visit(mRectangle rectangle)
        {
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Load();
        }

        public void Visit(mEllipse ellipse)
        {
            ellipse.Width = width;
            ellipse.Height = height;
            ellipse.X = x;
            ellipse.Y = y;
            ellipse.Load();
        }
    }

}