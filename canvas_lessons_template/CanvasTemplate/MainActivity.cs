using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

/// <summary>
/// Android Xamarin OpenGL ES lessons:
/// https://github.com/KirinDenis/XamarinAndroidOpenGLES
/// https://www.youtube.com/watch?v=j4JHrRV3r9g&list=PLIAm4wLW4KD9DEvxQglY2JMr9pt3H-wC9
/// </summary>
namespace CanvasTemplate
{    
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private DrawCanvas canvas;

        /// <summary>
        /// See Android App lifecycle diagram to know when Android call this event hadler 
        /// https://developer.android.com/guide/components/activities/activity-lifecycle
        /// </summary>
        /// <param name="savedInstanceState">use this to store global app variables and objects when ardroid reLaunch application</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set Resources/layout/activity_main.axml as this activity content view (see android:id="@+id/mainlayout")
            SetContentView(Resource.Layout.activity_main);

            //After content view created, main layout view (component) is Created, get pointer at
            CoordinatorLayout layout = (CoordinatorLayout)FindViewById(Resource.Id.mainlayout);
            //Create our owned View object to access Canvas class and OnDraw method from here (see DrawCanvas.cs)
            canvas = new DrawCanvas(this);
            //Set our View object as main layout child 
            layout.AddView(canvas);

            //Prepare and customise timer to put android to call OnDraw method for our View OnDraw event hadler calls
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 100; //draw each 0.1 secons
            timer.Elapsed += OnTimedEvent; //see OnTimedEvent method 
            timer.Enabled = true;
        }

        /// <summary>
        /// This activity global on screen toch event with touch parameters
        /// </summary>
        /// <param name="e">The current user mutlitouch parameters, see this object properties at runtime for more information about
        /// what Android OS put to App when user touch screen
        /// </param>
        /// <returns></returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            //return true if we handle the event and no one can work with this event at OnTochEvent call stack 
            //also - we use this to send some toch data to our View object (if needed)
            return true;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            canvas.Invalidate();
        }

    }
}

