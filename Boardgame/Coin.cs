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
        private BoardGame board;
        private float x, y, r; // circle coordinates and radius
        private float lastX, lastY; // previous coordinates
        private Paint p; // circle styling

        public Coin(BoardGame board, float x, float y, float r, float lastX, float lastY)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.lastX = lastX;
            this.lastY = lastY;
            this.p = new Paint();
            this.board = board;
            p.Color = Color.Aqua;
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawCircle(this.x, this.y, this.r, this.p);
        }

        public bool DidUserTouchMe(float otherX, float otherY)
        {
            return DistanceFromCenter(otherX, otherY) <= r;
        }

        private float DistanceFromCenter(float otherX, float otherY)
        {
            return (float)Math.Sqrt( Math.Pow((otherX - x), 2) + Math.Pow((otherY - y), 2));
        }

        public void SetX(float x)
        {
            this.x = x;
        }

        public void SetY(float y)
        {
            this.y = y;
        }

        public Point GetCoinSquare()
        {
            return GetSquare((int)x, (int)y);
        }

        public static Point GetSquare(int x, int y)
        {
            var p = new Point();
            p.X = x / 180;
            p.Y = y / 180;
            return p;
        }

        public bool IsSquareBlack()
        {
            return IsSquareBlack((int)x, (int)y);
        }

        public static bool IsSquareBlack(int x, int y)
        {
            var p = GetSquare(x, y);
            if ((p.X + p.Y) % 2 == 0)
            {
                return false;
            }
            return true;
        }
    }
}