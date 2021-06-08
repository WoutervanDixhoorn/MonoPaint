using Microsoft.Xna.Framework;

namespace MonoPaint.Graphics
{
    public class mRectangleDrawer : iShapeDrawer
    {
        private static mRectangleDrawer instance = null;

        public static mRectangleDrawer Instance
        {
            get { 
                if(instance == null){
                    instance = new mRectangleDrawer();
                } 

                return instance;
            }
        }

        private mRectangleDrawer()
        {
        }

        public Color[] GetData(int width, int height, Color color)
        {
            Color[] shapeData = new Color[(width * height)];

            for(int i = 0; i < shapeData.Length; i++)
                shapeData[i] = color;

            return shapeData;
        }

        public Color[] GetBorderData(int width, int height, int borderSize, Color borderColor)
        {
            int borderWidth = width+(borderSize*2), borderHeight = height+(borderSize*2);
            Color[] borderData = new Color[((borderWidth)*(borderHeight))];

            for(int i = 0; i < borderHeight; i++){
                for(int j = 0; j < borderWidth; j++){
                    if(j >= i*borderWidth - borderSize || j < borderSize){
                        borderData[i*borderWidth+j] = borderColor;
                    }else if(j >= borderWidth - borderSize){
                        borderData[i*borderWidth+j] = borderColor;
                    }else if(i >= borderHeight - borderSize || i < borderSize){
                        borderData[i*borderWidth+j] = borderColor;
                    }                 
                }
            }
                
            return borderData;
        }

    }
}