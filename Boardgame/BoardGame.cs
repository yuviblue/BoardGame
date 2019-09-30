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
    class BoardGame: View
    {
        private bool isCoinExist; 
        private Coin coin;
        private Square[,] squares;
        private Context context;
        private Point lastPosition;
        private bool dragging;

        public BoardGame(Context context): base(context)
        {
            this.context = context;
            this.squares = new Square[6, 6];
            this.coin = new Coin(this, 0, 0, 80, 0, 10);
        }

        public void DrawBoard(Canvas canvas)
        {
            int x = 0;
            int y = 0;
            int h = canvas.Width / 6;
            int w = canvas.Width / 6;
            Color color;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            color = Color.White;
                        }
                        else
                        {
                            color = Color.Black;
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            color = Color.Black;
                        }
                        else
                        {
                            color = Color.White;
                        }
                    }

                    squares[i, j] = new Square(x, y, w, h, color, this);
                    squares[i, j].Draw(canvas);
                    x += w;
                }
                y += h;
                x = 0;
            }
        }

        public void DrawCoin(Canvas canvas)
        {
            if (!isCoinExist)
            {
                float w = canvas.Width / 6;
                float h = canvas.Width / 6;
                coin.SetX(w / 2);
                coin.SetY(h / 2);
                isCoinExist = true;
            }
            coin.Draw(canvas);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            DrawBoard(canvas);
            DrawCoin(canvas);
        }

        public override bool OnTouchEvent(MotionEvent evn)
        {
            if (evn.Action == MotionEventActions.Down && coin.DidUserTouchMe(evn.GetX(), evn.GetY()))
            {
                lastPosition = coin.GetCoinSquare();
                dragging = true;
            }
            else if (evn.Action == MotionEventActions.Move && dragging)
            {
                coin.SetX(evn.GetX());
                coin.SetY(evn.GetY());
                Invalidate();
            }
            else if (evn.Action == MotionEventActions.Up )
            {
                if (dragging)
                {
                    if (!coin.IsSquareBlack())
                    {
                        coin.SetX(lastPosition.X * 180 + 90);
                        coin.SetY(lastPosition.Y * 180 + 90);
                        Invalidate();
                    }
                }
                else if (Coin.IsSquareBlack((int)evn.GetX(), (int)evn.GetY()))
                {
                    coin.SetX(evn.GetX());
                    coin.SetY(evn.GetY());
                    Invalidate();
                } 

                var p = coin.GetCoinSquare();
                coin.SetX(p.X * 180 + 90);
                coin.SetY(p.Y * 180 + 90);
                Invalidate();

                dragging = false;
            }
            return true;
        }
    }
}