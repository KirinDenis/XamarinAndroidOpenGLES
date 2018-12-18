using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Graphics;

namespace App4
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
        private DrawCanvas canvas;


        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);


            canvas = new DrawCanvas(this);
         

            CoordinatorLayout layout = (CoordinatorLayout)FindViewById(Resource.Id.mainlayout);
            layout.AddView(canvas);


            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10;
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            canvas.cx = (int)e.RawX;
            canvas.cy = (int)e.RawY;
            return true;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            canvas.Invalidate();
        }

    }
}

