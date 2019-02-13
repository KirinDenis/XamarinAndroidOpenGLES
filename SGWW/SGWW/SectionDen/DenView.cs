using System;
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
    public class DenView : View
    {
        public DenRenderer render;
        private Boolean flag = false;

        public DenView(Context context) : base(context)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            flag = !flag;

            Paint paint = new Paint();
            if (flag) paint.Color = Color.Red;
             else
                paint.Color = Color.Yellow;

            paint.TextSize = 34;

            canvas.DrawText("Attention broken glass on the first floor (south side of the building)", 0, 50, paint);
        }
    }
}