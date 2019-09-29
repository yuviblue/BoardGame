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

namespace Boardgame
{
    class BoardGame: View
    {
        private bool isCoinExist; // 
        private Coin coin;
        private Square[,] squares;
        private Context context;

        BoardGame(Context context): base(context)
        {
            this.context = context;
            this.squares = new Square[6, 6];
            this.coin = new Coin(this, 0, 0, 0, 0, 10);
        }
    }
}