using MonoPaint.Decorator;

namespace MonoPaint.Commands
{
    public class AddTextCommand : ICommand
    {
        mPlayground playground;
        aShape shape;
        ShapeTextDecorator textShape;
        string text;
        TextPos textPos;

        public AddTextCommand(mPlayground iPlayground, aShape iShape, string iText, TextPos pos)
        {
            playground = iPlayground;
            shape = iShape;
            text = iText;
            textPos = pos;
        }

        public void Execute()
        {
            textShape = new ShapeTextDecorator(shape);
            textShape.AddText(text, textPos);
            playground.AddShape(textShape);
            playground.RemoveShape(shape);
        }

        public void Undo()
        {
            playground.AddShape(shape);
            playground.RemoveShape(textShape);
        }
    }
}