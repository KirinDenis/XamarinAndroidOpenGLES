using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Opengl;
using Java.IO;
using Java.Nio;
using Javax.Microedition.Khronos.Opengles;

namespace OpenGLES_lessons_template
{
    /// <summary>
    /// This example Java source from https://github.com/learnopengles/Learn-OpenGLES-Tutorials/tree/master/android/AndroidOpenGLESLessons/app/src/main/java/com/learnopengles/android/lesson1
    /// lesson4 Java source code is used https://github.com/learnopengles/Learn-OpenGLES-Tutorials/blob/master/android/AndroidOpenGLESLessons/app/src/main/java/com/learnopengles/android/lesson4/LessonFourRenderer.java
    /// lesson RU description http://dedfox.com/izuchaem-opengl-es2-pod-android-urok-3-delaem-osveshhenie-realistichnee-po-tochechnyj-raschet-osveshheniya/
    /// used model https://free3d.com/3d-model/house-43064.html
    /// https://free3d.com/user/blenderister
    /// Old house model
    /// https://free3d.com/3d-model/old-house-2-96599.html
    ///https://free3d.com/user/tharidu
    /// </summary>
    class Renderer : Java.Lang.Object, GLSurfaceView.IRenderer
    {
        /// <summary>
        /// Android activity Context context
        /// </summary>
        private Context context;

        //Store the model matrix. This matrix is used to move models from object space (where each model can be thought of being located at the center of the universe) to world space.
        private float[] mProjectionMatrix = new float[16];
        private float[] mViewMatrix = new float[16];

        private List<GLObject> glObjects = new List<GLObject>();


        // Size of the position data in elements. 
        private int mPositionDataSize = 3;
        // How many bytes per float. 
        private const int mBytesPerFloat = 4;
        // Size of the color data in elements. 
        private int mColorDataSize = 4;

        private float lb = 0.001f;

        private float angleInDegrees = -2.0f;

        // Position the eye behind the origin.
        private float eyeX = 0.0f;
        private float eyeY = 0.0f;
        private float eyeZ = 7.5f;

        // We are looking toward the distance
        private float lookX = 0.0f;
        private float lookY = 0.0f;
        private float lookZ = -7.0f;

        // Set our up vector. This is where our head would be pointing were we holding the camera.
        private float upX = 0.0f;
        private float upY = 1.0f;
        private float upZ = 0.0f;



        public Renderer(Context context) : base()
        {
            this.context = context;
        }

        public void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {

            for (int i = 0; i < 20; i++)
            {
                glObjects.Add(new GLObject(context, "vertex_shader", "fragment_shader", "object1_objvertex", "oldhouse_objnormal", "oldhouse_objtexture", "body"));
                glObjects.Add(new GLObject(context, "vertex_shader", "fragment_shader", "oldhouse_objvertex", "oldhouse_objnormal", "oldhouse_objtexture", "body"));
            }

            Random r = new Random(0xFFFF);
            foreach (GLObject glObject in glObjects)
            {
                glObject.x = r.Next(20);
                glObject.y = -2.5f;
                glObject.z = r.Next(20);
                
            }



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

            eyeZ -= 0.01f;
            lookZ -= 0.01f;

            eyeX += 0.01f;
            lookX += 0.01f;

            Matrix.SetLookAtM(mViewMatrix, 0, eyeX, eyeY, eyeZ, lookX, lookY, lookZ, upX, upY, upZ);


            foreach (GLObject glObject in glObjects)
            {                
                glObject.DrawFrame(gl, mViewMatrix, mProjectionMatrix);
            }



        }

    }
}