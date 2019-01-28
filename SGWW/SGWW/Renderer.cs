using System;
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
        private Shader shader;
        private Shader lineshader;
        private VBO vertexVBO;
        private VBO normalVBO;
        private VBO textureVBO;


        //Store the model matrix. This matrix is used to move models from object space (where each model can be thought of being located at the center of the universe) to world space.
        private float[] mProjectionMatrix = new float[16];
        private float[] mViewMatrix = new float[16];

        // Size of the position data in elements. 
        private int mPositionDataSize = 3;
        // How many bytes per float. 
        private const int mBytesPerFloat = 4;
        // Size of the color data in elements. 
        private int mColorDataSize = 4;

        //1 texture handle storage
        private int[] textureHandle = new int[1];

        private float lb = 0.001f;

        private float angleInDegrees = -2.0f;


        public Renderer(Context context) : base()
        {
            this.context = context;
        }

        public void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            vertexVBO = new VBO(context, "oldhouse_objvertex");
            normalVBO = new VBO(context, "oldhouse_objnormal");
            textureVBO = new VBO(context, "oldhouse_objtexture");

            //Load and setup texture

            GLES20.GlGenTextures(1, textureHandle, 0); //init 1 texture storage handle 
            if (textureHandle[0] != 0)
            {
                //Android.Graphics cose class Matrix exists at both Android.Graphics and Android.OpenGL and this is only sample of using 
                Android.Graphics.BitmapFactory.Options options = new Android.Graphics.BitmapFactory.Options();
                options.InScaled = false; // No pre-scaling
                Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.body, options);
                GLES20.GlBindTexture(GLES20.GlTexture2d, textureHandle[0]);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter, GLES20.GlNearest);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter, GLES20.GlNearest);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS, GLES20.GlClampToEdge);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT, GLES20.GlClampToEdge);
                GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bitmap, 0);
                bitmap.Recycle();
            }

            //Ask android to run RAM garbage cleaner
            System.GC.Collect();

            //Setup OpenGL ES 
            GLES20.GlClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GLES20.GlEnable(GLES20.GlDepthTest); //uncoment if needs enabled dpeth test
                                                 //  GLES20.GlEnable(2884); // GlCullFace == 2884 see OpenGL documentation to this constant value  
                                                 //  GLES20.GlCullFace(GLES20.GlFront);


            // Position the eye behind the origin.
            float eyeX = 0.0f;
            float eyeY = 0.0f;
            float eyeZ = 7.5f;

            // We are looking toward the distance
            float lookX = 0.0f;
            float lookY = 0.0f;
            float lookZ = -7.0f;

            // Set our up vector. This is where our head would be pointing were we holding the camera.
            float upX = 0.0f;
            float upY = 1.0f;
            float upZ = 0.0f;

            // Set the view matrix. This matrix can be said to represent the camera position.
            // NOTE: In OpenGL 1, a ModelView matrix is used, which is a combination of a model and
            // view matrix. In OpenGL 2, we can keep track of these matrices separately if we choose.
            Matrix.SetLookAtM(mViewMatrix, 0, eyeX, eyeY, eyeZ, lookX, lookY, lookZ, upX, upY, upZ);

            shader = new Shader(context, "vertex_shader", "fragment_shader");
            string compileResult = shader.Compile();

            lineshader = new Shader(context, "vertex_line_shader", "fragment_line_shader");
            compileResult = lineshader.Compile();

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
            float far = 20.0f;
            Matrix.FrustumM(mProjectionMatrix, 0, left, right, bottom, top, near, far);
        }

        public void OnDrawFrame(IGL10 gl)
        {

            GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit);
            // Draw the triangle facing straight on.
            angleInDegrees += 0.1f;
            lb += 0.0001f;
            if (lb > 0.05f) lb = 0.001f;

            for (int x = -2; x < 3; x++)
            {
                for (int z = -2; z < 3; z++)
                {
                    DrawObject(x * 10.0f, 0.0f, z * 10.0f);
                }
            }
        }

        private void DrawObject(float x, float y, float z)
        {
            //Prepare model transformation matrix
            float[] mModelMatrix = new float[16];
            Matrix.SetIdentityM(mModelMatrix, 0);
            //Matrix.ScaleM(mModelMatrix, 0, 0.2f, 0.2f, 0.2f);
            Matrix.RotateM(mModelMatrix, 0, angleInDegrees, 0.0f, 0.4f, 0.0f);
            Matrix.TranslateM(mModelMatrix, 0, x, y + -2.5f, z);

            // Tell OpenGL to use this program when rendering.

            GLES20.GlUseProgram(shader.programHandle);

            //Draw with VBO 
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, vertexVBO.handle);
            GLES20.GlEnableVertexAttribArray(shader.mPositionHandle);
            GLES20.GlVertexAttribPointer(shader.mPositionHandle, mPositionDataSize, GLES20.GlFloat, false, 0, 0);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, textureVBO.handle);
            GLES20.GlEnableVertexAttribArray(shader.mTextureCoordHandle);
            GLES20.GlVertexAttribPointer(shader.mTextureCoordHandle, 2, GLES20.GlFloat, false, 0, 0);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, normalVBO.handle);
            GLES20.GlEnableVertexAttribArray(shader.mNormalHandle);
            GLES20.GlVertexAttribPointer(shader.mNormalHandle, 3, GLES20.GlFloat, false, 0, 0);

            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, textureHandle[0]);
            GLES20.GlUniform1i(shader.mTextureHandle, 0);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);
            //END OF Draw with VBO 

            //light position            
            GLES20.GlUniform4f(shader.mLightPos, 0.0f, 6.0f - 2.5f, 0.0f, 0.001f);

            // This multiplies the view matrix by the model matrix, and stores the result in the MVP matrix
            // (which currently contains model * view).            
            // Allocate storage for the final combined matrix. This will be passed into the shader program.

            float[] mMVPMatrix = new float[16];
            Matrix.MultiplyMM(mMVPMatrix, 0, mViewMatrix, 0, mModelMatrix, 0);

            // This multiplies the modelview matrix by the projection matrix, and stores the result in the MVP matrix
            // (which now contains model * view * projection).
            // THIS IS NOT WORK AT C# Matrix class -> Matrix.MultiplyMM(mMVPMatrix, 0, mProjectionMatrix, 0, mMVPMatrix, 0);
            float[] _mMVPMatrix = new float[16];
            Matrix.MultiplyMM(_mMVPMatrix, 0, mProjectionMatrix, 0, mMVPMatrix, 0);

            GLES20.GlUniformMatrix4fv(shader.mMVPMatrixHandle, 1, false, _mMVPMatrix, 0);
            GLES20.GlDrawArrays(GLES20.GlTriangles, 0, vertexVBO.objectSize); //Cube has 12 triagle faces each face has 3 coord


            // Draw with GlLines (green lines shaders)
            GLES20.GlUseProgram(lineshader.programHandle);
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, vertexVBO.handle);
            GLES20.GlEnableVertexAttribArray(lineshader.mPositionHandle);
            GLES20.GlVertexAttribPointer(lineshader.mPositionHandle, mPositionDataSize, GLES20.GlFloat, false, 0, 0);
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);
            GLES20.GlUniformMatrix4fv(lineshader.mMVPMatrixHandle, 1, false, _mMVPMatrix, 0);
            GLES20.GlDrawArrays(GLES20.GlLines, 0, vertexVBO.objectSize); //Cube has 12 triagle faces each face has 3 coord



        }
    }
}