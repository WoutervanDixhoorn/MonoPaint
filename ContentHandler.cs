using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPaint
{
    public class ContentHandler
    {
        
        //Quick singleton
        private static ContentHandler instance;

        public static ContentHandler Instance{
            get{
                if(instance == null){
                    instance = new ContentHandler();
                }
                return instance;
            }
        }

        public ContentManager Content{private set; get;}
        public GraphicsDevice Graphics{private set; get;}

        public ContentHandler(){

        }

        public void Load(ContentManager iContent, GraphicsDevice iGraphics){
            this.Content = new ContentManager(iContent.ServiceProvider, "Content");
            this.Graphics = iGraphics;
        }

    }
}