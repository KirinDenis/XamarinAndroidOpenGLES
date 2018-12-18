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
        public float xm0 = 400;
        public float ym0 = 100;
        public float rm = 300;
        public double am = Math.PI / 2;
        public double pm = Math.PI / 4;
        public Boolean mdirection = true;

        public float rc = 50;
        public double ar = 0;


        public Random rnd = new Random();

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
            if (mdirection)
            {
                am = am - 0.01f;
                if (am < (Math.PI / 2 - Math.PI / 4)) mdirection = false;
            }
            else
            {
                am = am + 0.01f;
                if (am > (Math.PI / 2 + Math.PI / 4)) mdirection = true;
            }

            float xm1 = rm * (float)Math.Cos(am) + xm0;
            float ym1 = rm * (float)Math.Sin(am) + ym0;
            Paint paint = new Paint();
            canvas.DrawLine(xm0, ym0, xm1, ym1, paint);

            ar = ar + 0.1f;
            float xc = rc * (float)Math.Cos(ar) + xm1;
            float yc = rc * (float)Math.Sin(ar) + ym1;
            canvas.DrawLine(xm1, ym1, xc, yc, paint);


            //canvas.DrawRect(x, y, x + 20, y + 20, paint);


        }
    }
}