using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoPaint.CompositeVisitor;

namespace MonoPaint {

    class ShapeComposite : aShape 
    {

        //TODO: Extend functionality and make it funciton like a whole shape

        List<aShape> shapes;
        
        public ShapeComposite(){

            shapes = new List<aShape>();

        }

        public void Add(aShape child){
            shapes.Add(child);
        }

        public void Remove(aShape child){

            shapes.Remove(child);

        }

        public override void Load(){

            foreach(aShape child in shapes){
                child.Load();
            }
        }


        public override void Unload(){

            foreach(aShape child in shapes){
                child.Unload();
            }
        }

        public override void Draw(SpriteBatch iSpriteBatch, float iAlpha = 1)
        {
            foreach(aShape child in shapes){
                child.Draw(iSpriteBatch);
            }
        }

        public override bool Contains(int iX, int iY)
        {
            if(iX > X && iX <  X + Width &&
                iY > Y &&  iY < Y + Height)
            {
                return true;
            }
            return false;
        }


        public override void LoadWhileDrawing()
        {
            //Nothing
        }

        public override void Accept(IShapeVisitor shapeVisitor)
        {
            foreach(aShape s in shapes)
            {
                shapeVisitor.Visit(s);
            }   
        }


    }

}