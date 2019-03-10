using Android.App;
using Android.Widget;
using Android.OS;
using Android.Opengl;
using Android.Views;

/// <summary>
/// Android Xamarin OpenGL ES lessons:
/// https://github.com/KirinDenis/XamarinAndroidOpenGLES
/// </summary>
namespace SGWW
{
    [Activity(Label = "@string/app_name",  MainLauncher = false)]
    public class VladimirActivity :Activity, View.IOnTouchListener
    {
        private GLSurfaceView _GLSurfaceView;
        private VladimirView vladimirView;
        private VladimirRenderer renderer;
        private VladimirRender2 renderer2;
        private SGWW.VladimirRender3VBO renderer3;
        private Test tewst;

        private float x = -1;
        private float y = -1;

        /// <summary>
        /// See Android App lifecycle diagram to know when Android call this event hadler 
        /// https://developer.android.com/guide/components/activities/activity-lifecycle
        /// </summary>
        /// <param name="savedInstanceState">use this to store global app variables and objects when ardroid reLaunch application</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Set Resources/layout/activity_main.axml as this activity content view (see android:id="@+id/mainlayout")
            SetContentView(Resource.Layout.activity_vladimir);

            _GLSurfaceView = new GLSurfaceView(this);
            _GLSurfaceView.SetEGLContextClientVersion(2);

            renderer3 = new VladimirRender3VBO(this);
            _GLSurfaceView.SetRenderer(renderer3);

            RelativeLayout sceneHolder = (RelativeLayout)this.FindViewById(Resource.Id.sceneHolder);
            sceneHolder.AddView(_GLSurfaceView);
            sceneHolder.SetOnTouchListener(this);

            //vladimirView = new VladimirView(this);
           //vladimirView.render = renderer;
            //vladimirView.SetOnTouchListener(this);

            //sceneHolder.AddView(vladimirView);

            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 10;
            //timer.Elapsed += OnTimedEvent;
            //timer.Enabled = true;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            //switch (e.Action)
            //{
            //    case MotionEventActions.Down:
            //        x = -1;
            //        y = -1;
            //        break;
            //    case MotionEventActions.Move:
            //        if (x != -1)
            //        {
            //            renderer.angleX += (y - e.RawY) / 5; 
            //            renderer.angleY -= (x - e.RawX) / 5;
            //        }

            //        x = e.RawX;
            //        y = e.RawY;

            //        break;
            //}
            return true;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
         //   vladimirView.Invalidate();
        }

        protected override void OnResume()
        {         
            base.OnResume();
            _GLSurfaceView.OnResume();
        }

        protected override void OnPause()
        {
         
            base.OnPause();
            _GLSurfaceView.OnPause();
        }
    }
}

