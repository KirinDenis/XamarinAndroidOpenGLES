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
    public class CanvasView : View
    {
        public Renderer renderer;

        private ConsoleDrawer consoleDrawer = new ConsoleDrawer();

        public CanvasView(Context context) : base(context)
        {
            
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            //draw console 
            consoleDrawer.OnDraw(canvas);
            //call all GLObjects to draw data on View.Canvas
            if (renderer != null)
            {
                foreach (GLObject glObject in renderer.glObjects)
                {
                    glObject.OnDraw(canvas);
                }
            }
        }


        public void ConsoleWrite(string text, int status)
        {
            consoleDrawer.Add(text, status);
        }
    }
}