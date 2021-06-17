using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Input;

namespace MonoPaint.UI
{
    public class UIButton : UIComponent
    {
        int width;
        int height;

        string buttonText;

        public string Text{
            set{ buttonText = value; }
        }

        mRectangle buttonRect;

        public Color Color {
            set { buttonRect.Color = value; }
        }

        public Color BorderColor {
            set { buttonRect.BorderColor = value; }
        }

        public bool Border{
            set { buttonRect.DrawBorder = value; }
        }

        Action buttonAction;

        public Action OnPress{
            set{ buttonAction = value; }
        }

        public UIButton(int iX, int iY, int iWidth, int iHeight) : base(iX, iY)
        {
            width = iWidth;
            height = iHeight;

            buttonText = "Button";

            buttonRect = new mRectangle(iWidth, iHeight, Color.Aqua);
            buttonRect.X = iX; buttonRect.Y = iY;
        }

        public void Load()
        {
            buttonRect.Load();
        }

        public void Unload()
        {
            buttonRect.Unload();
        }

        bool pressed = false;
        public void Update()
        {
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            if(IsOver(mX, mY))
            {
                buttonRect.Hovered = true;
            }else{
                buttonRect.Hovered = false;
            }

            if(!pressed && InputManager.IsPressed(MouseInput.LeftButton) && buttonRect.Hovered)
            {
                pressed = true;
                buttonAction();
            }else if(pressed && InputManager.IsReleased(MouseInput.LeftButton))
            {
                pressed = false;
            }
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            SpriteFont font = ContentHandler.Instance.Content.Load<SpriteFont>("TempFont");
            buttonRect.Draw(iSpriteBatch);

            int textX = (int)(width - (font.MeasureString(buttonText).X)) / 2;
            int textY = (int)(height - (font.MeasureString(buttonText).Y)) / 2;

            iSpriteBatch.DrawString(font, buttonText, new Vector2(x + textX, y + textY), Color.Black);
        }

        public bool IsOver(int iX, int iY)
        {
            return buttonRect.Contains(iX, iY);
        }

    }
}