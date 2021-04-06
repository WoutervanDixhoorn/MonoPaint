using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Input;
using MonoPaint.Shapes;
using MonoPaint.Graphics;
namespace MonoPaint
{
    public class mPlayground
    {
        Screen screen;
        List<mCanvas> layers;

        int width = 640, height = 480;//TODO: Dont hard code width and height here

        public mPlayground()
        {
            screen = new Screen(width, height);

            layers = new List<mCanvas>();

            //Push first layer
            layers.Add(new mCanvas(width, height));
        }

        public void Load()
        {
            foreach(mCanvas c in layers)
            {
                c.Load();
            }
        }

        public void Unload()
        {
            foreach(mCanvas c in layers)
            {
                c.Unload();
            }
        }

        public void Update()
        {
            foreach(mCanvas c in layers)
            {
                c.Update();
            }

            UpdateInput();

        }

        public void Draw(SpriteBatch iSpriteBatch)
        {
            screen.Set();

            iSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.LinearClamp, DepthStencilState.Default,
            RasterizerState.CullNone);

            foreach(mCanvas c in layers)
            {
                c.Draw(iSpriteBatch);
            }

            iSpriteBatch.End();

            screen.Unset();
            screen.Present(iSpriteBatch);
        }

        int xPos1, yPos1, xPos2, yPos2 = 0;
        int rWidth, rHeight;
        bool leftClicked = false;

        
        enum eShape{
            Rect,
            Ellps
        }
        aShape shape;
        eShape curShape = eShape.Rect;

        void UpdateInput()
        {     
            if(InputManger.IsKeyPressed(Keys.Q))
            {
                if(curShape == eShape.Rect)
                {
                    curShape = eShape.Ellps;
                }else{
                    curShape = eShape.Rect;
                }
            }            

            if(!leftClicked && InputManger.IsPressed(MouseInput.LeftButton))
            {
                if(curShape == eShape.Rect)
                    shape = new mRectangle(1,1, Color.Red);

                if(curShape == eShape.Ellps)
                    shape = new mEllipse(1,1, Color.Red);

                xPos1 = InputManger.CurrentMouseState.X;
                yPos1 = InputManger.CurrentMouseState.Y;
                leftClicked = true;

                shape.X = xPos1; shape.Y = yPos1;
                shape.Load();
                layers[0].Shapes.Add(shape);//TODO: Change layer to current using layer


                Console.WriteLine("Start drawing at: [" + xPos1 + " | " + yPos1 + "]");
            }else if(leftClicked && InputManger.IsReleased(MouseInput.LeftButton))
            {
                leftClicked = false;

                shape.Width = Util.Clamp(rWidth, 1, width);
                shape.Height = Util.Clamp(rHeight, 1, height);

                layers[0].Load();

                Console.WriteLine("Added: " + shape.ToString());
            }else if(leftClicked)
            {
                xPos2 = InputManger.CurrentMouseState.X;
                yPos2 = InputManger.CurrentMouseState.Y;

                rWidth = xPos2 - xPos1; 
                rHeight = yPos2 - yPos1;
            
                shape.Width = Util.Clamp(rWidth, 1, width);
                shape.Height = Util.Clamp(rHeight, 1, height);

                layers[0].Load();
            }
        }

    }
}