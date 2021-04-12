namespace MonoPaint.Commands
{
    public class DrawCommand : ICommand
    {
        aShape currentShape;
        mPlayground playground;

        public DrawCommand(aShape iShape, mPlayground iPlayground)
        {
            currentShape = iShape;
            playground = iPlayground;
        }

        public void Execute()
        {
            playground.AddShape(currentShape);
        }

        public void Undo()
        {
            playground.RemoveShape(currentShape);
        }

    }
}