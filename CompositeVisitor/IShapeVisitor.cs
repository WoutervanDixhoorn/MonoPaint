using MonoPaint.Shapes;
using MonoPaint.Decorator;
using MonoPaint.CompositeVisitor;

namespace MonoPaint.CompositeVisitor
{
    public interface IShapeVisitor
    {
        void Visit(aShape shape);
        void Visit(ShapeTextDecorator rectangle);
        void Visit(ShapeComposite group);
        void Visit(mRectangle rectangle);
        void Visit(mEllipse ellipse);
    }
}