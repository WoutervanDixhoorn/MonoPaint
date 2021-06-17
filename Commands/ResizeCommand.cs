using MonoPaint.CompositeVisitor;

namespace MonoPaint.Commands
{
    public class ResizeCommand : ICommand
    {
        aShape currentShape;
        int oldWidth, oldHeight, oldX, oldY;
        int newWidth, newHeight, newX, newY;

        ShapeVisitorResize resizeVisitor;
        ShapeVisitorResize undoResizeVisitor;

        public ResizeCommand(aShape iShape, int iWidth, int iHeight, int iX, int iY)
        {
            currentShape = iShape;
            oldWidth = iShape.Width;
            oldHeight = iShape.Height;
            oldX = iShape.X;
            oldY = iShape.Y;
            
            newX = iX;
            newY = iY;

            newWidth = Util.Clamp(iWidth, 1, int.MaxValue);
            newHeight =  Util.Clamp(iHeight, 1, int.MaxValue);

            resizeVisitor = new ShapeVisitorResize(newWidth, newHeight, newX, newX);
            undoResizeVisitor = new ShapeVisitorResize(oldWidth, oldHeight, oldX, oldY);
        }

        public ResizeCommand(aShape iShape, int iWidth, int iHeight)
        {
            currentShape = iShape;
            oldWidth = iShape.Width;
            oldHeight = iShape.Height;
            oldX = iShape.X;
            oldY = iShape.Y;
            
            newX = iShape.X;
            newY = iShape.Y;

            newWidth = Util.Clamp(iWidth, 1, int.MaxValue);
            newHeight =  Util.Clamp(iHeight, 1, int.MaxValue);

            resizeVisitor = new ShapeVisitorResize(newWidth, newHeight, newX, newY);
            undoResizeVisitor = new ShapeVisitorResize(oldWidth, oldHeight, oldX, oldY);
        }

        public void Execute()
        {
            // currentShape.Width = newWidth;
            // currentShape.Height = newHeight;
            // currentShape.X = newX;
            // currentShape.Y = newY;
            // currentShape.Load();
            currentShape.Accept(resizeVisitor);
        }

        public void Undo()
        {
            // currentShape.Width = oldWidth;
            // currentShape.Height = oldHeight;
            // currentShape.X = oldX;
            // currentShape.Y = oldY;
            // currentShape.Load(); 
            currentShape.Accept(undoResizeVisitor);
        }
    }
}