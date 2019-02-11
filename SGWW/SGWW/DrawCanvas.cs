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

namespace OpenGLES_lessons_template
{
    public class DrawCanvas : View
    {
        public Renderer render;

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

        private float CosR(float a)
        {
            return (float)Math.Cos(Math.PI * a / 180.0);
        }

        private float SinR(float a)
        {
            return (float)Math.Sin(Math.PI * a / 180.0);
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            Paint paint = new Paint();
            paint.Color = Color.Black;
            paint.TextSize = 34;

            canvas.DrawText("eyeX: " + render.eyeX, 0, 50, paint);
            canvas.DrawText("eyeY: " + render.eyeY, 0, 100, paint);
            canvas.DrawText("eyeZ: " + render.eyeZ, 0, 150, paint);
            canvas.DrawText("lookX :" + render.lookX, 0, 200, paint);
            canvas.DrawText("lookY :" + render.lookY, 0, 250, paint);
            canvas.DrawText("lookZ :" + render.lookZ, 0, 300, paint);

            paint.Color = Color.Argb(0x5F, 0, 0, 0);
            
            canvas.DrawText("eyeX: " + render.eyeX, 2, 52, paint);
            canvas.DrawText("eyeY: " + render.eyeY, 2, 102, paint);
            canvas.DrawText("eyeZ: " + render.eyeZ, 2, 152, paint);
            canvas.DrawText("lookX :" + render.lookX, 2, 202, paint);
            canvas.DrawText("lookY :" + render.lookY, 2, 252, paint);
            canvas.DrawText("lookZ :" + render.lookZ, 2, 302, paint);

        }
    }
}