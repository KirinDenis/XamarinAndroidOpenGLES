using Android.Opengl;
using Android.Content;
using System.Collections.Generic;
using Javax.Microedition.Khronos.Opengles;

namespace SGWW
{
    public class Renderer : Java.Lang.Object, GLSurfaceView.IRenderer
    {
        /// <summary>
        /// Android activity Context context
        /// </summary>
        public Context context;

        /// <summary>
        /// Scen camera
        /// </summary>
        public Camera camera;

        /// <summary>
        /// Scene objects list
        /// </summary>
        public List<GLObject> glObjects = new List<GLObject>();

        /// <summary>
        /// on scene 2D canvas view 
        /// </summary>
        public CanvasView canvasView;


        public Renderer(Context context) : base()
        {
            this.context = context;
            canvasView = new CanvasView(context);            
            canvasView.renderer = this;
        }

        public virtual void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
        }

        public virtual void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            camera = new Camera(width, height);
        }

        public virtual void OnDrawFrame(IGL10 gl)
        {
            camera.OnDrawFrame();

            foreach (GLObject glObject in glObjects)
            {
                glObject.DrawFrame();
            }

            if (canvasView != null)
            {
                canvasView.Invalidate();
            }
        }
    }
}