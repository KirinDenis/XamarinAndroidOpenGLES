using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Opengl;
using Android.Views;

/// <summary>
/// Android Xamarin OpenGL ES lessons:
/// https://github.com/KirinDenis/XamarinAndroidOpenGLES
/// </summary>
namespace SGWW
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class PublicActivity : AppCompatActivity, View.IOnTouchListener
    {
        private GLSurfaceView _GLSurfaceView;
        private PublicView publicView;
        private PublicRenderer renderer;

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
            SetContentView(Resource.Layout.activity_public);

            _GLSurfaceView = new GLSurfaceView(this);
            _GLSurfaceView.SetEGLContextClientVersion(2);
            //_GLSurfaceView.SetEGLConfigChooser(true);


            renderer = new PublicRenderer(this);
            _GLSurfaceView.SetRenderer(renderer);

            RelativeLayout sceneHolder = (RelativeLayout)this.FindViewById(Resource.Id.sceneHolder);
            sceneHolder.AddView(_GLSurfaceView);

            publicView = new PublicView(this);
            publicView.render = renderer;
            publicView.SetOnTouchListener(this);

            sceneHolder.AddView(publicView);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10;
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    x = -1;
                    y = -1;
                    break;
                case MotionEventActions.Move:
                    if (x != -1)
                    {
                        renderer.camera.eyeX -= (x-e.RawX) / 50;
                        renderer.camera.eyeY -= (y-e.RawY) / 50;
                    }

                    x = e.RawX;
                    y = e.RawY;

                    break;
            }
            return true;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            publicView.Invalidate();
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

