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
    public class VladimirView : View
    {
        public VladimirRenderer render;

        float[] matrix = new float[16];

       float[] modelObject = {
                1.000000f, -1.000000f, 1.000000f,
               -1.000000f, -1.000000f, 1.000000f,
               0.999999f, 1.000000f, 1.000001f,
              -1.000000f, 1.000000f, 1.000000f,
               1.000000f, 3.000000f, -3.000000f,
               0.999999f, 3.000000f, -0.999999f,
               -1.000000f, 3.000000f, -1.000000f,
               -1.000000f, 3.000000f, -3.000000f,
               3.000000f, -1.000000f, -3.000000f,
               3.000000f, -1.000000f, -1.000000f,
               3.000000f, 1.000000f, -3.000000f,
               2.999999f, 1.000000f, -0.999999f,
               1.000000f, 1.000000f, -1.000000f,
               -1.000000f, -1.000000f, -3.000000f,
               -3.000000f, -1.000000f, -1.000000f,
               -3.000000f, -1.000000f, -3.000000f,
               -1.000001f, 1.000000f, -0.999999f,
               -3.000000f, 1.000000f, -1.000000f,
               -3.000000f, 1.000000f, -3.000000f,
               1.000000f, -3.000000f, -3.000000f,
               1.000000f, -3.000000f, -1.000000f,
               -1.000000f, -3.000000f, -1.000000f,
               -1.000000f, -3.000000f, -3.000000f,
               1.000000f, -1.000000f, -3.000000f,
               0.999999f, -1.000000f, -0.999999f,
               -1.000000f, -1.000000f, -1.000000f,
               1.000000f, -1.000000f, -5.000000f,
               -1.000000f, -1.000000f, -5.000000f,
               1.000000f, 1.000000f, -5.000000f,
               0.999999f, 1.000000f, -3.000000f,
               -1.000000f, 1.000000f, -3.000000f,
               -1.000000f, 1.000000f, -5.000000f};
        int a = 0;
 
         

        public VladimirView(Context context) : base(context)
        {
//            modelObject2D  = new float[modelObject.Length/3*2];
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            a++;
           
            Paint paint = new Paint();
            paint.Color = Color.Black;
            paint.TextSize = 34;

            canvas.DrawText("Vladimir view section", 50, 50, paint);

            Paint paintPoint = new Paint();
            paintPoint.Color = Color.Black;
            paintPoint.StrokeWidth = 6;
            paintPoint.SetStyle(Paint.Style.Fill);

            float[] vectorResult = new float[4];
            float[] partModelVector = new float[4];
            List<float> resultModel = new List<float> { };
            float[] resultModelArray = new float[4];
            

            Android.Opengl.Matrix.SetIdentityM(matrix, 0);
            Android.Opengl.Matrix.RotateM(matrix, 0, a, 1, 1, 1);
            //Android.Opengl.Matrix.PerspectiveM(matrix, 0, 1, 1, 1, 1);
            Android.Opengl.Matrix.ScaleM(matrix, 0, 50, 50, 50);

            for (int i=0; i< modelObject.Length; i+=3)
            {
                partModelVector[0]   = modelObject[i];
                partModelVector[1] = modelObject[i+1];
                partModelVector[2] = modelObject[i+2];
                partModelVector[3] = 1;

                Android.Opengl.Matrix.MultiplyMV(vectorResult, 0, matrix, 0, partModelVector, 0);

                canvas.DrawPoint(vectorResult[0] + 500, vectorResult[1] + 500, paintPoint);

                resultModel.Add(vectorResult[0]);
                resultModel.Add(vectorResult[1]);
                resultModel.Add(vectorResult[2]);
                resultModel.Add(vectorResult[3]);
            }


                     
            

            



            
            //canvas.DrawPoint(xResult+500, yResult + 500, paintPoint);
                   

                    

         

        }
    }
}