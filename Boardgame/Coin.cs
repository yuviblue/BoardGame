using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Boardgame
{
    class Coin
    {
        private Boardgame board;
        private float x, y, r; // circle coordinates and radius
        private float lastX, lastY; // previous coordinates
        private Paint p; // circle styling

        public Coin(float x, float y, float r, float lastX, float lastY, Color color)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.lastX = lastX;
            this.lastY = lastY;
            this.p = new Paint();
            p.Color = color;
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawCircle(this.x, this.y, this.r, this.p);
        }

        public bool DidUserTouchMe(float otherX, float otherY)
        {
            return ((otherX < r + x && otherX > r - x) && (otherY < r + y && otherY > r - y));
        }
    }
}