namespace MonoPaint.Commands
{
    public class ResizeCommand : ICommand
    {
        aShape currentShape;
        int oldWidth, oldHeight, oldX, oldY;
        int newWidth, newHeight, newX, newY;

        public ResizeCommand(aShape iShape, int iWidth, int iHeight, int iX, int iY)
        {
            currentShape = iShape;
            oldWidth = iShape.Width;
            oldHeight = iShape.Height;
            oldX = iShape.X;
            oldY = iShape.Y;
            
            newX = iX;
            newY = iY;

            newWidth = iWidth;
            newHeight = iHeight;
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

            newWidth = iWidth;
            newHeight = iHeight;
        }

        public void Execute()
        {
            currentShape.Width = newWidth;
            currentShape.Height = newHeight;
            currentShape.X = newX;
            currentShape.Y = newY;
            currentShape.Load();
        }

        public void Undo()
        {
            currentShape.Width = oldWidth;
            currentShape.Height = oldHeight;
            currentShape.X = oldX;
            currentShape.Y = oldY;
            currentShape.Load(); 
        }
    }
}