﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.Content;
using Android.Graphics;

namespace SGWW
{
    /// <summary>
    /// view class for public section (canvas draw handler)
    /// </summary>
    public class RuslanView : View
    {
        public RuslanRenderer render;

        public RuslanView(Context context) : base(context)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            Paint paint = new Paint();
            paint.Color = Color.Black;
            paint.TextSize = 34;

            canvas.DrawText("Ruslan view section", 0, 50, paint);
        }
    }
}