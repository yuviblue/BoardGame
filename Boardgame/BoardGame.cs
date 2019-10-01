using System;
using System.Collections.Generic;
using System.Timers;
using Android.Content;
using Android.Graphics;
using Android.Views;

namespace Boardgame
{
    class BoardGame: View
    {
        private List<Coin> coins = new List<Coin>();
        private Square[,] squares;
        private Context context;
        private Timer timer;
        private int score;

        public int Lives { get ; set;}

        enum GameState
        {
            Playing,
            GameOver,
            PressAnyKey
        };

        private GameState gameState = GameState.Playing;

        public BoardGame(Context context): base(context)
        {
            this.context = context;
            squares = new Square[6, 6];
            score = 0;
            Lives = 3;
            SetTimer(); 
        }

        public void SetTimer()
        {
            // Create a timer with a two second interval.
            timer = new Timer(500);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Random random = new Random();
            int ix = random.Next(0, 6);
            int iy = random.Next(0, 6);

            while(IsPositionTaken(ix, iy))
            {
                ix = random.Next(0, 6);
                iy = random.Next(0, 6);
            }
            var coin = new Coin(this, ix * 180 + 90, iy * 180 + 90);
            lock (coins)
            {
                coins.Add(coin);
            }
            Invalidate();
            
        }

        public bool IsPositionTaken(int ix, int iy)
        {
            lock (coins)
            {
                foreach (var c in coins)
                {
                    int ixCoin = (int)c.PX / 180;
                    int iyCoin = (int)c.PY / 180;

                    if (ix == ixCoin && iy == iyCoin)
                    {
                        return true;
                    }
                }
            }

            return false;
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

        

        public void DrawCoins(Canvas canvas)
        {
            lock (coins)
            {
                foreach (var c in coins)
                {
                    c.Draw(canvas);
                }
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            DrawBoard(canvas);
            DrawCoins(canvas);
            DrawText(canvas);
        }

        private void DrawText(Canvas canvas)
        {
            canvas.DrawText("Score: " + score, 10, 1200, new Paint() { Color = Color.Black, TextSize = 100});
            canvas.DrawText("Lives: " + Lives, 10, 1350, new Paint() { Color = Color.Black, TextSize = 100 });

            if (gameState != GameState.Playing)
            {
                canvas.DrawText("*** Game Over ***", 10, 1530, new Paint() { Color = Color.Black, TextSize = 130 });

                if (gameState == GameState.GameOver)
                {
                    Timer t = new Timer(3000);
                    t.Elapsed += OnFreezeEnd;
                    t.AutoReset = false;
                    t.Enabled = true;
                }
                else if(gameState == GameState.PressAnyKey)
                {
                    canvas.DrawText("Touch screen to start over", 10, 1680, new Paint() { Color = Color.Black, TextSize = 80 });
                }
            }
        }

        public void OnFreezeEnd(object source, ElapsedEventArgs e)
        {
            gameState = GameState.PressAnyKey;
            Invalidate();
        }

        public override bool OnTouchEvent(MotionEvent evn)
        {
             if (gameState == GameState.PressAnyKey)
            {
                NewGame();
            }
            else if(gameState == GameState.Playing)
            {
                foreach (var c in coins.ToArray())
                {
                    if (c.OnTouchEvent(evn.GetX(), evn.GetY()))
                    {
                        DeleteCoin(c);
                        score++;
                        timer.Interval -= 5;
                        break;
                    }
                }
            }
            return true;
        }

        public void DeleteCoin(Coin coin)
        {
            lock (coins)
            {
                coins.Remove(coin);
                coin.Dispose();
                HandleGameOver();
            }
            
            Invalidate();
        }

        private void HandleGameOver()
        {
            if (Lives == 0)
            {
                timer.Stop();
                timer.Close();
                gameState = GameState.GameOver;
            }
        }

        private void NewGame()
        {
            foreach (var c in coins)
            {
                c.Dispose();
            }

            coins.Clear();
            SetTimer();
            gameState = GameState.Playing;
            Lives = 3;
            score = 0;
        }

    }
}