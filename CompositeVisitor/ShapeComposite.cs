using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoPaint.CompositeVisitor;

namespace MonoPaint {

    public class ShapeComposite : aShape 
    {
        public override int X
        {
            get { return shapes[0].X; }
            set
            {
                int oldX = X;
                int deltaX = value - oldX;
                shapes[0].X = value;
                for(int i = 1; i < shapes.Count; i++)
                {
                    shapes[i].X += deltaX;
                }
            }
        }

        public override int Y
        {
            get { return shapes[0].Y; }
            set 
            {  
                int oldY = Y;
                int deltaY = value - oldY;
                shapes[0].Y = value;
                for(int i = 1; i < shapes.Count; i++)
                {
                    shapes[i].Y += deltaY;
                }
            }
        }

        public override int Width 
        {
             get { return (int)width; }
             set { setWidth(value); }
        }

        public override int Height 
        {
             get { return (int)height; }
             set { setHeight(value); }
        }

        //TODO: Extend functionality and make it funciton like a whole shape
        public override bool Selected
        {
            get { return shapes[0].Selected; }
            set { foreach(aShape s in shapes){ s.Selected = value; } }
        }

        public override bool Hovered
        {
            get { return shapes[0].Hovered; }
            set { foreach(aShape s in shapes){ s.Hovered = value; } }
        }

        public override bool DrawBorder
        {
            get { return shapes[0].DrawBorder; }
            set { foreach(aShape s in shapes){ s.DrawBorder = value; } }
        }

        
        public override bool Visible
        {
            get { return shapes[0].Visible; }
            set { foreach(aShape s in shapes){ s.Visible = value; } }
        }

        public override Color Color{
            get { return shapes[0].Color; }
            set { foreach(aShape s in shapes){ s.Color = value; } }
        }
    
        public override Color BorderColor{
            set { foreach(aShape s in shapes){ s.BorderColor = value; } }
        }
        
        public override int BorderSize
        {
            get { return shapes[0].BorderSize; }
            set { foreach(aShape s in shapes){ s.BorderSize = value; } }
        }

        List<aShape> shapes;
        
        Vector4 dimensions;

        int width, height;

        public ShapeComposite()
        {
            ShapeName = "group";        
            shapes = new List<aShape>();
        }

        public List<aShape> GetChildren()
        {
            return shapes;
        }


        public void Add(aShape child){
            shapes.Add(child);
            dimensions = calculateDimensions();
            width = (int)dimensions.Y - (int)dimensions.X;
            height = (int)dimensions.W - (int)dimensions.Z;
        }

        public void Add(List<aShape> children){
            shapes.AddRange(children);
            dimensions = calculateDimensions();
            width = (int)dimensions.Y - (int)dimensions.X;
            height = (int)dimensions.W - (int)dimensions.Z;
        }


        public void Remove(aShape child){

            shapes.Remove(child);

        }

        public override void Load(){

            foreach(aShape child in shapes){
                child.Load();
            }
            dimensions = calculateDimensions();
        }


        public override void Unload(){

            foreach(aShape child in shapes){
                child.Unload();
            }
        }

        public override void Draw(SpriteBatch iSpriteBatch, float iAlpha = 1)
        {
            calculateDimensions();
            foreach(aShape child in shapes){
                child.Visible = true; //NOTE: Here to quickly fix bug with groups moving
                child.Draw(iSpriteBatch);
            }
        }

        public override bool Contains(int iX, int iY)
        {
            foreach(aShape child in shapes){
                if(child.Contains(iX, iY))
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
            shapeVisitor.Visit(this);
        }

        void setWidth(int newWidth)
        {
            int xOffset = (newWidth - Width)/2;
            shapes[0].Width += xOffset;
            for(int i = 1; i < shapes.Count; i++)
            {
                shapes[i].Width += xOffset;
                shapes[i].X += xOffset;
            }
        }

        void setHeight(int newHeight)
        {
            int yOffset = (newHeight - Height)/2;
            shapes[0].Height += yOffset;
            for(int i = 1; i < shapes.Count; i++)
            {
                shapes[i].Height += yOffset;
                shapes[i].Y += yOffset;
            }
        }

        public Vector4 calculateDimensions()
        {
            int leftX = int.MaxValue;
            int topY = int.MaxValue;

            int rightX = int.MinValue;
            int bottomY = int.MinValue;
            
            foreach(aShape s in shapes)
            {
                // if(s.GetType() == typeof(ShapeComposite)){
                //     ShapeComposite temp = (ShapeComposite)s;
                //     Vector4 groupDim = temp.calculateDimensions();
                //     int tempLeftX = (int)groupDim.X;
                //     int tempRightX = (int)groupDim.Y;
                //     int tempTopY = (int)groupDim.Z;
                //     int tempBottomY = (int)groupDim.W;

                //     if(tempLeftX < leftX)
                //         leftX = s.X;
                //     if(tempRightX > rightX)
                //         rightX = tempRightX;
                    
                //     if(tempTopY < topY)
                //         topY = tempTopY;
                //     if(tempBottomY > bottomY)
                //         bottomY = tempBottomY;
                // }else{
                    
                    if(s.X < leftX)
                        leftX = s.X;
                    if(s.X + s.Width > rightX)
                        rightX = s.X + s.Width;
                    
                    if(s.Y < topY)
                        topY = s.Y;
                    if(s.Y + s.Height > bottomY)
                        bottomY = s.Y + s.Height;

                // }
            }

            return new Vector4(leftX, rightX, topY, bottomY);
        }
        
    }

}