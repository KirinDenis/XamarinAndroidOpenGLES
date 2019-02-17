using Android.Opengl;
using Android.Content;
using Javax.Microedition.Khronos.Opengles;
using System;

namespace SGWW
{
    public class DenRenderer : Renderer
    {
        //For demo RUN only
        private DateTime lastTime;
        private Random r = new Random(0xFFFFF);
        private int animation = 0;

        public DenRenderer(Context context) : base(context)
        {
        }

        public override void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            base.OnSurfaceCreated(gl, config);

            //SETUP OpenGL ES             
            GLES20.GlClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GLES20.GlEnable(GLES20.GlDepthTest); //uncoment if needs enabled dpeth test
            //  GLES20.GlEnable(2884); // GlCullFace == 2884 see OpenGL documentation to this constant value  
            //  GLES20.GlCullFace(GLES20.GlFront);
            //ENDSETUP OpenGL ES             

            //Loading objects 
            glObjects.Add(new GLObject(this, "den_vertex_shader", "den_fragment_shader", "den_house1_objvertex", "den_house1_objnormal", "den_house1_objtexture", "den_housetextutre"));
            glObjects.Add(new GLObject(this, "den_vertex_shader", "den_fragment_shader", "den_house2_objvertex", "den_house2_objnormal", "den_house2_objtexture", "den_housetextutre2"));
            glObjects.Add(new GLObject(this, "den_vertex_shader", "den_fragment_shader", "den_house3_objvertex", "den_house3_objnormal", "den_house3_objtexture", "den_housetextutre"));
            glObjects.Add(new DenGlassObject(this, "den_glass"));


            //Ask android to run RAM garbage cleaner
            System.GC.Collect();
        }

        public override void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            base.OnSurfaceChanged(gl, width, height);
        }

        public override void OnDrawFrame(IGL10 gl)
        {
            GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit);

            //base.OnDrawFrame(gl);

            camera.OnDrawFrame();
            //TimeSpan span = DateTime.Now - lastTime;
            animation++;
            if (animation > 800) animation = 0;

            glObjects[0].DrawFrame();

            if (animation > 200)
            {
               glObjects[2].DrawFrame();
            }

            if (animation > 500)
            {
               glObjects[1].DrawFrame();
            }
           

            glObjects[3].DrawFrame();

            //Animation no way 
            /*
            TimeSpan span = DateTime.Now - lastTime;
            if (span.TotalMilliseconds > 500)
            {
                int rnd = r.Next(2000);

                if (rnd < 100)
                {
                    canvasView.ConsoleWrite("Room 1 temperature: 24C humidity 42%", 2);
                }
                else
                if (rnd < 200)
                {
                    canvasView.ConsoleWrite("Room 2 temperature: 27C humidity 35%", 2);
                }
                else
                if (rnd < 300)
                {
                    canvasView.ConsoleWrite("Garage light 100lm (dark)", 0);
                }
                else
                if (rnd < 400)
                {
                    canvasView.ConsoleWrite("Entry door 1 is oppen", 3);
                }
                else
                if (rnd < 500)
                {
                    canvasView.ConsoleWrite("Watering plants requires attention", 1);
                }
                else
                if (rnd < 600)
                {
                    canvasView.ConsoleWrite("Garage light On success", 2);
                }
                else
                if (rnd < 700)
                {
                    (glObjects[3] as DenGlassObject).brocken = true;
                }

                lastTime = DateTime.Now;
            }

            if (canvasView != null)
            {
                try
                {
                    canvasView.Invalidate();
                }
                catch { }
            }
            */
            /*
            camera.OnDrawFrame();

            glObjects[0].DrawFrame(camera);

            //Glass code with enable blend
            gl.GlEnable(GL10.GlBlend);
            gl.GlBlendFunc(GL10.GlSrcAlpha, GL10.GlOneMinusSrcAlpha);

            glObjects[1].lx = 0.9f;
            glObjects[1].ly = 0.0f;
            glObjects[1].lz = 0.0f;
            glObjects[1].lw += 0.01f;
            if (glObjects[1].lw > 0.5) glObjects[1].lw = 0;

            glObjects[1].DrawFrame(camera);

            gl.GlDisable(GL10.GlBlend);
            */
        }
    }
}