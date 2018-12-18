using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace App4
{
    public class DrawCanvas : View
    {
        public int[] x = new int[1000];
        public int[] y = new int[1000];
        public int[] c = new int[100];
        public Random rnd = new Random();

        public int cx = 500;
        public int cy = 500;

        public DrawCanvas(Context context) : base(context)
        {
        }

        public DrawCanvas(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public DrawCanvas(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public DrawCanvas(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected DrawCanvas(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            //base.OnDraw(canvas);

            Paint paint = new Paint();
            canvas.DrawText("Hello", 100, 100, paint);

            
            for (int i = 0; i < 1000; i++)
            {
                int l = rnd.Next(1, 100);

                if (l < 25) x[i]++;
                else
                    if (l < 50) x[i]--;
                else
                    if (l < 75) y[i]++;
                else
                    y[i]--;

                if (x[i] > cx) x[i]--;
                if (x[i] < cx) x[i]++;
                if (y[i] > cy) y[i]--;
                if (y[i] < cy) y[i]++;



                paint.Color = Color.Rgb(i+75, i + 50, i + 150);
                paint.StrokeWidth = rnd.Next(40);
                //canvas.DrawPoint(rnd.Next(0,10)+i, i, paint);
                canvas.DrawRect(x[i], y[i] , x[i] + 10 , y[i] + 10, paint);
            }
        }
    }
}