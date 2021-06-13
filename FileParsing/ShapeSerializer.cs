using System;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using System.IO;
using System.Threading.Tasks;

using MonoPaint.Shapes;
using MonoPaint.Decorator;
using MonoPaint.CompositeVisitor;

namespace MonoPaint.FileParsing
{
    public class ShapeSerializer
    {

        public static async Task Serialize(List<aShape> iShapes, string path)
        {
            string shapeStrings = string.Empty;

            ShapeVisitorSerialize shapeSerializer = new ShapeVisitorSerialize();

            foreach (aShape s in iShapes)
                s.Accept(shapeSerializer);

            shapeStrings = shapeSerializer.GetShapeText();
            await File.WriteAllTextAsync(path, shapeStrings);
        }

        public static async Task<List<aShape>> Deserialize(string path)
        {
            List<aShape> shapes = new List<aShape>();

            Regex rxGroup = new Regex(@"(\t*)(group)\s(?<groupNumber>\d+)(\n\r|\n|\r)(?<shapes>(((\t+)(?<type>ornament)\s(Top|Botton|Left|Right)\s(\')(?<text>.*)(\')|(\t+)(?<isTextShape>t\s)*(?<type>rectangle|ellipse)\s(?<X>\d+)\s(?<Y>\d+)\s(?<width>\d+)\s(?<height>\d+))(\n\r|\n|\r))*)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rxTextShape = new Regex(@"(ornament)\s(?<textpos>Top|Botton|Left|Right)\s(\')(?<text>.*)(\')(\n\r|\n|\r)(t\s)(?<shape>rectangle|ellipse)\s(?<X>\d+)\s(?<Y>\d+)\s(?<width>\d+)\s(?<height>\d+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rxShape = new Regex(@"(?<isGrouped>(\t*))(?<isTextShape>t\s)*(?<shape>rectangle|ellipse)\s(?<X>\d+)\s(?<Y>\d+)\s(?<width>\d+)\s(?<height>\d+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (!File.Exists(path))
            {
                throw new System.ArgumentException("Filepath does not exist!");
            }

            string shapeStrings = File.ReadAllText(path);
            
            MatchCollection matchesGroups = rxGroup.Matches(shapeStrings);

            ShapeComposite group = new ShapeComposite();
            ShapeComposite underGroup = null;
            foreach (Match m in matchesGroups)
            {
                GroupCollection groups = m.Groups;

                string gr = groups["shapes"].Value;

                int.TryParse(groups["groupNumber"].Value, out int groupNumber);
                if(groupNumber > 0)
                {
                    ShapeComposite newGroup = new ShapeComposite();
                    List<aShape> temp = deserializeGroupShapes(gr);
                    foreach(aShape s in temp)
                    {
                        newGroup.Add(s);
                    }
                    if(underGroup != null)
                    {
                        underGroup.Add(newGroup);
                        underGroup = newGroup;
                    }else{
                        group.Add(newGroup);
                        underGroup = newGroup;
                    }
                }else{
                    List<aShape> temp = deserializeGroupShapes(gr);
                    foreach(aShape s in temp)
                    {
                        group.Add(s);
                    }
                }
            }
            shapes.Add(group);

            MatchCollection matchesTextShapes = rxTextShape.Matches(shapeStrings);

            foreach (Match m in matchesTextShapes)
            {
                GroupCollection groups = m.Groups;

                aShape temp = null;

                TextPos textPos = StringToTextpos(groups["textpos"].Value);
                string text = groups["text"].Value;

                //Parse strings to int
                int.TryParse(groups["X"].Value, out int x);
                int.TryParse(groups["Y"].Value, out int y);
                int.TryParse(groups["width"].Value, out int width);
                int.TryParse(groups["height"].Value, out int height);

                if (groups["shape"].Value == "rectangle")
                    temp = new mRectangle(width, height, Color.Red);
                else if (groups["shape"].Value == "ellipse")
                    temp = new mEllipse(width, height, Color.Red);

                temp.X = x;
                temp.Y = y;

                ShapeTextDecorator decTemp = new ShapeTextDecorator(temp);
                decTemp.AddText(text, textPos);

                if (temp != null)
                {
                    temp.Load();
                    shapes.Add(decTemp);
                }
                else
                {
                    return shapes;
                }
            }

            MatchCollection matchesShapes = rxShape.Matches(shapeStrings);

            foreach (Match m in matchesShapes)
            {
                GroupCollection groups = m.Groups;

                //Check on group and text
                if(groups["isTextShape"].Value != string.Empty || groups["isGrouped"].Value != string.Empty)
                    continue;

                aShape temp = null;

                //Parse strings to int
                int.TryParse(groups["X"].Value, out int x);
                int.TryParse(groups["Y"].Value, out int y);
                int.TryParse(groups["width"].Value, out int width);
                int.TryParse(groups["height"].Value, out int height);

                if (groups["shape"].Value == "rectangle")
                    temp = new mRectangle(width, height, Color.Red);
                else if (groups["shape"].Value == "ellipse")
                    temp = new mEllipse(width, height, Color.Red);

                temp.X = x;
                temp.Y = y;

                if (temp != null)
                {
                    temp.Load();
                    shapes.Add(temp);
                }
                else
                {
                    return shapes;
                }
            }

            return shapes;
        }

        static List<aShape> deserializeGroupShapes(string shapes)
        {
            Regex rxTextShape = new Regex(@"(\t*)(ornament)\s(?<textpos>Top|Botton|Left|Right)\s(\')(?<text>.*)(\')(\n\r|\n|\r)(\t*)t\s(?<shape>rectangle|ellipse)\s(?<X>\d+)\s(?<Y>\d+)\s(?<width>\d+)\s(?<height>\d+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rxShape = new Regex(@"(\t*)(?<isTextShape>t\s)*(?<shape>rectangle|ellipse)\s(?<X>\d+)\s(?<Y>\d+)\s(?<width>\d+)\s(?<height>\d+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matchesTextShapes = rxTextShape.Matches(shapes);
            List<aShape> returnShapes = new List<aShape>();

            foreach (Match m in matchesTextShapes)
            {
                GroupCollection groups = m.Groups;

                aShape temp = null;

                TextPos textPos = StringToTextpos(groups["textpos"].Value);
                string text = groups["text"].Value;

                //Parse strings to int
                int.TryParse(groups["X"].Value, out int x);
                int.TryParse(groups["Y"].Value, out int y);
                int.TryParse(groups["width"].Value, out int width);
                int.TryParse(groups["height"].Value, out int height);

                if (groups["shape"].Value == "rectangle")
                    temp = new mRectangle(width, height, Color.Red);
                else if (groups["shape"].Value == "ellipse")
                    temp = new mEllipse(width, height, Color.Red);

                temp.X = x;
                temp.Y = y;

                ShapeTextDecorator decTemp = new ShapeTextDecorator(temp);
                decTemp.AddText(text, textPos);

                if (temp != null)
                {
                    temp.Load();
                    returnShapes.Add(decTemp);
                }
            }

            MatchCollection matchesShapes = rxShape.Matches(shapes);

            foreach (Match m in matchesShapes)
            {
                GroupCollection groups = m.Groups;

                aShape temp = null;

                if(groups["isTextShape"].Value != string.Empty)
                    continue;

                //Parse strings to int
                int.TryParse(groups["X"].Value, out int x);
                int.TryParse(groups["Y"].Value, out int y);
                int.TryParse(groups["width"].Value, out int width);
                int.TryParse(groups["height"].Value, out int height);

                if (groups["shape"].Value == "rectangle")
                    temp = new mRectangle(width, height, Color.Red);
                else if (groups["shape"].Value == "ellipse")
                    temp = new mEllipse(width, height, Color.Red);

                temp.X = x;
                temp.Y = y;

                if (temp != null)
                {
                    temp.Load();
                    returnShapes.Add(temp);
                }
            }

            return returnShapes;
        }

        public static TextPos StringToTextpos(string textPos)
        {
            TextPos pos = TextPos.Top;

            switch (textPos)
            {
                case "Top":
                    pos = TextPos.Top;
                    break;
                case "Left":
                    pos = TextPos.Left;
                    break;
                case "Bottom":
                    pos = TextPos.Bottom;
                    break;
                case "Right":
                    pos = TextPos.Right;
                    break;
            }

            return pos;
        }


    }
}