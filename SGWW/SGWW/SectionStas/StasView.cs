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
    public class StasView : View
    {
        public StasRenderer render;

        public StasView(Context context) : base(context)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            Paint paint = new Paint();
            paint.Color = Color.Black;
            paint.TextSize = 34;

            canvas.DrawText("Stas view section", 0, 50, paint);
        }
    }
}