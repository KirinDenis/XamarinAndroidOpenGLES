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
        private int i = 0;
        private float a = 45;

        private float[] vertex = { 100, 0, 0, 100, -100, 0, 23, 234, 34, 45, -45, -54 };

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
            paint.Color = Color.Red;

            a++;

            // Rotation by coords with use matrix 
            float[] translateMatrix = new float[16]; //Matrxi 3x3            
            Android.Opengl.Matrix.SetIdentityM(translateMatrix, 0);
            Android.Opengl.Matrix.TranslateM(translateMatrix, 0, 300, 0, 300);
            Android.Opengl.Matrix.RotateM(translateMatrix, 0, a, 0, 1, 0);

            for (int i = 0; i < vertex.Length; i += 2)
            {
                paint.Color = Color.Red;

                float[] vector = new float[4];
                float[] vectorResult = new float[4];
                vector[0] = vertex[i];
                vector[1] = 1;
                vector[2] = vertex[i + 1];
                vector[3] = 1;

                Android.Opengl.Matrix.MultiplyMV(vectorResult, 0, translateMatrix, 0, vector, 0);
                canvas.DrawRect(vectorResult[0], vectorResult[2], vectorResult[0] + 10, vectorResult[2] + 10, paint);

                /*
                //Link vetexes if you need (Home Work ))) )
                if (xP != 0)
                {
                    paint.Color = Color.Green;
                    canvas.DrawLine(xS0, yS0, xP, yP, paint);
                }

                xP = xS0;
                yP = yS0;
                */
            }


            /* Transform no matrix used 
            float xP = 0;
            float yP = 0;
            float xS0;
            float yS0;

            float tx = 300;
            float ty = 300;

            float sx = 1;
            float sy = 1;

                for (int i = 0; i < vertex.Length; i += 2)
                {
                    paint.Color = Color.Red;

                    xS0 = vertex[i] * CosR(a) * sx - vertex[i + 1] * SinR(a) * sy + tx;
                    yS0 = vertex[i] * SinR(a) * sx + vertex[i + 1] * CosR(a) * sy + ty;

                    canvas.DrawRect(xS0, yS0, xS0 + 10, yS0 + 10, paint);

                    if (xP != 0)
                    {
                        paint.Color = Color.Green;
                        canvas.DrawLine(xS0, yS0, xP, yP, paint);
                    }

                    xP = xS0;
                    yP = yS0;

                }


            xS0 = vertex[1] * CosR(a) * sx - vertex[1 + 1] * SinR(a) * sy + tx;
            yS0 = vertex[1] * SinR(a) * sx + vertex[1 + 1] * CosR(a) * sy + ty;
            canvas.DrawLine(xS0, yS0, xP, yP, paint);
            */
            /* Rotation no matrix used 
            float x0 = 100;
            float y0 = 0;

            float xS0 = x0 * CosR(a) - y0 * SinR(a) + 300;
            float yS0 = x0 * SinR(a) + y0 * CosR(a) + 300;

            canvas.DrawRect(xS0, yS0, xS0 + 10, yS0 + 10, paint);

            float x1 = 0;
            float y1 = 100;

            float xS1 = x1 * CosR(a) - y1 * SinR(a) + 300;
            float yS1 = x1 * SinR(a) + y1 * CosR(a) + 300;

            canvas.DrawRect(xS1, yS1, xS1 + 10, yS1 + 10, paint);

            float x2 = -100;
            float y2 = 0;

            float xS2 = x2 * CosR(a) - y2 * SinR(a) + 300;
            float yS2 = x2 * SinR(a) + y2 * CosR(a) + 300;

            canvas.DrawRect(xS2, yS2, xS2 + 10, yS2 + 10, paint);

            paint.Color = Color.Green;
            canvas.DrawLine(xS0, yS0, xS1, yS1, paint);
            canvas.DrawLine(xS1, yS1, xS2, yS2, paint);
            canvas.DrawLine(xS2, yS2, xS0, yS0, paint);
            */


            // Radius Vertex ----------------------------------------------------
            //x = r * cos(a)
            //y = r * sin(a)


            /*

            float r = 100;
            float x1 = r * CosR(a) + 300;
            float y1 = r * SinR(a) + 300;
            canvas.DrawRect(x1, y1, x1+10, y1 +10, paint);

            float x2 = r * CosR(a + 90) + 300;
            float y2 = r * SinR(a + 90) + 300;
            canvas.DrawRect(x2, y2, x2 + 10, y2 + 10, paint);


            float x3 = r * CosR(a + 270) + 300;
            float y3 = r * SinR(a + 270) + 300;
            canvas.DrawRect(x3, y3, x3 + 10, y3 + 10, paint);

            paint.Color = Color.Green;
            canvas.DrawLine(x1, y1, x2, y2, paint);
            canvas.DrawLine(x2, y2, x3, y3, paint);
            canvas.DrawLine(x3, y3, x1, y1, paint);
            */





        }
    }
}