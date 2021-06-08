using Microsoft.Xna.Framework;

namespace MonoPaint.Graphics
{
    public interface iShapeDrawer
    {   
        Color[] GetData(int width, int height, Color color);
        Color[] GetBorderData(int width, int height, int borderSize, Color color);
    }
}