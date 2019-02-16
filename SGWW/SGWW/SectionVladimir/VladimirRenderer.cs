using Android.Opengl;
using Android.Content;
using Javax.Microedition.Khronos.Opengles;

namespace SGWW
{
    public class VladimirRenderer : Renderer
    {

        public VladimirRenderer(Context context) : base(context)
        {
        }

        public override void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            base.OnSurfaceCreated(gl, config);

            //SETUP OpenGL ES             
            GLES20.GlClearColor(0.9f, 0.9f, 0.9f, 0.9f);
            GLES20.GlEnable(GLES20.GlDepthTest); //uncoment if needs enabled dpeth test
            //ENDSETUP OpenGL ES             

            //Loading objects 
            glObjects.Add(new GLObject(this, "vertex_shader", "fragment_shader", "oldhouse_objvertex", "oldhouse_objnormal", "oldhouse_objtexture", "body"));

            //Ask android to run RAM garbage cleaner
            System.GC.Collect();
        }

        public override void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            base.OnSurfaceChanged(gl, width, height);
        }

        public override void OnDrawFrame(IGL10 gl)
        {
            GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit);
            base.OnDrawFrame(gl);
        }
    }
}


