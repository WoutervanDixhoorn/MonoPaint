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
            if(shape.GetType() == typeof(ShapeComposite)){
                ShapeComposite temp = (ShapeComposite)shape;
                textShape = new ShapeTextDecorator(temp.GetChildren()[0]);
                textShape.AddText(text, textPos);
                temp.GetChildren()[temp.GetChildren().FindIndex(ind=>ind.Equals(temp.GetChildren()[0]))] = textShape;
                
                playground.AddShape(temp);
                playground.RemoveShape(shape);
            }else{
                textShape.AddText(text, textPos);
                
                playground.AddShape(textShape);
                playground.RemoveShape(shape);
            }

        }

        public void Undo()
        {
            playground.AddShape(shape);
            playground.RemoveShape(textShape);
        }
    }
}