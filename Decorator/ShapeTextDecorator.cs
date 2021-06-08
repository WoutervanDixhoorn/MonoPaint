using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoPaint.Decorator
{
    public enum TextPos
    {
        Top = 0,
        Left = 1,
        Bottom = 2,
        Right = 3
    }

    public class ShapeTextDecorator : aShapeDecorator
    {   
        private string topText = string.Empty;
        private string leftText = string.Empty;
        private string bottomText = string.Empty;
        private string rightText = string.Empty;


        private SpriteFont font = ContentHandler.Instance.Content.Load<SpriteFont>("TempFont");

        public ShapeTextDecorator(aShape iShape) : base(iShape)
        {
        }

        public void AddText(string text, TextPos? pos = TextPos.Top)
        {
            switch(pos)
            {
                case TextPos.Top:
                    topText = text;
                    break;
                case TextPos.Left:
                    leftText = text;
                    break;
                case TextPos.Bottom:
                    bottomText = text;
                    break;
                case TextPos.Right:
                    rightText = text;
                    break;
            }
           
        }

        public override void OnDraw(SpriteBatch iSpriteBatch)
        {
            Vector2 topDim = font.MeasureString(topText);
            Vector2 leftDim = font.MeasureString(leftText);
            Vector2 bottomDim = font.MeasureString(bottomText);
            Vector2 rightDim = font.MeasureString(rightText);

            iSpriteBatch.DrawString(font, topText, new Vector2(X, Y - topDim.Y), Color.Black);
            iSpriteBatch.DrawString(font, leftText, new Vector2(X, Y), Color.Black, (float)Math.PI/2, new Vector2(0,0), 1, 0, 0);
            iSpriteBatch.DrawString(font, bottomText, new Vector2(X, Y + Height), Color.Black);
            iSpriteBatch.DrawString(font, rightText, new Vector2(X + Width + rightDim.Y, Y), Color.Black, (float)Math.PI/2, new Vector2(0,0), 1, 0, 0);
        }

    }
}