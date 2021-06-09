using MonoPaint.CompositeVisitor;

namespace MonoPaint.Commands
{
    public class MoveCommand : ICommand
    {
        aShape shape;
        int oldX, oldY;
        int newX, newY;

        int deltaX, deltaY;

        ShapeVisitorMove moveVisitor;
        ShapeVisitorMove undoMoveVisitor;
        
        public MoveCommand(aShape iShape, int iX, int iY)
        {
            shape = iShape;
            oldX = iShape.X;
            oldY = iShape.Y;

            newX = iX;
            newY = iY;

            deltaX = newX - oldX;
            deltaY = newY - oldY;

            moveVisitor = new ShapeVisitorMove(deltaX, deltaY);
            undoMoveVisitor = new ShapeVisitorMove(-deltaX, -deltaY);
        }

        public void Execute()
        {
            shape.Accept(moveVisitor);
            shape.GenerateTransformRect();
        }

        public void Undo()
        {
            shape.Accept(undoMoveVisitor);
            shape.GenerateTransformRect();
        }
    }
}