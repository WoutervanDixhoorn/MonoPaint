using System;

namespace MonoPaint.UI
{
    public class UIBasics
    {
        
        static UIBasics instance;

        public UIBasics Instance
        {
            get { 
                if(instance == null)
                {
                    instance = new UIBasics();
                }

                return instance;
            }
        }

        public static UITextBox BasicTextbox(int x, int y, Action<string> onEnter)
        {
            UITextBox BasicTextBox = new UITextBox(x,y,100,30);
            BasicTextBox.Load();

            BasicTextBox.OnEnter = onEnter;

            return BasicTextBox;
        }

    }
}