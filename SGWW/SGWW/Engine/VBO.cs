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

namespace SGWW
{
    public class VBO
    {
        private int[] VBOBuffers = new int[1];
        private const int mBytesPerFloat = 4;
        public int handle = -1;
        public int objectSize = -1;

        public VBO(Context context, string fileName)
        {

            if (string.IsNullOrEmpty(fileName)) return;
            //GLES20.GlGenBuffers(1, VBOBuffers, 0);
            //GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[0]);
            //float[] floatArray;
            long size;
            // Vertex
            //FloatBuffer vertexBuffer;
            int resourceId = context.Resources.GetIdentifier(fileName, "raw", context.PackageName);
            Stream fileIn = context.Resources.OpenRawResource(resourceId) as Stream;
            MemoryStream m = new MemoryStream();
            fileIn.CopyTo(m);
            size = m.Length;
            //floatArray = new float[size / 4];
            

            ArrayToVBO(m.ToArray(), (int)size);
            //System.Buffer.BlockCopy(m.ToArray(), 0, floatArray, 0, (int)size);

            //vertexBuffer = FloatBuffer.Allocate((int)size / 4); // float array to
            //vertexBuffer.Put(floatArray, 0, (int)size / 4);
            //vertexBuffer.Flip();

            //VBOManager.setSize(fileName, vertexBuffer.Capacity() / 4); //is size of vertex count = 1 vertex 4 float x,y,z, 1                                
            //GLES20.GlBufferData(GLES20.GlArrayBuffer, vertexBuffer.Capacity() * mBytesPerFloat, vertexBuffer, GLES20.GlStaticDraw);
            //floatArray = null;
            //vertexBuffer = null;
            fileIn.Close();
            m.Close();
            //GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);
            //handle = VBOBuffers[0];
        }


        public VBO(byte[] byteArray)
        {
            ArrayToVBO(byteArray, byteArray.Length);
        }

        private void ArrayToVBO(Array source, int size)
        {
            
            float[] floatArray = new float[size / 4];
            
            System.Buffer.BlockCopy(source, 0, floatArray, 0, (int)size);
            //temp Cut vertex 
            int cutSize = 0;
            for (int i=0; i < size / 4; i+=3)
            {
                if (floatArray[i + 1] < 20.0f) cutSize++;
            }
            float[] cutArray = new float[cutSize*3];

            int cutArrayIndex = 0;
            for (int i = 0; i < size / 4; i += 3)
            {
                if (floatArray[i + 1] < 20.0f)
                {
                    cutArray[cutArrayIndex] = floatArray[i];
                    cutArray[cutArrayIndex+1] = floatArray[i + 1];
                    cutArray[cutArrayIndex + 2] = floatArray[i + 2];
                    cutArrayIndex+=3;
                }
            }
            size = cutSize * 3 * 4;
            objectSize = (int)(size / 4 / 3);

            FloatBuffer vertexBuffer;
            vertexBuffer = FloatBuffer.Allocate((int)size / 4); // float array to
            vertexBuffer.Put(cutArray, 0, (int)size / 4);
            vertexBuffer.Flip();

            GLES20.GlGenBuffers(1, VBOBuffers, 0);
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, VBOBuffers[0]);

            //VBOManager.setSize(fileName, vertexBuffer.Capacity() / 4); //is size of vertex count = 1 vertex 4 float x,y,z, 1                                
            GLES20.GlBufferData(GLES20.GlArrayBuffer, vertexBuffer.Capacity() * mBytesPerFloat, vertexBuffer, GLES20.GlStaticDraw);
            GLES20.GlBindBuffer(GLES20.GlArrayBuffer, 0);
            handle = VBOBuffers[0];

            floatArray = null;
            vertexBuffer = null;
        }

    }
}