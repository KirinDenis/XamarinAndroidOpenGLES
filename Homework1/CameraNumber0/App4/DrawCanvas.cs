using System;
using System.Collections.Generic;
using System.IO;
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

        const float coord = 1.0f;
        const float w = 1.0f;

        /*
        // Cube coords
        // X, Y, Z = 1 vertex * 3 = 1 face * 12 = 1 cube             
        float[] vertex = {
                -coord,-coord,-coord, w,
                -coord,-coord, coord,w,
                -coord, coord, coord,w,

                coord, coord,-coord,w,
                -coord,-coord,-coord,w,
                -coord, coord,-coord,w,

                coord,-coord, coord,w,
                -coord,-coord,-coord,w,
                coord,-coord,-coord,w,

                coord, coord,-coord,w,
                coord,-coord,-coord,w,
                -coord,-coord,-coord,w,

                -coord,-coord,-coord,w,
                -coord, coord, coord,w,
                -coord, coord,-coord,w,

                coord,-coord, coord,w,
                -coord,-coord, coord,w,
                -coord,-coord,-coord,w,

                -coord, coord, coord,w,
                -coord,-coord, coord,w,
                coord,-coord, coord,w,

                coord, coord, coord,w,
                coord,-coord,-coord,w,
                coord, coord,-coord,w,

                coord,-coord,-coord,w,
                coord, coord, coord,w,
                coord,-coord, coord,w,

                coord, coord, coord,w,
                coord, coord,-coord,w,
                -coord, coord,-coord,w,

                coord, coord, coord,w,
                -coord, coord,-coord,w,
                -coord, coord, coord,w,

                coord, coord, coord,w,
                -coord, coord, coord,w,
                coord,-coord, coord,w,
            };
            */
        //empty buffer for loading from file 
        float[] vertex;

        public DrawCanvas(Context context) : base(context)
        {
            long size;
            float[] vertex3; //temporary array for object format x,y,z to be converted to x,y,z,w
            // Vertex
            Stream fileIn = context.Resources.OpenRawResource(Resource.Raw.tor_objvertex) as Stream;
            MemoryStream m = new MemoryStream();
            fileIn.CopyTo(m);
            size = m.Length;
            vertex3 = new float[size / 4];            
            System.Buffer.BlockCopy(m.ToArray(), 0, vertex3, 0, (int)size);

            //convert x,y,z to x,y,z,w
            vertex = new float[vertex3.Length + vertex3.Length / 3];

            int v3counter = 0;
            for (int i = 0; i < vertex.Length; i+=4)
            {
                vertex[i] = vertex3[v3counter]; //x
                vertex[i+1] = vertex3[v3counter + 1]; //y
                vertex[i + 2] = vertex3[v3counter + 2]; //z 
                vertex[i + 3] = 1; //w

                v3counter += 3;
            }

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
            Android.Opengl.Matrix.TranslateM(translateMatrix, 0,  0, 0, 350);
            Android.Opengl.Matrix.RotateM(translateMatrix, 0, a, 1, 1, 1);
            Android.Opengl.Matrix.ScaleM(translateMatrix, 0, 120, 120, 120);

            float[] gl_Position = new float[vertex.Length];

            //transform all object vertexes by translation matrix
            //vertex shadex emulation
            for (int i = 0; i < vertex.Length; i += 4)            
            {
                Android.Opengl.Matrix.MultiplyMV(gl_Position, i, translateMatrix, 0, vertex, i);
            }

            float prevX = 0;
            float prevY = 0;
            float firstX = 0;
            float firstY = 0;


            //aspect 
            float w = 1400;
            float h = 800;            
            paint.Color = Color.Black;
            canvas.DrawRect(0, 0, w, h, paint);


            for (int i = 0; i < vertex.Length; i += 4)            
            {

                //Calculate camera screen coords
                //
                //gl_Position[i+0] = x, gl_Position[i + 1] = y, gl_Position[i + 2] = z
                //eye x,y,z = |0,0,0]
                //lookat 0,0, z + 50 
                //screen x,z and y,z               
                float cameraZ = 250;
                //First y, z where z = 50, find y - step by step CODEE
                float cosA = gl_Position[i + 2] / (float)(Math.Sqrt(gl_Position[i + 1] * gl_Position[i + 1] + gl_Position[i + 2] + gl_Position[i + 2]));
                float sinA = gl_Position[i + 1] / (float)(Math.Sqrt(gl_Position[i + 1] * gl_Position[i + 1] + gl_Position[i + 2] + gl_Position[i + 2]));
                float screenY = cameraZ / cosA * sinA + h / 2;
                //Next x, z
                cosA = gl_Position[i + 2] / (float)(Math.Sqrt(gl_Position[i + 0] * gl_Position[i + 0] + gl_Position[i + 2] + gl_Position[i + 2]));
                sinA = gl_Position[i + 0] / (float)(Math.Sqrt(gl_Position[i + 0] * gl_Position[i + 0] + gl_Position[i + 2] + gl_Position[i + 2]));
                float screenX = cameraZ / cosA * sinA + w / 2;

                //Surface draw 
                paint.Color = Color.Red;
                canvas.DrawRect(screenX, screenY, screenX + 2, screenY + 2, paint);

                //Link vetexes (GlStripLines)
                if (i != 0) //skip first
                {

                    paint.Color = Color.Red;
                    canvas.DrawLine(screenX-300, screenY, prevX - 300, prevY, paint);
                    paint.Color = Color.Green;
                    canvas.DrawLine(screenX, screenY, prevX, prevY, paint);
                    paint.Color = Color.Blue;
                    canvas.DrawLine(screenX+300, screenY + 1, prevX +300, prevY + 1, paint);

                }
                else
                {
                    firstX = screenX;
                    firstY = screenY;
                }
                prevX = screenX;
                prevY = screenY;
            }

            paint.Color = Color.Green;
            canvas.DrawLine(prevX, prevY, firstX, firstY, paint);


        }
    }
}