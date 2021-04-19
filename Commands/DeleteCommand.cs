namespace MonoPaint.Commands
{
    public class DeleteCommand : ICommand
    {

        mPlayground playground;
        aShape deletedShape;

        public DeleteCommand(aShape iShape, mPlayground iPlayground)
        {
            playground = iPlayground;
            deletedShape = iShape;
        }

        public void Execute()
        {
            playground.RemoveShape(deletedShape);
        }

        public void Undo()
        {
            playground.AddShape(deletedShape);
        }

    }
}