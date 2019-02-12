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

        public GLObject(Context context, string vertextShader, string fragmentShader, string vertexFile, string normalFile, string textureFile, string textureImage)
        {
            shader = new Shader(context, vertextShader, fragmentShader);
            compileResult = shader.Compile();

            vertexVBO = new VBO(context, vertexFile);
            normalVBO = new VBO(context, normalFile);
            textureVBO = new VBO(context, textureFile);
            texture = new Texture(context, textureImage);

            //Ask android to run RAM garbage cleaner
            System.GC.Collect();

        }

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

            GLES20.GlUseProgram(0);
        }

    }
}