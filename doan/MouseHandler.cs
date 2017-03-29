using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace doan
{
    class MouseHandler
    {
        string mouseposx;
        string mouseposy;
        SpriteFont font;
        Dictionary<MouseEvent, Action<Vector2>> eventDictionary;
        StringBuilder registedaction = new StringBuilder();

        public void Initialize()
        {
            eventDictionary = new Dictionary<MouseEvent, Action<Vector2>>();
        }

        /// <summary>
        /// bind an action to an mouse event
        /// </summary>
        /// <param name="mouseEvent"></param>
        /// <param name="action"></param>
        public void AddEvent(MouseEvent mouseEvent,Action<Vector2> action)
        {
            if (!eventDictionary.ContainsKey(mouseEvent))
            {
                eventDictionary.Add(mouseEvent, action);
            }
        }

        public void SetFont(SpriteFont font)
        {
            this.font = font;
        }

        public void Update()
        {
            MouseState current_mouse = Mouse.GetState();
            MouseEvent current_mouse_event = new MouseEvent(current_mouse);

            foreach (var action in eventDictionary)
            {
                if (action.Key.Equals(current_mouse_event))
                {
                    action.Value.Invoke(new Vector2(current_mouse.X, current_mouse.Y));
                }
            }

            {
                // The mouse x and y positions are relative to the
                // upper-left corner of the game window.
                StringBuilder temp = new StringBuilder();
                temp.Append("x: ");
                temp.Append(current_mouse.X);
                mouseposx = temp.ToString();
                temp.Clear();
                temp.Append("y: ");
                temp.Append(current_mouse.Y);
                mouseposy = temp.ToString();
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, mouseposx, new Vector2(10, 10), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Gui);
            spriteBatch.DrawString(font, mouseposy, new Vector2(10, 30), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Gui);
            //spriteBatch.DrawString(font, registedaction, new Vector2(120, 520), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Gui);
        }
    }

    class MouseEvent
    {
        int X;
        int Y;
        MouseButtonState mouseButtonState;
        Rectangle area;

        public MouseEvent()
        {
            X = 0;
            Y = 0;
            mouseButtonState = MouseButtonState.Void;
            area = Rectangle.Empty;
        }
        public MouseEvent(MouseState mouseState)
        {
            X = mouseState.X;
            Y = mouseState.Y;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mouseButtonState = MouseButtonState.Mouse1;
                return;
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                mouseButtonState = MouseButtonState.Mouse2;
                return;
            }
            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                mouseButtonState = MouseButtonState.Mouse3;
                return;
            }
            mouseButtonState = MouseButtonState.Release;
        }

        /// <param name="x">set to -1 to ignore x</param>
        /// <param name="y">set to -1 to ignore y</param>
        public MouseEvent(int x,int y, MouseButtonState mousebuttonstate)
        {
            X = x;
            Y = y;
            area = Rectangle.Empty;
            mouseButtonState = mousebuttonstate;
        }

        public MouseEvent(Rectangle area, MouseButtonState mousebuttonstate)
        {
            X = -1;
            Y = -1;
            this.area = area;
            mouseButtonState = mousebuttonstate;
        }

        public override bool Equals(object obj)
        {// Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            MouseEvent p = (MouseEvent)obj;
            if (area != Rectangle.Empty)
            {
                if (area.Contains(p.X, p.Y))
                    return ((mouseButtonState == p.mouseButtonState) || (mouseButtonState == MouseButtonState.Void));
                else return false;
            }
            bool Xequal = ((X == p.X) || (X == -1));
            bool Yequal = ((Y == p.Y) || (Y == -1));
            bool mousestateequal = ((mouseButtonState == p.mouseButtonState) || (X != -1 && Y != -1 && (mouseButtonState == MouseButtonState.Void)));
            return (Xequal && Yequal) && mousestateequal;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 99) + X.GetHashCode();
            hash = (hash * 99) + Y.GetHashCode();
            hash = (hash * 99) + mouseButtonState.GetHashCode();
            hash = (hash * 99) + area.GetHashCode();
            return hash;
        }

        public static MouseEvent Mouse1Pressed(int X=-1,int Y=-1)
        {
            return new MouseEvent(X, Y, MouseButtonState.Mouse1);
        }
        public static MouseEvent Mouse1Pressed(Rectangle area)
        {
            return new MouseEvent(area, MouseButtonState.Mouse1);
        }

        public static MouseEvent Mouse2Pressed(int X = -1, int Y = -1)
        {
            return new MouseEvent(X, Y, MouseButtonState.Mouse2);
        }
        public static MouseEvent Mouse2Pressed(Rectangle area)
        {
            return new MouseEvent(area, MouseButtonState.Mouse2);
        }

        public static MouseEvent Mouse3Pressed(int X = -1, int Y = -1)
        {
            return new MouseEvent(X, Y, MouseButtonState.Mouse3);
        }
        public static MouseEvent Mouse3Pressed(Rectangle area)
        {
            return new MouseEvent(area, MouseButtonState.Mouse3);
        }
        
        public static MouseEvent MouseOn(int x,int y)
        {
            return new MouseEvent(x, y, MouseButtonState.Void);
        }

        public static MouseEvent MouseOn(Rectangle area)
        {
            return new MouseEvent(area, MouseButtonState.Void);
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append(mouseButtonState.ToString());
            output.Append('|');
            if (area == Rectangle.Empty)
            {
                output.Append(X);
                output.Append(';');
                output.Append(Y);
            }
            else
            {
                output.Append(area.ToString());
            }
            return output.ToString();
        }
    }
}