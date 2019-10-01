using System;
using System.Timers;
using Android.Graphics;

namespace Boardgame
{
    class Coin
    {
        private BoardGame board;
        private Paint p; // circle styling
        private Timer t;

        public float PX { get; set; }
        public float PY { get; set; }
        public float R { get; set; }

        public Coin(BoardGame board, float x, float y)
        {
            PX = x;
            PY = y;
            R = 80;
            p = new Paint();
            this.board = board;
            p.Color = Color.Gold;
            SetTimer();
        }

        public bool OnTouchEvent(float otherX, float otherY)
        {
            if(!DidUserTouchMe(otherX, otherY))
            {
                return false;
            }

            t.Stop();
            return true;
        }
        public void Draw(Canvas canvas)
        {
            canvas.DrawCircle(this.PX, this.PY, this.R, this.p);
        }
        public bool DidUserTouchMe(float otherX, float otherY)
        {
            return DistanceFromCenter(otherX, otherY) <= R;
        }

        private float DistanceFromCenter(float otherX, float otherY)
        {
            return (float)Math.Sqrt( Math.Pow((otherX - PX), 2) + Math.Pow((otherY - PY), 2));
        }

        public void SetX(float x)
        {
            this.PX = x;
        }

        public void SetY(float y)
        {
            this.PY = y;
        }

        public Point GetCoinSquare()
        {
            return GetSquare((int)PX, (int)PY);
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
            return IsSquareBlack((int)PX, (int)PY);
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

        public void SetTimer()
        {
            // Create a timer with a two second interval.
            t = new Timer(2000);
            // Hook up the Elapsed event for the timer. 
            t.Elapsed += OnTimedEvent;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            board.Refresh(this);
        }
    }
}