namespace MonoPaint.Commands
{
    public class MoveCommand : ICommand
    {
        aShape shape;
        int oldX, oldY;
        int newX, newY;

        public MoveCommand(aShape iShape, int iX, int iY)
        {
            shape = iShape;
            oldX = iShape.X;
            oldY = iShape.Y;

            newX = iX;
            newY = iY;
        }

        public void Execute()
        {
            shape.X = newX;
            shape.Y = newY;

            shape.GenerateTransformRect();
        }

        public void Undo()
        {
            shape.X = oldX;
            shape.Y = oldY;

            shape.GenerateTransformRect();
        }
    }
}