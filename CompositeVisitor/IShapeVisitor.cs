using MonoPaint.Shapes;

namespace MonoPaint.CompositeVisitor
{
    public interface IShapeVisitor
    {
        void Visit(aShape shape);
        void Visit(mRectangle rectangle);
        void Visit(mEllipse ellipse);
    }
}