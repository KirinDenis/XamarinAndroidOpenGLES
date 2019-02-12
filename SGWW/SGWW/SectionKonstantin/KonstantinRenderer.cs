using Android.Opengl;
using Android.Content;
using System.Collections.Generic;
using Javax.Microedition.Khronos.Opengles;

namespace SGWW
{
    /// Old house model
    /// https://free3d.com/3d-model/old-house-2-96599.html
    /// https://free3d.com/user/tharidu
    /// </summary>
    public class KonstantinRenderer : Java.Lang.Object, GLSurfaceView.IRenderer
    {
        /// <summary>
        /// Android activity Context context
        /// </summary>
        private Context context;

        //Store the model matrix. This matrix is used to move models from object space (where each model can be thought of being located at the center of the universe) to world space.
        private float[] mProjectionMatrix = new float[16];
        private float[] mViewMatrix = new float[16];

        /// <summary>
        /// Scene objects list
        /// </summary>
        private List<GLObject> glObjects = new List<GLObject>();

        // Position the eye behind the origin.
        public float eyeX = 0.0f;
        public float eyeY = 0.0f;
        public float eyeZ = -4.5f;

        // We are looking toward the distance
        public float lookX = 0.0f;
        public float lookY = 0.0f;
        public float lookZ = 10.0f;

        // Set our up vector. This is where our head would be pointing were we holding the camera.
        private float upX = 0.0f;
        private float upY = 1.0f;
        private float upZ = 0.0f;

        public KonstantinRenderer(Context context) : base()
        {
            this.context = context;
        }

        public void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            glObjects.Add(new GLObject(context, "vertex_shader", "fragment_shader", "oldhouse_objvertex", "oldhouse_objnormal", "oldhouse_objtexture", "body"));

            Matrix.SetLookAtM(mViewMatrix, 0, eyeX, eyeY, eyeZ, lookX, lookY, lookZ, upX, upY, upZ);

            //Ask android to run RAM garbage cleaner
            System.GC.Collect();

            //Setup OpenGL ES 
            GLES20.GlClearColor(0.9f, 0.9f, 0.9f, 0.9f);
            GLES20.GlEnable(GLES20.GlDepthTest); //uncoment if needs enabled dpeth test
            //  GLES20.GlEnable(2884); // GlCullFace == 2884 see OpenGL documentation to this constant value  
            //  GLES20.GlCullFace(GLES20.GlFront);
        }

        public void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            GLES20.GlViewport(0, 0, width, height);
            float ratio = (float)width / height;
            float left = -ratio;
            float right = ratio;
            float top = 1.0f;
            float bottom = -1.0f;
            float near = 1.0f;
            float far = 100.0f;
            Matrix.FrustumM(mProjectionMatrix, 0, left, right, bottom, top, near, far);
        }

        public void OnDrawFrame(IGL10 gl)
        {
            GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit);

            // Set the view matrix. This matrix can be said to represent the camera position.
            // NOTE: In OpenGL 1, a ModelView matrix is used, which is a combination of a model and
            // view matrix. In OpenGL 2, we can keep track of these matrices separately if we choose.
            Matrix.SetLookAtM(mViewMatrix, 0, eyeX, eyeY, eyeZ, lookX, lookY, lookZ, upX, upY, upZ);

            glObjects[0].DrawFrame(gl, mViewMatrix, mProjectionMatrix);
        }
    }
}