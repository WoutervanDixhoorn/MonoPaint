using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

using Input;

namespace MonoPaint.UI
{
    public class UITextBox : UIComponent
    {
        bool visible = true;

        int width;
        int height;

        mRectangle textboxRect;
        Action<string> textboxAction;

        StringBuilder typedText = new StringBuilder();

        bool focused;

        public bool Focused{
            get { return focused; }
            set { focused = value; }
        }

        public Action<string> OnEnter{
            set{ textboxAction = value; }
        }

        public UITextBox(int iX, int iY, int iWidth, int iHeight) : base(iX, iY)
        {
            focused = true;

            width = iWidth;
            height = iHeight;

            textboxRect = new mRectangle(iWidth, iHeight, new Color(205,205,205));
            textboxRect.DrawBorder = true;
            textboxRect.X = x; textboxRect.Y = y;
            
            MonoPaint.RegisterFocusedButtonForTextInput(OnInput);
        }

        
        public void Load()
        {
            textboxRect.Load();
        }

        public void Unload()
        {
            textboxRect.Unload();
            typedText = new StringBuilder();
            visible = false;
        }

        bool leftClicked = false;
        public void Update()
        {
            int mX = InputManager.CurrentMouseState.X;
            int mY = InputManager.CurrentMouseState.Y;

            if(IsOver(mX, mY))
            {
                textboxRect.Hovered = true;
            }else{
                textboxRect.Hovered = false;
            }

            //TODO: FIX
            if(InputManager.IsKeyPressed(Keys.Enter))
            {
                focused = false;
                MonoPaint.UnRegisterFocusedButtonForTextInput(OnInput);

                if(textboxAction == null){
                    Unload();
                }else{
                    textboxAction(typedText.ToString());//Let user handle text
                }
            }

            if(InputManager.IsPressed(MouseInput.LeftButton))
                leftClicked = true;

            if(InputManager.IsReleased(Input.MouseInput.LeftButton))
                leftClicked = false;
        }

        void OnInput(object sender, TextInputEventArgs e)
        {
            var k = e.Key;
            var c = e.Character;

            if(k == Keys.Back)
            {
                typedText.Remove(typedText.Length - 1, 1);
            }else{
                typedText.Append(c);
            }
        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            if(!visible)
                return;
                
            int padding = 3;
            SpriteFont font = ContentHandler.Instance.Content.Load<SpriteFont>("TempFont");

            int strLength = (int)font.MeasureString(typedText).X;
            if(strLength > textboxRect.Width)
            {
                textboxRect.Width = strLength + textboxRect.BorderSize + padding;
                textboxRect.Load();
            }

            textboxRect.Draw(iSpriteBatch);

            iSpriteBatch.DrawString(font, typedText, new Vector2(x + padding, y + padding), Color.Black);
        }

        public bool IsOver(int iX, int iY)
        {
            return textboxRect.Contains(iX, iY);
        }


    }
}