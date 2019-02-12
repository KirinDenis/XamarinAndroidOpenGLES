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

namespace SGWW
{
    public class Shader
    {
        private Context context;
        private string vertexShaderFile;
        private string fragmentShaderFile;

        public int programHandle;

        // This will be used to pass in model position information. */
        public int mPositionHandle;

        // This will be used to pass in model color information. */
        public int mColorHandle;

        //This will be used to pass in model texture
        public int mTextureCoordHandle;

        public int mTextureHandle;

        public int mNormalHandle;

        public int mMVPMatrixHandle;

        public int mLightPos;

        public Shader(Context context, string vertexShaderFile, string fragmentShaderFile)
        {
            this.context = context;
            this.vertexShaderFile = vertexShaderFile;
            this.fragmentShaderFile = fragmentShaderFile;
            
        }

        public string Compile()
        {
            string result = string.Empty;

            string vertexShader = string.Empty;
            string fragmentShader = string.Empty;

            int resourceId = context.Resources.GetIdentifier(vertexShaderFile, "raw", context.PackageName);
            Stream fileStream = context.Resources.OpenRawResource(resourceId);
            StreamReader streamReader = new StreamReader(fileStream);

            string line = string.Empty;
            while ((line = streamReader.ReadLine()) != null)
            {
                vertexShader += line + "\n";
            }

            resourceId = context.Resources.GetIdentifier(fragmentShaderFile, "raw", context.PackageName);
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
                    result += "vertex shader error";
                    result += GLES20.GlGetProgramInfoLog(vertexShaderHandle);
                    GLES20.GlDeleteShader(vertexShaderHandle);
                    vertexShaderHandle = 0;                 
                }
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
                    result += "fragment shader error";
                    result += GLES20.GlGetProgramInfoLog(fragmentShaderHandle);
                    GLES20.GlDeleteShader(fragmentShaderHandle);
                    fragmentShaderHandle = 0;
                    return result;
                }
            }

            // Create a program object and store the handle to it.
            programHandle = GLES20.GlCreateProgram();

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
                    result += "shader link error";
                    result += GLES20.GlGetProgramInfoLog(programHandle);
                    GLES20.GlDeleteProgram(programHandle);
                    programHandle = 0;
                    return result;
                }
            }


            // Set program handles. These will later be used to pass in values to the program.
            mMVPMatrixHandle = GLES20.GlGetUniformLocation(programHandle, "u_MVPMatrix");
            mLightPos = GLES20.GlGetUniformLocation(programHandle, "u_LightPos");
            mPositionHandle = GLES20.GlGetAttribLocation(programHandle, "a_Position");
            mColorHandle = GLES20.GlGetAttribLocation(programHandle, "a_Color");
            mTextureCoordHandle = GLES20.GlGetAttribLocation(programHandle, "a_TextureCoord");
            mNormalHandle = GLES20.GlGetAttribLocation(programHandle, "a_Normal");
            mTextureHandle = GLES20.GlGetUniformLocation(programHandle, "u_Texture");

            return result; 
         
        }
    }
}