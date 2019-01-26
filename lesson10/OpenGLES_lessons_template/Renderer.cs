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

        //Store the model matrix. This matrix is used to move models from object space (where each model can be thought of being located at the center of the universe) to world space.
        private float[] mProjectionMatrix = new float[16];
        private float[] mViewMatrix = new float[16];

        private int mMVPMatrixHandle;

        private int mLightPos;

        private int objectSize;

        // This will be used to pass in model position information. */
        private int mPositionHandle;

        // This will be used to pass in model color information. */
        private int mColorHandle;

        //This will be used to pass in model texture
        private int mTextureCoordHandle;

        private int mTextureHandle;

        private int mNormalHandle;

        private float angleInDegrees = -2.0f;


        // Size of the position data in elements. 
        private int mPositionDataSize = 3;

        // How many bytes per float. 
        private const int mBytesPerFloat = 4;

        // Size of the color data in elements. 
        private int mColorDataSize = 4;

        //3 buffers for vertices, colors, texture UVMap and normals
        private int[] VBOBuffers = new int[4];

        //1 texture handle storage
        private int[] textureHandle = new int[1];

        public Renderer(Context context) : base()
        {
            this.context = context;
        }

        public void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            const float coord = 1.0f;



            //FloatBuffer mTriangleVertices = ByteBuffer.AllocateDirect(triangleVerticesData.Length * mBytesPerFloat).Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            //mTriangleVertices.Put(triangleVerticesData).Flip();

            // Cube colors
            // R, G, B, A
            float[] triangleColorsData = {
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

            FloatBuffer mTriangleColors = ByteBuffer.AllocateDirect(triangleColorsData.Length * mBytesPerFloat).Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            mTriangleColors.Put(triangleColorsData).Flip();

            //Cube texture UV Map
            float[] triangleTextureUVMapData = {
                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f,

                0.0f, 0.0f,
                0.0f, 1.0f,
                1.0f, 0.0f
            };

            FloatBuffer mTriangleTextureUVMap = ByteBuffer.AllocateDirect(triangleTextureUVMapData.Length * mBytesPerFloat).Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            mTriangleTextureUVMap.Put(triangleTextureUVMapData).Flip();

            //triagles normals
            //This normal array is not right, it is spacialy DO FOR demonstrate how normals work with faces when light is calculated at shader program
            float[] triangleNormalData =
                    {												
				// Front face
				0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
				
				// Right face 
				1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
				
				// Back face 
				0.0f, 0.0f, -1.0f,
                0.0f, 0.0f, -1.0f,
                0.0f, 0.0f, -1.0f,
                0.0f, 0.0f, -1.0f,
                0.0f, 0.0f, -1.0f,
                0.0f, 0.0f, -1.0f,
				
				// Left face 
				-1.0f, 0.0f, 0.0f,
                -1.0f, 0.0f, 0.0f,
                -1.0f, 0.0f, 0.0f,
                -1.0f, 0.0f, 0.0f,
                -1.0f, 0.0f, 0.0f,
                -1.0f, 0.0f, 0.0f,
				
				// Top face 
				0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
				
				// Bottom face 
				0.0f, -1.0f, 0.0f,
                0.0f, -1.0f, 0.0f,
                0.0f, -1.0f, 0.0f,
                0.0f, -1.0f, 0.0f,
                0.0f, -1.0f, 0.0f,
                0.0f, -1.0f, 0.0f
            };

            FloatBuffer mTriangleNormal = ByteBuffer.AllocateDirect(triangleNormalData.Length * mBytesPerFloat).Order(ByteOrder.NativeOrder()).AsFloatBuffer();
            mTriangleNormal.Put(triangleNormalData).Flip();

            //Data buffers to VBO
            GLES20.GlGenBuffers(4, VBOBuffers, 0); //2 buffers for vertices, texture and colors



            //--------------------------------------------
            //int resourceId = //context.Resources.GetIdentifier("object1_objvertex", "raw", context.PackageName);
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[0]);
            float[] floatArray;
            long size;
            // Vertex
            FloatBuffer vertexBuffer;            
            Stream fileIn = context.Resources.OpenRawResource(Resource.Raw.OldHouse_objvertex) as Stream;
            MemoryStream m = new MemoryStream();
            fileIn.CopyTo(m);
            size = m.Length;
            floatArray = new float[size / 4];
            objectSize = (int)( size / 4 / 3);
            System.Buffer.BlockCopy(m.ToArray(), 0, floatArray, 0, (int)size);

            vertexBuffer = FloatBuffer.Allocate((int)size / 4); // float array to
            vertexBuffer.Put(floatArray, 0, (int)size / 4);
            vertexBuffer.Flip();

            //VBOManager.setSize(fileName, vertexBuffer.Capacity() / 4); //is size of vertex count = 1 vertex 4 float x,y,z, 1                                
            GLES20.GlBufferData(GLES20.GlArrayBuffer, vertexBuffer.Capacity() * mBytesPerFloat, vertexBuffer, GLES20.GlStaticDraw);
            floatArray = null;
            vertexBuffer = null;
            fileIn.Close();
            m.Close();
            //--------------------------------------------
            //GLES20.GlBufferData(GLES20.GlArrayBuffer, mTriangleVertices.Capacity() * mBytesPerFloat, mTriangleVertices, GLES20.GlStaticDraw);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[1]);
            GLES20.GlBufferData(GLES20.GlArrayBuffer, mTriangleColors.Capacity() * mBytesPerFloat, mTriangleColors, GLES20.GlStaticDraw);


            //Textures -----------------------------------------
            //GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[2]);
            //GLES20.GlBufferData(GLES20.GlArrayBuffer, mTriangleTextureUVMap.Capacity() * mBytesPerFloat, mTriangleTextureUVMap, GLES20.GlStaticDraw);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[2]);
            // Vertex
            
            fileIn = context.Resources.OpenRawResource(Resource.Raw.OldHouse_objtexture) as Stream;
            m = new MemoryStream();
            fileIn.CopyTo(m);
            size = m.Length;
            floatArray = new float[size / 4];
            //objectSize = (int)(size / 4 / 3);
            System.Buffer.BlockCopy(m.ToArray(), 0, floatArray, 0, (int)size);

            vertexBuffer = FloatBuffer.Allocate((int)size / 4); // float array to
            vertexBuffer.Put(floatArray, 0, (int)size / 4);
            vertexBuffer.Flip();

            //VBOManager.setSize(fileName, vertexBuffer.Capacity() / 4); //is size of vertex count = 1 vertex 4 float x,y,z, 1                                
            GLES20.GlBufferData(GLES20.GlArrayBuffer, vertexBuffer.Capacity() * mBytesPerFloat, vertexBuffer, GLES20.GlStaticDraw);
            floatArray = null;
            vertexBuffer = null;
            fileIn.Close();
            m.Close();


            //ENDOF Textures -----------------------------------------

            // GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[3]);
            /// GLES20.GlBufferData(GLES20.GlArrayBuffer, mTriangleNormal.Capacity() * mBytesPerFloat, mTriangleNormal, GLES20.GlStaticDraw);
            /// Normales
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[3]);
            // Vertex

            fileIn = context.Resources.OpenRawResource(Resource.Raw.OldHouse_objnormal) as Stream;
            m = new MemoryStream();
            fileIn.CopyTo(m);
            size = m.Length;
            floatArray = new float[size / 4];
            //objectSize = (int)(size / 4 / 3);
            System.Buffer.BlockCopy(m.ToArray(), 0, floatArray, 0, (int)size);

            vertexBuffer = FloatBuffer.Allocate((int)size / 4); // float array to
            vertexBuffer.Put(floatArray, 0, (int)size / 4);
            vertexBuffer.Flip();

            //VBOManager.setSize(fileName, vertexBuffer.Capacity() / 4); //is size of vertex count = 1 vertex 4 float x,y,z, 1                                
            GLES20.GlBufferData(GLES20.GlArrayBuffer, vertexBuffer.Capacity() * mBytesPerFloat, vertexBuffer, GLES20.GlStaticDraw);
            floatArray = null;
            vertexBuffer = null;
            fileIn.Close();
            m.Close();


            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);

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
            GLES20.GlClearColor(0.0f, 0.0f, 0.5f, 0.0f);
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
            float upY = coord;
            float upZ = 0.0f;

            // Set the view matrix. This matrix can be said to represent the camera position.
            // NOTE: In OpenGL 1, a ModelView matrix is used, which is a combination of a model and
            // view matrix. In OpenGL 2, we can keep track of these matrices separately if we choose.
            Matrix.SetLookAtM(mViewMatrix, 0, eyeX, eyeY, eyeZ, lookX, lookY, lookZ, upX, upY, upZ);

            //all "attribute" variables is "triagles" VBO (arrays) items representation
            //a_Possition[0] <=> a_Color[0] <=> a_TextureCoord[0] <=> a_Normal[0]
            //a_Possition[1] <=> a_Color[1] <=> a_TextureCoord[1] <=> a_Normal[1]
            //...
            //a_Possition[n] <=> a_Color[n] <=> a_TextureCoord[n] <=> a_Normal[n] -- where "n" is object buffers length
            //-> HOW MANY faces in your object (model) in VBO -> how many times the vertex shader will be called by OpenGL
            string vertexShader =
                    "uniform mat4 u_MVPMatrix;      \n"     // A constant representing the combined model/view/projection matrix.
                  + "uniform vec3 u_LightPos;       \n"     // A constant representing the light source position
                  + "attribute vec4 a_Position;     \n"     // Per-vertex position information we will pass in. (it means vec4[x,y,z,w] but we put only x,y,z at this sample
                  + "attribute vec4 a_Color;        \n"     // Per-vertex color information we will pass in.			  
                  + "varying vec4 v_Color;          \n"     // This will be passed into the fragment shader.
                  + "attribute vec2 a_TextureCoord; \n"     // Per-vertex texture UVMap information we will pass in.			  
                  + "varying vec2 v_TextureCoord;   \n"     // This will be passed into the fragment shader.
                  + "attribute vec3 a_Normal;       \n"     // Per-vertex normals information we will pass in.			  
                  + "void main()                    \n"     // The entry point for our vertex shader.
                  + "{                              \n"
                  //light calculation section for fragment shader 
                  + "   vec3 modelViewVertex = vec3(u_MVPMatrix * a_Position);\n"
                  + "   vec3 modelViewNormal = vec3(u_MVPMatrix * vec4(a_Normal, 0.0));\n"
                  + "   float distance = length(u_LightPos - modelViewVertex);\n"
                  + "   vec3 lightVector = normalize(u_LightPos - modelViewVertex);\n"
                  + "   float diffuse = max(dot(modelViewNormal, lightVector), 0.1);\n"
                  + "   diffuse = diffuse * (1.0 / (1.0 + (0.7 * distance * distance)))+0.5;\n"
                  + "   v_Color = vec4(diffuse);\n"   //Pass the color with light aspect to fragment shader                  
                  + "   v_TextureCoord = a_TextureCoord; \n"     // Pass the texture coordinate through to the fragment shader. It will be interpolated across the triangle.                                                                              
                  + "   gl_Position = u_MVPMatrix   \n"     // gl_Position is a special variable used to store the final position.
                  + "                 * a_Position; \n"     // Multiply the vertex by the matrix to get the final point in normalized screen coordinates.			                                            			 
                  + "}                              \n";

            string fragmentShader =
                "precision mediump float;       \n"     // Set the default precision to medium. We don't need as high of a 
                                                        // precision in the fragment shader.				
              + "varying vec4 v_Color;          \n"     // This is the color from the vertex shader interpolated across the triangle per fragment.			                
              + "varying vec2 v_TextureCoord;   \n"     // This is the texture coordinate from the vertex shader interpolated across the triangle per fragment.			                
              + "uniform sampler2D u_Texture;   \n"     // This is the texture image handler 
              + "void main()                    \n"     // The entry point for our fragment shader.
              + "{                              \n"
              + "   gl_FragColor = texture2D(u_Texture, v_TextureCoord) * v_Color;  \n"   // Pass the color directly through the pipeline.		                
            //  + "   gl_FragColor = texture2D(u_Texture, v_TextureCoord);  \n"   // Pass the color directly through the pipeline.		                
              + "}                              \n";

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
                GLES20.GlBindAttribLocation(programHandle, 3, "a_Normal");

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
            mLightPos = GLES20.GlGetUniformLocation(programHandle, "u_LightPos");
            mPositionHandle = GLES20.GlGetAttribLocation(programHandle, "a_Position");
            mColorHandle = GLES20.GlGetAttribLocation(programHandle, "a_Color");
            mTextureCoordHandle = GLES20.GlGetAttribLocation(programHandle, "a_TextureCoord");
            mNormalHandle = GLES20.GlGetAttribLocation(programHandle, "a_Normal");
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

            // Draw the triangle facing straight on.
            angleInDegrees += 0.1f;
            //Prepare model transformation matrix
            float[] mModelMatrix = new float[16];
            Matrix.SetIdentityM(mModelMatrix, 0);            
            Matrix.RotateM(mModelMatrix, 0, angleInDegrees, 0.0f, 0.4f, 0.0f);
            Matrix.TranslateM(mModelMatrix, 0, 0.0f, -2.4f, 0.0f);
            Matrix.ScaleM(mModelMatrix, 0, 0.02f, 0.02f, 0.02f);

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

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[3]);
            GLES20.GlEnableVertexAttribArray(mNormalHandle);
            GLES20.GlVertexAttribPointer(mNormalHandle, 3, GLES20.GlFloat, false, 0, 0);

            GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, textureHandle[0]);
            GLES20.GlUniform1i(mTextureHandle, 0);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);
            //END OF Draw with VBO 

            //light position
            // GLES20.GlUniform3f(mLightPos, 0.0f, 0.0f, angleInDegrees);
            GLES20.GlUniform3f(mLightPos, 0.0f, 0.0f, 0.0f);

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

            GLES20.GlDrawArrays(GLES20.GlTriangles, 0, objectSize); //Cube has 12 triagle faces each face has 3 coord


        }
    }
}