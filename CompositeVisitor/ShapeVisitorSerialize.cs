using MonoPaint.Decorator;
using MonoPaint.Shapes;

namespace MonoPaint.CompositeVisitor
{
    public class ShapeVisitorSerialize : IShapeVisitor
    {
        
        string output;

        int groupLevel = 0;

        string tabPre = "";

        public ShapeVisitorSerialize()
        {
            output = string.Empty;
        }

        public string GetShapeText()
        {
            return output;
        }

        public void Visit(aShape shape)
        {
            if(shape.GetType() == typeof(ShapeTextDecorator))
            {
                ShapeTextDecorator s = (ShapeTextDecorator)shape;
                Visit(s);
            }else if(shape.GetType() == typeof(ShapeComposite))
            {
                ShapeComposite s = (ShapeComposite)shape;
                Visit(s);
            }else if(shape.GetType() == typeof(mRectangle))
            {
                mRectangle s = (mRectangle)shape;
                Visit(s);
            }else if(shape.GetType() == typeof(mEllipse))
            {
                mEllipse s = (mEllipse)shape;
                Visit(s);
            }
        }

        public void Visit(ShapeTextDecorator shape)
        {
            string shapeString = "";
            ShapeTextDecorator dShape = (ShapeTextDecorator)shape;
            string text = dShape.GetText(TextPos.Top);
            text = text.Substring(0, text.Length - 1);
            shapeString += "ornament Top " + "\'"+text+"\'\n";
            output += shapeString;
            if(groupLevel > 0)
                output += tabPre + "\t" + "t " + shape.ShapeName + " " + shape.X + " " + shape.Y + " " + shape.Width + " " + shape.Height + "\n";
            else
                output += "t " +shape.ShapeName + " " + shape.X + " " + shape.Y + " " + shape.Width + " " + shape.Height + "\n";
        
        }

        public void Visit(ShapeComposite group)
        {
            for(int i = 0; i < groupLevel; i++)
                tabPre += "\t";

            output += tabPre + "group " + groupLevel + "\n";
            foreach(aShape s in group.GetChildren())
            {
                if(s.GetType() == typeof(ShapeComposite))
                    groupLevel++;
                output +=  tabPre + "\t";
                Visit(s);
            }
            groupLevel = 0;
        }

        public void Visit(mRectangle rectangle)
        {
            output += rectangle.ShapeName + " " + rectangle.X + " " + rectangle.Y + " " + rectangle.Width + " " + rectangle.Height + "\n";
        }

        public void Visit(mEllipse ellipse)
        {
            output += ellipse.ShapeName + " " + ellipse.X + " " + ellipse.Y + " " + ellipse.Width + " " + ellipse.Height + "\n";
        }

    }
}