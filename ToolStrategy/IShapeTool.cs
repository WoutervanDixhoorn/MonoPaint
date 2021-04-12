using Microsoft.Xna.Framework.Graphics;
namespace MonoPaint.ToolStrategy
{
    public interface IShapeTool
    {
        void Update();
        void Draw(SpriteBatch iSpriteBatch);

        void Reset();
    }
}