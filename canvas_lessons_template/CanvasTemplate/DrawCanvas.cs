using System;
using Android.Util;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Graphics;

/// <summary>
/// Android Xamarin OpenGL ES lessons:
/// https://github.com/KirinDenis/XamarinAndroidOpenGLES
/// https://www.youtube.com/watch?v=j4JHrRV3r9g&list=PLIAm4wLW4KD9DEvxQglY2JMr9pt3H-wC9
/// </summary>
namespace CanvasTemplate
{
    public class DrawCanvas : View
    {

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

        /// <summary>
        /// Canvas draw event hamdler 
        /// </summary>
        /// <param name="canvas">The current View Canvas</param>
        protected override void OnDraw(Canvas canvas)
        {
            // Put your Canvas draw code here
        }
    }
}