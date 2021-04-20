using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Collections.Generic;

using System.IO;
using System.Threading.Tasks;

using MonoPaint.Shapes;

namespace MonoPaint.FileParsing
{
    public class ShapeSerializer
    {

        public static async Task Serialize(List<aShape> iShapes)
        {
            string shapeStrings = string.Empty;

            foreach(aShape s in iShapes)
            {
                shapeStrings += s.ShapeName + " " + s.X + " " + s.Y + " " + s.Width + " " + s.Height + "\n";
            }

            await File.WriteAllTextAsync("Saves/Shapes.txt", shapeStrings);
        }

        public static async Task<List<aShape>> Deserialize()
        {
            List<aShape> shapes = new List<aShape>();

            Regex rx = new Regex(@"(rectangle|ellipse)\s(\d+)\s(\d+)\s(\d+)\s(\d+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string shapeStrings = File.ReadAllText("Saves/Shapes.txt");

            MatchCollection matches = rx.Matches(shapeStrings);

            foreach(Match m in matches)
            {
                GroupCollection groups = m.Groups;

                aShape temp = null;

                int.TryParse(groups[2].Value, out int x);
                int.TryParse(groups[3].Value, out int y);
                int.TryParse(groups[4].Value, out int width);
                int.TryParse(groups[5].Value, out int height);
                if(groups[1].Value == "rectangle")
                {
                    temp = new mRectangle(width, height);
                    temp.X = x;
                    temp.Y = y;
                }else if(groups[1].Value == "ellipse")
                {
                    temp = new mEllipse(width, height);
                    temp.X = x;
                    temp.Y = y; 
                }
                
                if(temp != null){
                    temp.Load();
                    shapes.Add(temp);
                }else{
                    return shapes;
                }
            }

            System.Console.WriteLine("Contents of WriteText.txt = {0}", shapeStrings);

            return shapes;
        }

    }
}