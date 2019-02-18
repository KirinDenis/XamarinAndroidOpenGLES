using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.Content;
using Android.Graphics;
using SGWW.SectionVladimir;
using Point = SGWW.SectionVladimir.Point;

namespace SGWW
{
    /// <summary>
    /// view class for public section (canvas draw handler)
    /// </summary>
    public class VladimirView : View
    {
        public VladimirRenderer render;

        float[] matrix = new float[16];

        Distanсe distanceClass;

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
        public int objectSize = -1;



        public VladimirView(Context context) : base(context)
        {
            //   modelObject2D  = new float[modelObject.Length/3*2];

            ObjParser model3D = new ObjParser();

            List<byte[]> test1 = model3D.ParsedObject(context, "dolphin3");
                        
            float[] floatArray = new float[test1[0].Length / 4];
            System.Buffer.BlockCopy(test1[0], 0, floatArray, 0, (int)test1[0].Length);

            modelObject = floatArray;



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
            paintPoint.StrokeWidth = 1;
            paintPoint.SetStyle(Paint.Style.Fill);

            float[] vectorResult = new float[4];
            float[] partModelVector = new float[4];
            List<float> resultModel = new List<float> { };
            float[] resultModelArray;

            List<Distanсe> dataDistance = new List<Distanсe> { };

            List<List<Distanсe>> dataDistanceAllPoints = new List<List<Distanсe>> { };

            Android.Opengl.Matrix.SetIdentityM(matrix, 0);
            Android.Opengl.Matrix.RotateM(matrix, 0, a, 1, 1, 1);
            // Android.Opengl.Matrix.PerspectiveM(matrix, 0, 50.0f, 1,  1, 1);
            Android.Opengl.Matrix.ScaleM(matrix, 0, 50, 50, 50);

            float startX = -100;
            float startY = -100;

            float stopX = -100;
            float stopY = -100;

            float newStartX = -100;
            float newStartY = -100;

            float newStopX = -100;
            float newStopY = -100;

            Point[] tringle = new Point[3];

            //float bufferStartX = -100;
            //float bufferStartY = -100;

            //float bufferStopX = -100;
            //float bufferStopY = -100;

            int countCicle = 0;

            for (int i = 0; i < modelObject.Length; i += 3)
            {
                partModelVector[0] = modelObject[i];
                partModelVector[1] = modelObject[i + 1];
                partModelVector[2] = modelObject[i + 2];
                partModelVector[3] = 1;

                Android.Opengl.Matrix.MultiplyMV(vectorResult, 0, matrix, 0, partModelVector, 0);

                canvas.DrawPoint(vectorResult[0] + 500, vectorResult[1] + 500, paintPoint);
                

                if (countCicle % 3 == 0)
                {
                    Point point = new Point();
                    point.x = vectorResult[0];
                    point.y = vectorResult[1];

                    tringle[0] = point;
                }


                if (countCicle % 3 == 1)
                {
                    Point point = new Point();
                    point.x = vectorResult[0];
                    point.y = vectorResult[1];

                    tringle[1] = point;
                }

                if (countCicle % 3 == 2)
                {
                    Point point = new Point();
                    point.x = vectorResult[0];
                    point.y = vectorResult[1];

                    tringle[2] = point;
                }

                if (tringle[0] !=null && tringle[1] != null && tringle[2] != null)
                {
                    canvas.DrawLine(tringle[0].x + 500, tringle[0].y + 500, tringle[1].x + 500, tringle[1].y + 500, paintPoint);
                    canvas.DrawLine(tringle[1].x + 500, tringle[1].y + 500, tringle[2].x + 500, tringle[2].y + 500, paintPoint);
                    canvas.DrawLine(tringle[2].x + 500, tringle[2].y + 500, tringle[1].x + 500, tringle[1].y + 500, paintPoint);

                    tringle = new Point[3];
                }

                countCicle++;

                //if (i % 2 == 0)
                //{
                //    startX = vectorResult[0];
                //    startY = vectorResult[1];
                //    //if (countCicle != 0)
                //    //{
                //    //    bufferStopX = vectorResult[0];
                //    //    bufferStopY = vectorResult[1];
                //    //}

                //    if (newStartX != -100)
                //    {
                //        newStopX = startX;
                //        newStopY = startY;
                //    }



                //}

                //if (i % 2 == 1)
                //{
                //    stopX = vectorResult[0];
                //    stopY = vectorResult[1];

                //    //bufferStartX = vectorResult[0];
                //    //bufferStartY = vectorResult[1];
                //}

                //if (startX != -100 && stopX != -100)
                //{
                //    canvas.DrawLine(startX + 500, startY + 500, stopX + 500, stopY + 500, paintPoint);

                //    newStartX = stopX;
                //    newStartY = stopY;

                //    // canvas.DrawLine(newStartX + 500, newStartY + 500, newStopX+ 500, newStopY + 500, paintPoint);


                //    //startX = -100;
                //    //startY = -100;

                //    //stopX = -100;
                //    //stopY = -100;
                //}

                //if (newStartX != -100 && newStopX != -100)
                //{

                //    canvas.DrawLine(newStartX + 500, newStartY + 500, newStopX+ 500, newStopY + 500, paintPoint);


                //    //newStartX = -100;
                //    //newStartY = -100;

                //    //newStopX = -100;
                //    //newStopY = -100;
                //}


                //if (bufferStartX != -100 && bufferStopX != -100)
                //{
                //    canvas.DrawLine(bufferStartX + 500, bufferStartY + 500, bufferStopX + 500, bufferStopY + 500, paintPoint);

                //    bufferStartX = -100;
                //    bufferStartY = -100;

                //    bufferStopX = -100;
                //    bufferStopY = -100;
                //}
                //countCicle++;
                //resultModel.Add(vectorResult[0]);
                //resultModel.Add(vectorResult[1]);
                //resultModel.Add(vectorResult[2]);
                //resultModel.Add(vectorResult[3]);
            }

            //resultModelArray = new float[resultModel.Count];
            //resultModelArray = resultModel.ToArray<float>();

            //float xResultModelArray = 0;
            //float yResultModelArray = 0;
            //float zResultModelArray = 0;

            //float xResultModelArrayCompare = 0;
            //float yResultModelArrayCompare = 0;
            //float zResultModelArrayCompare = 0;

            //float distance = 0;


            //for (int i = 0; i < resultModelArray.Length; i += 4)
            //{

            //    for (int k = 0; k < resultModelArray.Length; k += 4)
            //    {

            //        xResultModelArray = resultModelArray[i];
            //        yResultModelArray = resultModelArray[i + 1];
            //        zResultModelArray = resultModelArray[i + 2];


            //        xResultModelArrayCompare = resultModelArray[k];
            //        yResultModelArrayCompare = resultModelArray[k + 1];
            //        zResultModelArrayCompare = resultModelArray[k + 2];


            //        distance = (float)Math.Sqrt(Math.Pow(xResultModelArrayCompare - xResultModelArray, 2) +
            //                                     Math.Pow(yResultModelArrayCompare - yResultModelArray, 2) +
            //                                     Math.Pow(zResultModelArrayCompare - zResultModelArray, 2));
            //        distanceClass = new Distanсe();

            //        distanceClass.x1 = xResultModelArray;
            //        distanceClass.y1 = yResultModelArray;
            //        distanceClass.z1 = zResultModelArray;

            //        distanceClass.x2 = xResultModelArrayCompare;
            //        distanceClass.y2 = yResultModelArrayCompare;
            //        distanceClass.z2 = zResultModelArrayCompare;

            //        distanceClass.distance = distance;

            //        if (distance != 0)
            //        {

            //            dataDistance.Add(distanceClass);
            //        }
            //    }

            //    dataDistanceAllPoints.Add(dataDistance);
            //    dataDistance = new List<Distanсe>();

            //}


            //for (int i = 0; i < dataDistanceAllPoints.Count; i ++)
            //{

            //    Utilits.comparing(dataDistanceAllPoints[i]);


            //    canvas.DrawLine(dataDistanceAllPoints[i][0].x1 + 500, dataDistanceAllPoints[i][0].y1 + 500, dataDistanceAllPoints[i][0].x2 + 500, dataDistanceAllPoints[i][0].y2 + 500, paintPoint);
            //    canvas.DrawLine(dataDistanceAllPoints[i][1].x1 + 500, dataDistanceAllPoints[i][1].y1 + 500, dataDistanceAllPoints[i][1].x2 + 500, dataDistanceAllPoints[i][1].y2 + 500, paintPoint);
            //    canvas.DrawLine(dataDistanceAllPoints[i][2].x1 + 500, dataDistanceAllPoints[i][2].y1 + 500, dataDistanceAllPoints[i][2].x2 + 500, dataDistanceAllPoints[i][2].y2 + 500, paintPoint);
            //    if( dataDistanceAllPoints[i][3].distance <= dataDistanceAllPoints[i][0].distance ||
            //        dataDistanceAllPoints[i][3].distance <= dataDistanceAllPoints[i][1].distance ||
            //        dataDistanceAllPoints[i][3].distance <= dataDistanceAllPoints[i][2].distance) { 
            //    canvas.DrawLine(dataDistanceAllPoints[i][3].x1 + 500, dataDistanceAllPoints[i][3].y1 + 500, dataDistanceAllPoints[i][3].x2 + 500, dataDistanceAllPoints[i][3].y2 + 500, paintPoint);
            //    }
            //    if (dataDistanceAllPoints[i][4].distance <= dataDistanceAllPoints[i][0].distance ||
            //        dataDistanceAllPoints[i][4].distance <= dataDistanceAllPoints[i][1].distance ||
            //        dataDistanceAllPoints[i][4].distance <= dataDistanceAllPoints[i][2].distance)
            //    {
            //        canvas.DrawLine(dataDistanceAllPoints[i][4].x1 + 500, dataDistanceAllPoints[i][4].y1 + 500, dataDistanceAllPoints[i][4].x2 + 500, dataDistanceAllPoints[i][4].y2 + 500, paintPoint);
            //    }

            //    if (dataDistanceAllPoints[i][5].distance <= dataDistanceAllPoints[i][0].distance ||
            //       dataDistanceAllPoints[i][5].distance <= dataDistanceAllPoints[i][1].distance ||
            //       dataDistanceAllPoints[i][5].distance <= dataDistanceAllPoints[i][2].distance)
            //    {
            //        canvas.DrawLine(dataDistanceAllPoints[i][5].x1 + 500, dataDistanceAllPoints[i][5].y1 + 500, dataDistanceAllPoints[i][5].x2 + 500, dataDistanceAllPoints[i][5].y2 + 500, paintPoint);
            //    }
            //    if (dataDistanceAllPoints[i][6].distance <= dataDistanceAllPoints[i][0].distance ||
            //       dataDistanceAllPoints[i][6].distance <= dataDistanceAllPoints[i][1].distance ||
            //       dataDistanceAllPoints[i][6].distance <= dataDistanceAllPoints[i][2].distance)
            //    {
            //        canvas.DrawLine(dataDistanceAllPoints[i][6].x1 + 500, dataDistanceAllPoints[i][6].y1 + 500, dataDistanceAllPoints[i][6].x2 + 500, dataDistanceAllPoints[i][6].y2 + 500, paintPoint);
            //    }
            //}



                    //canvas.DrawPoint(xResult+500, yResult + 500, paintPoint);
                }
    }
}