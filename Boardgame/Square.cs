using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Boardgame
{
    class Square
    {
        private float x, y, w, h; // top left, bottom right, width, height;
        private Color color; // tile color
        private Paint p; // style brush
        private BoardGame board; // not yet built

        public Square(float x, float y, float w, float h, Color color, BoardGame board)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.board = board;
            this.p = new Paint();
            this.color = color;
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawRect(x, y, w, h, p);
        }
        
        public bool IsXAndYInSquare(float otherX, float otherY)
        {
            if (otherX < this.x && otherX < this.w && otherY < this.y && otherY < this.h)
                return true;
            return false;
        }
    }
}