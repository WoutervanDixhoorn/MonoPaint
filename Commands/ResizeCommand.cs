namespace MonoPaint.Commands
{
    public class ResizeCommand : ICommand
    {
        aShape currentShape;
        int oldWidth, oldHeight;
        int newWidth, newHeight;

        public ResizeCommand(aShape iShape, int iWidth, int iHeight)
        {
            currentShape = iShape;
            oldWidth = iShape.Width;
            oldHeight = iShape.Height;
            
            newWidth = iWidth;
            newHeight = iHeight;
        }

        public void Execute()
        {
            currentShape.Width = newWidth;
            currentShape.Height = newHeight;
            currentShape.Load();
        }

        public void Undo()
        {
            currentShape.Width = oldWidth;
            currentShape.Height = oldHeight;
            currentShape.Load(); 
        }
    }
}