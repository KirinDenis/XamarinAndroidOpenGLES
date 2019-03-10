using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio;
using Javax.Microedition.Khronos.Opengles;

namespace SGWW
{
    public class VladimirRender3VBO : Java.Lang.Object, GLSurfaceView.IRenderer
    {
        /// <summary>
        /// Android activity Context context
        /// </summary>
        private Context context;

        //Store the model matrix. This matrix is used to move models from object space (where each model can be thought of being located at the center of the universe) to world space.
        private float[] mProjectionMatrix = new float[16];
        private float[] mViewMatrix = new float[16];

        private int mMVPMatrixHandle;

        // This will be used to pass in model position information. */
        private int mPositionHandle;

        // This will be used to pass in model color information. */
        private int mColorHandle;

        //This will be used to pass in model texture
        private int mTextureCoordHandle;

        private int mTextureHandle;

        private float angleInDegrees = 0;


        // Size of the position data in elements. 
        private int mPositionDataSize = 3;

        // How many bytes per float. 
        private const int mBytesPerFloat = 4;

        // Size of the color data in elements. 
        private int mColorDataSize = 4;

        //3 buffers for vertices, colors and texture UVMap
        private int[] VBOBuffers = new int[3];

        //1 texture handle storage
        private int[] textureHandle = new int[1];

        float[] modelVerticesData;
        float[] modelTextureUVMapData;

        //----

        public float angerX;
        public float angerY;

        public float scale = 0.2f;

        public VladimirRender3VBO(Context context) : base()
        {
            this.context = context;
        }

        public void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            const float coord = 1.0f;

            ObjParser model3D = new ObjParser();

            List<byte[]> test1 = model3D.ParsedObject(context, "buggy");             

            float[] vertexArray = new float[test1[0].Length / 4];
            System.Buffer.BlockCopy(test1[0], 0, vertexArray, 0, (int)test1[0].Length);

            modelVerticesData = vertexArray;

            FloatBuffer mTriangleVertices = ByteBuffer.AllocateDirect(modelVerticesData.Length * mBytesPerFloat).Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            mTriangleVertices.Put(modelVerticesData).Flip();

            // Cube colors
            // R, G, B, A
            float[] modelColorsData = {
                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                1.0f, 0.0f, 0.0f, 0.5f,
                0.0f, 0.5f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f

            };

            FloatBuffer mTriangleColors = ByteBuffer.AllocateDirect(modelColorsData.Length * mBytesPerFloat).Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            mTriangleColors.Put(modelColorsData).Flip();


            float[] textureUVMapArray = new float[test1[1].Length / 4];
            System.Buffer.BlockCopy(test1[1], 0, textureUVMapArray, 0, (int)test1[1].Length);

            modelTextureUVMapData = textureUVMapArray;

            FloatBuffer mTriangleTextureUVMap = ByteBuffer.AllocateDirect(modelTextureUVMapData.Length * mBytesPerFloat).Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            mTriangleTextureUVMap.Put(modelTextureUVMapData).Flip();



            //Data buffers to VBO
            GLES20.GlGenBuffers(3, VBOBuffers, 0); //2 buffers for vertices, texture and colors

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[0]);
            GLES20.GlBufferData(GLES20.GlArrayBuffer, mTriangleVertices.Capacity() * mBytesPerFloat, mTriangleVertices, GLES20.GlStaticDraw);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[1]);
            GLES20.GlBufferData(GLES20.GlArrayBuffer, mTriangleColors.Capacity() * mBytesPerFloat, mTriangleColors, GLES20.GlStaticDraw);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[2]);
            GLES20.GlBufferData(GLES20.GlArrayBuffer, mTriangleTextureUVMap.Capacity() * mBytesPerFloat, mTriangleTextureUVMap, GLES20.GlStaticDraw);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);

            //Load and setup texture

            GLES20.GlGenTextures(1, textureHandle, 0); //init 1 texture storage handle 
            if (textureHandle[0] != 0)
            {
                //Android.Graphics cose class Matrix exists at both Android.Graphics and Android.OpenGL and this is only sample of using 
                Android.Graphics.BitmapFactory.Options options = new Android.Graphics.BitmapFactory.Options();
                options.InScaled = false; // No pre-scaling
                Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.iam, options);
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
            GLES20.GlClearColor(coord, coord, coord, coord);
            // GLES20.GlEnable(GLES20.GlDepthTest); //uncoment if needs enabled dpeth test
            GLES20.GlEnable(2884); // GlCullFace == 2884 see OpenGL documentation to this constant value  
            GLES20.GlCullFace(GLES20.GlBack);


            // Position the eye behind the origin.
            float eyeX = 0.0f;
            float eyeY = 0.0f;
            float eyeZ = 4.5f;

            // We are looking toward the distance
            float lookX = 0.0f;
            float lookY = 0.0f;
            float lookZ = -5.0f;

            // Set our up vector. This is where our head would be pointing were we holding the camera.
            float upX = 0.0f;
            float upY = coord;
            float upZ = 0.0f;

            // Set the view matrix. This matrix can be said to represent the camera position.
            // NOTE: In OpenGL 1, a ModelView matrix is used, which is a combination of a model and
            // view matrix. In OpenGL 2, we can keep track of these matrices separately if we choose.
            Matrix.SetLookAtM(mViewMatrix, 0, eyeX, eyeY, eyeZ, lookX, lookY, lookZ, upX, upY, upZ);

            string vertexShader =
                    "uniform mat4 u_MVPMatrix;      \n"     // A constant representing the combined model/view/projection matrix.
                  + "attribute vec4 a_Position;     \n"     // Per-vertex position information we will pass in.
                  + "attribute vec4 a_Color;        \n"     // Per-vertex color information we will pass in.			  
                  + "varying vec4 v_Color;          \n"     // This will be passed into the fragment shader.
                  + "attribute vec2 a_TextureCoord; \n"
                  + "varying vec2 v_TextureCoord;   \n"
                  + "void main()                    \n"     // The entry point for our vertex shader.
                  + "{                              \n"
                  + "   v_TextureCoord = a_TextureCoord; \n"     // Pass the color through to the fragment shader. It will be interpolated across the triangle.                                                            
                  + "   v_Color = a_Color;          \n"     // Pass the color through to the fragment shader. It will be interpolated across the triangle.                                                            
                  + "   gl_Position = u_MVPMatrix   \n"     // gl_Position is a special variable used to store the final position.
                  + "                 * a_Position; \n"     // Multiply the vertex by the matrix to get the final point in normalized screen coordinates.			                                            			 
                  + "}                              \n";

            string fragmentShader =
                "precision mediump float;       \n"     // Set the default precision to medium. We don't need as high of a 
                                                        // precision in the fragment shader.				
              + "varying vec4 v_Color;          \n"     // This is the color from the vertex shader interpolated across the triangle per fragment.			  
              + "varying vec2 v_TextureCoord;   \n"
              + "uniform sampler2D u_Texture;   \n"
              + "void main()                    \n"     // The entry point for our fragment shader.
              + "{                              \n"
              + "   gl_FragColor = texture2D(u_Texture, v_TextureCoord);  \n"   // Pass the color directly through the pipeline.		  
              + "}                              \n";


            vertexShader = string.Empty;
            fragmentShader = string.Empty;

            int resourceId = context.Resources.GetIdentifier("vertexshadervladimir1", "raw", context.PackageName);
            Stream fileStream = context.Resources.OpenRawResource(resourceId);
            StreamReader streamReader = new StreamReader(fileStream);

            string line = string.Empty;
            while ((line = streamReader.ReadLine()) != null)
            {
                vertexShader += line + "\n";
            }

            resourceId = context.Resources.GetIdentifier("fragmentshadervladimir1", "raw", context.PackageName);
            fileStream = context.Resources.OpenRawResource(resourceId);
            streamReader = new StreamReader(fileStream);
            while ((line = streamReader.ReadLine()) != null)
            {
                fragmentShader += line + "\n";
            }

            int vertexShaderHandle = GLES20.GlCreateShader(GLES20.GlVertexShader);

            if (vertexShaderHandle != 0)
            {
                // Pass in the shader source.
                GLES20.GlShaderSource(vertexShaderHandle, vertexShader);

                // Compile the shader.
                GLES20.GlCompileShader(vertexShaderHandle);

                // Get the compilation status.
                int[] compileStatus = new int[1];
                GLES20.GlGetShaderiv(vertexShaderHandle, GLES20.GlCompileStatus, compileStatus, 0);

                // If the compilation failed, delete the shader.
                if (compileStatus[0] == 0)
                {
                    GLES20.GlDeleteShader(vertexShaderHandle);
                    vertexShaderHandle = 0;
                }
            }

            if (vertexShaderHandle == 0)
            {
                throw new Exception("Error creating vertex shader.");
            }

            // Load in the fragment shader shader.
            int fragmentShaderHandle = GLES20.GlCreateShader(GLES20.GlFragmentShader);

            if (fragmentShaderHandle != 0)
            {
                // Pass in the shader source.
                GLES20.GlShaderSource(fragmentShaderHandle, fragmentShader);

                // Compile the shader.
                GLES20.GlCompileShader(fragmentShaderHandle);

                // Get the compilation status.
                int[] compileStatus = new int[1];
                GLES20.GlGetShaderiv(fragmentShaderHandle, GLES20.GlCompileStatus, compileStatus, 0);

                // If the compilation failed, delete the shader.
                if (compileStatus[0] == 0)
                {
                    GLES20.GlDeleteShader(fragmentShaderHandle);
                    fragmentShaderHandle = 0;
                }
            }

            if (fragmentShaderHandle == 0)
            {
                throw new Exception("Error creating fragment shader.");
            }

            // Create a program object and store the handle to it.
            int programHandle = GLES20.GlCreateProgram();

            if (programHandle != 0)
            {
                // Bind the vertex shader to the program.
                GLES20.GlAttachShader(programHandle, vertexShaderHandle);

                // Bind the fragment shader to the program.
                GLES20.GlAttachShader(programHandle, fragmentShaderHandle);

                // Bind attributes
                GLES20.GlBindAttribLocation(programHandle, 0, "a_Position");
                GLES20.GlBindAttribLocation(programHandle, 1, "a_Color");
                GLES20.GlBindAttribLocation(programHandle, 2, "a_TextureCoord");

                // Link the two shaders together into a program.
                GLES20.GlLinkProgram(programHandle);

                // Get the link status.
                int[] linkStatus = new int[1];
                GLES20.GlGetProgramiv(programHandle, GLES20.GlLinkStatus, linkStatus, 0);

                // If the link failed, delete the program.
                if (linkStatus[0] == 0)
                {
                    GLES20.GlDeleteProgram(programHandle);
                    programHandle = 0;
                }
            }

            if (programHandle == 0)
            {
                throw new Exception("Error creating program.");
            }

            // Set program handles. These will later be used to pass in values to the program.
            mMVPMatrixHandle = GLES20.GlGetUniformLocation(programHandle, "u_MVPMatrix");
            mPositionHandle = GLES20.GlGetAttribLocation(programHandle, "a_Position");
            mColorHandle = GLES20.GlGetAttribLocation(programHandle, "a_Color");
            mTextureCoordHandle = GLES20.GlGetAttribLocation(programHandle, "a_TextureCoord");
            mTextureHandle = GLES20.GlGetUniformLocation(programHandle, "u_Texture");


            // Tell OpenGL to use this program when rendering.
            GLES20.GlUseProgram(programHandle);

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
                       
            
            //Prepare model transformation matrix
            float[] mModelMatrix = new float[16];
            Matrix.SetIdentityM(mModelMatrix, 0);
            Matrix.RotateM(mModelMatrix, 0, angerX, 1.0f, 0.0f, 0.0f);
            Matrix.RotateM(mModelMatrix, 0, angerY, 0.0f, 1.0f, 0.0f);
            Matrix.ScaleM(mModelMatrix, 0, scale, scale, scale);


            //Draw with VBO 
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[0]);
            GLES20.GlEnableVertexAttribArray(mPositionHandle);
            GLES20.GlVertexAttribPointer(mPositionHandle, mPositionDataSize, GLES20.GlFloat, false, 0, 0);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[1]);
            GLES20.GlEnableVertexAttribArray(mColorHandle);
            GLES20.GlVertexAttribPointer(mColorHandle, mColorDataSize, GLES20.GlFloat, false, 0, 0);


            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[2]);
            GLES20.GlEnableVertexAttribArray(mTextureCoordHandle);

            GLES20.GlVertexAttribPointer(mTextureCoordHandle, 2, GLES20.GlFloat, false, 0, 0);

            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, textureHandle[0]);
            GLES20.GlUniform1i(mTextureHandle, 0);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);
            //END OF Draw with VBO 


            // This multiplies the view matrix by the model matrix, and stores the result in the MVP matrix
            // (which currently contains model * view).            
            // Allocate storage for the final combined matrix. This will be passed into the shader program. */
            float[] mMVPMatrix = new float[16];
            Matrix.MultiplyMM(mMVPMatrix, 0, mViewMatrix, 0, mModelMatrix, 0);

            // This multiplies the modelview matrix by the projection matrix, and stores the result in the MVP matrix
            // (which now contains model * view * projection).
            // THIS IS NOT WORK AT C# Matrix class -> Matrix.MultiplyMM(mMVPMatrix, 0, mProjectionMatrix, 0, mMVPMatrix, 0);
            float[] _mMVPMatrix = new float[16];
            Matrix.MultiplyMM(_mMVPMatrix, 0, mProjectionMatrix, 0, mMVPMatrix, 0);

            GLES20.GlUniformMatrix4fv(mMVPMatrixHandle, 1, false, _mMVPMatrix, 0);

            GLES20.GlDrawArrays(GLES20.GlTriangles, 0, modelVerticesData.Length/3); //Cube has 12 triagle faces each face has 3 coord


        }
    }
}