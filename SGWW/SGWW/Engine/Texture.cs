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

namespace SGWW
{
    public class Texture
    {
        private int[] textureHandle = new int[1];
        public int handle = -1;

        public Texture(Context context, string fileName)
        {
            GLES20.GlGenTextures(1, textureHandle, 0); //init 1 texture storage handle 
            if (textureHandle[0] != 0)
            {
                //Android.Graphics cose class Matrix exists at both Android.Graphics and Android.OpenGL and this is only sample of using 
                Android.Graphics.BitmapFactory.Options options = new Android.Graphics.BitmapFactory.Options();
                options.InScaled = false; // No pre-scaling
                int id = context.Resources.GetIdentifier(fileName, "drawable", context.PackageName);
                Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeResource(context.Resources, id, options);
                GLES20.GlBindTexture(GLES20.GlTexture2d, textureHandle[0]);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter, GLES20.GlNearest);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter, GLES20.GlNearest);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS, GLES20.GlClampToEdge);
                GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT, GLES20.GlClampToEdge);
                GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bitmap, 0);
                bitmap.Recycle();

                handle = textureHandle[0];
            }
        }
    }
}