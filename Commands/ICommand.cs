namespace MonoPaint
{
    public interface ICommand
    {  
        void Execute();
        void Undo();
    }
}