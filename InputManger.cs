using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public enum MouseInput
    {
        None,
        LeftButton,
        MiddleButton,
        RightButton,
        Button1,
        Button2
    }
    
    public class InputManger
    {
        static bool buttonClicked = false;
     
        static KeyboardState currentKeyboardState;
        static KeyboardState previousKeyboardState;
        public static KeyboardState CurrentKeyboardState{
            get{ return currentKeyboardState; }
        }

        static MouseState currentMouseState;
        static MouseState previousMouseState;
        public static MouseState CurrentMouseState{
            get{ return currentMouseState; }
        }

        public static bool IsKeyPressed(Keys key){
            return (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key));
        }
        public static bool IsKeyDown(Keys key){
            return (currentKeyboardState.IsKeyDown(key));
        }
        public static bool IsKeyReleased(Keys key){
            return (currentKeyboardState.IsKeyUp(key)) && previousKeyboardState.IsKeyDown(key);
        }

        public static bool IsPressed(MouseInput input)
        {
            return IsMousePressed(currentMouseState, input);
        }
                public static bool IsReleased(MouseInput input)
        {
            return IsMouseReleased(currentMouseState, input);
        }
        static bool IsMousePressed(MouseState state, MouseInput input)
        {
            switch (input)
            {
                case MouseInput.LeftButton:
                   { buttonClicked = true; return state.LeftButton == ButtonState.Pressed; }
                case MouseInput.MiddleButton:
                   { buttonClicked = true; return state.MiddleButton == ButtonState.Pressed; }
                case MouseInput.RightButton:
                   { buttonClicked = true; return state.RightButton == ButtonState.Pressed; }
                case MouseInput.Button1:
                   { buttonClicked = true; return state.XButton1 == ButtonState.Pressed; }
                case MouseInput.Button2:
                   { buttonClicked = true; return state.XButton2 == ButtonState.Pressed; }
            }
            return false;
        }

        static bool IsMouseReleased(MouseState state, MouseInput input)
        {
            switch (input)
            {
                case MouseInput.LeftButton:
                    { return state.LeftButton == ButtonState.Released; }
                case MouseInput.MiddleButton:
                    { return state.MiddleButton == ButtonState.Released; }
                case MouseInput.RightButton:
                    { return state.RightButton == ButtonState.Released; }
                case MouseInput.Button1:
                    { return state.XButton1 == ButtonState.Released; }
                case MouseInput.Button2:
                    { return state.XButton2 == ButtonState.Released; }
            }
            return false;
        }

        public static Point GetMousePosition()
        {
            return currentMouseState.Position;
        }
        public static bool IsMouseMoved()
        {
            return currentMouseState.X != previousMouseState.X || currentMouseState.Y != previousMouseState.Y;
        }
        public static int GetMouseScroll()
        {
            return currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
        }

        public static void Update()
        {
            // Get keyboard states
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // Get mouse states
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

    }
}