using Android.Opengl;
using Android.Content;
using Javax.Microedition.Khronos.Opengles;

namespace SGWW
{
    public class PublicRenderer : Renderer
    {
        private bool flag = true; 

        public PublicRenderer(Context context) : base(context)
        {
        }

        public override void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            base.OnSurfaceCreated(gl, config);

            //SETUP OpenGL ES             
            GLES20.GlClearColor(0.9f, 0.9f, 0.9f, 1.0f);

           GLES20.GlEnable(GLES20.GlDepthTest); //uncoment if needs enabled dpeth test

            //GLES20.GlEnable(2884); // GlCullFace == 2884 see OpenGL documentation to this constant value  GLES20.GlDisable(2884);
            //GLES20.GlFrontFace(GLES20.GlCcw);            
            //GLES20.GlCullFace(GLES20.GlFront);

            GLES20.GlEnable(GL10.GlBlend);
            GLES20.GlBlendFunc(GL10.GlSrcAlpha, GL10.GlOneMinusSrcAlpha);

          //  GLES20.GlDisable(GL10.GlLighting);            
          // GLES20.GlEnable(2884);
          //  GLES20.GlDisable(GL10.GlDepthBufferBit);
          //  GLES20.GlEnable(GL10.GlDither);
         //   GLES20.GlEnable(GL10.GlBlend);
           // GLES20.GlBlendFunc(GL10.GlSrcAlpha, GL10.GlOne);


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


