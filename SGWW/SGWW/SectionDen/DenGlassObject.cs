using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Javax.Microedition.Khronos.Opengles;

namespace SGWW
{
    class DenGlassObject : GLObject
    {        
        private const string vertexShader = "den_glass_vertex_shader";
        private const string fragmentShader = "den_glass_fragment_shader";

        private Boolean flag = false;

        public Boolean brocken = false;
        private DateTime lastAlertTime;

        public DenGlassObject(Renderer renderer, string vertexFile, string normalFile) : base(renderer, vertexShader, fragmentShader, vertexFile, normalFile, string.Empty, string.Empty)
        {

        }

        public DenGlassObject(Renderer renderer, string objFile) : base(renderer, vertexShader, fragmentShader, objFile, string.Empty)
        {

        }

        public override void DrawFrame()
        {

            //Glass code with enable blend
            GLES20.GlEnable(GL10.GlBlend);
            GLES20.GlBlendFunc(GL10.GlSrcAlpha, GL10.GlOneMinusSrcAlpha);

            if (brocken)
            {
                lx = 0.9f;
                ly = 0.0f;
                lz = 0.0f;
                lw += 0.01f;
                if (lw > 0.5) lw = 0;
            }
            else
            {
                lw = 0;
            }

            base.DrawFrame();


            GLES20.GlDisable(GL10.GlBlend);

        }

        public override void OnDraw(Canvas canvas)
        {
            flag = !flag;

            Paint paint = new Paint();
            if (flag) paint.Color = Color.Red;
            else
                paint.Color = Color.Yellow;

            //   paint.TextSize = 34;

            //  canvas.DrawText("Attention broken glass on the first floor (south side of the building)", 0, 50, paint);

            if (brocken)
            {
                TimeSpan span = DateTime.Now - lastAlertTime;
                if (span.TotalMilliseconds > 1000)
                {
                    renderer.canvasView.ConsoleWrite("Glass 2 status: brocken", 3);
                    lastAlertTime = DateTime.Now;
                }
            }
        }
    }
}