using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using System.IO;
using System.Threading.Tasks;

using MonoPaint.Shapes;

namespace MonoPaint.FileParsing
{
    public class ShapeSerializer
    {

        public static async Task Serialize(List<aShape> iShapes, string path)
        {
            string shapeStrings = string.Empty;

            foreach(aShape s in iShapes)
            {
                shapeStrings += s.ShapeName + " " + s.X + " " + s.Y + " " + s.Width + " " + s.Height + "\n";
            }

            await File.WriteAllTextAsync(path, shapeStrings);
        }

        public static async Task<List<aShape>> Deserialize(string path)
        {
            List<aShape> shapes = new List<aShape>();

            Regex rx = new Regex(@"(?<shape>rectangle|ellipse)\s(?<X>\d+)\s(?<Y>\d+)\s(?<width>\d+)\s(?<height>\d+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if(!File.Exists(path))
            {
                throw new System.ArgumentException("Filepath does not exist!");
            }

            string shapeStrings = File.ReadAllText(path);
            MatchCollection matches = rx.Matches(shapeStrings);

            foreach(Match m in matches)
            {
                GroupCollection groups = m.Groups;

                aShape temp = null;

                //Parse strings to int
                int.TryParse(groups["X"].Value, out int x);
                int.TryParse(groups["Y"].Value, out int y);
                int.TryParse(groups["width"].Value, out int width);
                int.TryParse(groups["height"].Value, out int height);

                if(groups["shape"].Value == "rectangle")
                    temp = new mRectangle(width, height, Color.Red);
                else if(groups["shape"].Value == "ellipse")
                    temp = new mEllipse(width, height, Color.Red);
                
                temp.X = x;
                temp.Y = y; 

                if(temp != null){
                    temp.Load();
                    shapes.Add(temp);
                }else{
                    return shapes;
                }
            }

            return shapes;
        }

    }
}