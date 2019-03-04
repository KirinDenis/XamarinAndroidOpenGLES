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
    public class Camera
    {
        //Store the model matrix. This matrix is used to move models from object space (where each model can be thought of being located at the center of the universe) to world space.
        public float[] mProjectionMatrix = new float[16];
        public float[] mViewMatrix = new float[16];

        // Position the eye behind the origin.
        public float eyeX = 0.0f;
        public float eyeY = 0.0f;
        public float eyeZ = -4.0f;

        // We are looking toward the distance
        public float lookX = 0.0f;
        public float lookY = 0.0f;
        public float lookZ = 0.0f;

        // Set our up vector. This is where our head would be pointing were we holding the camera.
        public float upX = 0.0f;
        public float upY = 1.0f;
        public float upZ = 1.0f;

        public Camera(int width, int height)
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
            //Alternative
            //Matrix.PerspectiveM(mProjectionMatrix, 0, 10.0f, ratio, near, far);
        }

        public void OnDrawFrame()
        {
            // Set the view matrix. This matrix can be said to represent the camera position.
            // NOTE: In OpenGL 1, a ModelView matrix is used, which is a combination of a model and
            // view matrix. In OpenGL 2, we can keep track of these matrices separately if we choose.
            Matrix.SetLookAtM(mViewMatrix, 0, eyeX, eyeY, eyeZ, lookX, lookY, lookZ, upX, upY, upZ);
        }
    }
}