using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPaint {

class CompoundGraphic : aShape 
{

    List<aShape> shapes;
    


    public CompoundGraphic(){

         shapes = new List<aShape>();

    }

    public void add(aShape child){
        shapes.Add(child);
    }

    public void remove(aShape child){

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
            shapeTexture = new Texture2D(ContentHandler.Instance.Graphics, width, height); 
            shapeData = new Color[(shapeTexture.Width * shapeTexture.Height)];

            for(int i = 0; i < height; i++){
                for(int j = 0; j < width; j++){
                    if(j >= i*width - 1 || j < 1){
                        shapeData[i*width+j] = borderColor;
                    }else if(j >= width - 1){
                        shapeData[i*width+j] = borderColor;
                    }else if(i >= height - 1 || i < 1){
                        shapeData[i*width+j] = borderColor;
                    }                 
                }
            }

            shapeTexture.SetData(shapeData);
        }




}




}