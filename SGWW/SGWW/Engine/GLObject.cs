using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Javax.Microedition.Khronos.Opengles;

namespace SGWW
{
    public class GLObject
    {
        private Shader shader;
        private VBO vertexVBO;
        private VBO normalVBO;
        private VBO textureVBO;
        private Texture texture;
        private int mPositionDataSize = 3;

        public string compileResult = string.Empty;

        public float x, y, z, angle, ay, az;

        public float ax = 1.0f;
        public float scale = 1.0f;

        /// <summary>
        /// temporary light position
        /// </summary>
        public float lx = 0.65f;
        public float ly = 1.0f;
        public float lz = -3.0f;
        public float lw = 0.01f;

        /// <summary>
        /// Object init on create, loding and compile shaders, VBOs and textures from resources
        /// </summary>
        /// <param name="context">current activity Context</param>
        /// <param name="vertextShader">raw/shader file name</param>
        /// <param name="fragmentShader">raw/shader file name</param>
        /// <param name="vertexFile">raw/vertex file name_objvertex</param>
        /// <param name="normalFile">raw/normal file name_objnormal</param>
        /// <param name="textureFile">raw/UVMap file name_objtexture</param>
        /// <param name="textureImage">drawable/file name (use .PNG files with alpha chanel is allowed)</param>
        public GLObject(Context context, string vertextShader, string fragmentShader, string vertexFile, string normalFile, string textureFile, string textureImage)
        {
            //Loading vertex and fragment shader source codes from resource files, than compile and link it
            shader = new Shader(context, vertextShader, fragmentShader);
            compileResult = shader.Compile();
            //Loading vertexes from resource file to VBO
            vertexVBO = new VBO(context, vertexFile);
            //Loading UVMap from resource file to VBO
            textureVBO = new VBO(context, textureFile);
            //Loading normales from resource file to VBO
            normalVBO = new VBO(context, normalFile);
            //Loading texture image file
            texture = new Texture(context, textureImage);
            //Ask android to run RAM garbage cleaner
            System.GC.Collect();
        }

        /// <summary>
        /// This method is called from Renderer when OpenGL ready to draw scene 
        /// </summary>
        /// <param name="gl">IGL10 access object pointer from orign OpenGL.OnDraw(IGL10 gl)</param>
        /// <param name="mViewMatrix">Camere View matrix</param>
        /// <param name="mProjectionMatrix">Camera Projection matrix</param>
        public void DrawFrame(IGL10 gl, float[] mViewMatrix, float[] mProjectionMatrix)
        {

            float[] mModelMatrix = new float[16];
            Matrix.SetIdentityM(mModelMatrix, 0);
            Matrix.ScaleM(mModelMatrix, 0, scale, scale, scale);
            Matrix.RotateM(mModelMatrix, 0, angle, ax, ay, az);
            Matrix.TranslateM(mModelMatrix, 0, x, y, z);

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
            GLES20.GlBindTexture(GLES20.GlTexture2d, texture.handle);
            GLES20.GlUniform1i(shader.mTextureHandle, 0);

            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);
            //END OF Draw with VBO 

            //light position            
            GLES20.GlUniform4f(shader.mLightPos, lx, ly, lz, lw);

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
            

            GLES20.GlUseProgram(0);
        }

    }
}