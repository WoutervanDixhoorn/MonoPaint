namespace MonoPaint.UI
{
    public abstract class UIComponent
    {
        protected int x;
        protected int y;

        public UIComponent(int iX, int iY)
        {
            x = iX;
            y = iY; 
        }
    }
}