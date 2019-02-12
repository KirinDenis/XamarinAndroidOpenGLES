using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Opengl;

namespace Clock
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private DrawView canvas;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            var rectView = new DrawView(this);
            SetContentView(Resource.Layout.activity_main);
            canvas = new DrawView(this);


            CoordinatorLayout layout = (CoordinatorLayout)FindViewById(Resource.Id.mainlayout);
            layout.AddView(canvas);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10;
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;

        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            canvas.Invalidate();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {

        }
    }
    public class DrawView : View
    {
        Paint paintCommon;
        Paint paintDigit;
        Paint paintPointCenter;
        Paint paintHour;
        Paint paintMinutes;
        Paint paintSeconds;

        RectF rectf1;
        RectF rectf2;
        Point point;

        double currentXHour = 0;
        double currentYHour = 0;
        double currentXHourNeg = 0;
        double currentYHourNeg = 0;

        int radiusHour = 85;
        int radiusHourNeg = 25;

        double currentXMinutes = 0;
        double currentYMinutes = 0;
        double currentXMinutesNeg = 0;
        double currentYMinutesNeg = 0;

        int radiusMinutes = 110;
        int radiusMinutesNeg = 35;

        double zeroXSeconds = -1;
        double zeroYSeconds = -1;

        double zeroXSecondsNeg = -1;
        double zeroYSecondsNeg = -1;

        double currentXSeconds = -1;
        double currentYSeconds = -1;

        double nextXSeconds = -1;
        double nextYSeconds = -1;

        double currentXSecondsNeg = 0;
        double currentYSecondsNeg = 0;

        int radiusSeconds = 130;
        int radiusSecondsNeg = 45;

        int radiusCircle = 150;

        float widthCenter = 0;
        float heightCenter = 0;


        int xZeroSeconds = -1;
        int yZeroSeconds = -1;

        double oneSecondRadian;


        double radianForCurrentSeconds;

        float[] objectDraw = new float[16];
        float[] resultVec = new float[4];
        float[] IhsMat = new float[16];
        float[] rhsVec = new float[4];

        int a = 0;


        public DrawView(Context context)
            : base(context)
        {

            paintCommon = new Paint();
            paintCommon.StrokeWidth = 3;
            paintCommon.SetStyle(Paint.Style.Stroke);

            paintDigit = new Paint();
            paintDigit.StrokeWidth = 1;
            paintDigit.SetStyle(Paint.Style.Stroke);

            paintHour = new Paint();
            paintHour.StrokeWidth = 10;
            paintHour.SetStyle(Paint.Style.Stroke);

            paintMinutes = new Paint();
            paintMinutes.StrokeWidth = 8;
            paintMinutes.SetStyle(Paint.Style.Stroke);

            paintSeconds = new Paint();
            paintSeconds.StrokeWidth = 5;
            paintSeconds.SetStyle(Paint.Style.Stroke);

            paintPointCenter = new Paint();
            paintPointCenter.StrokeWidth = 3;
            paintPointCenter.SetStyle(Paint.Style.Fill);

            rectf1 = new RectF(50, 50, 100, 100);
            rectf2 = new RectF(50, 150, 100, 200);
            point = new Point();

        }

        protected override void OnDraw(Canvas canvas)
        {


            widthCenter = canvas.Width / 2;
            heightCenter = canvas.Height / 3;

            //canvas.DrawCircle(widthCenter, heightCenter, radiusCircle, paintCommon);
            canvas.DrawCircle(widthCenter, heightCenter, 12, paintPointCenter);

            TimeSpan time = DateTime.Now.TimeOfDay;


            drawHour(time.Hours, canvas);
            drawMinutes(time.Minutes, canvas);
            drawSeconds(time.Seconds, canvas);

            drawDigit(canvas);

            a++;

        }

        private void drawDigit(Canvas canvas)
        {
            for (int i = 0; i < 12; i++)
            {

                int hourInDegris = i * 360 / 12;
                double degrisForCurrentHour = hourInDegris - 90;
                double radianForCurrentHour = convertDegToRad(degrisForCurrentHour);


                currentXHour = radiusCircle * Math.Cos(radianForCurrentHour);
                currentYHour = radiusCircle * Math.Sin(radianForCurrentHour);

                matrixRotation(ref currentXHour, ref currentYHour);

                canvas.DrawCircle(Convert.ToSingle(currentXHour) + widthCenter, Convert.ToSingle(currentYHour) + heightCenter, 5, paintCommon);

            }
        }

        private void drawHour(int hour, Canvas canvas)
        {

            int hourInDegris = hour * 360 / 12;
            double degrisForCurrentHour = hourInDegris - 90;
            double radianForCurrentHour = convertDegToRad(degrisForCurrentHour);


            currentXHour = radiusHour * Math.Cos(radianForCurrentHour);
            currentYHour = radiusHour * Math.Sin(radianForCurrentHour);

            currentXHourNeg = -(radiusHourNeg * Math.Cos(radianForCurrentHour));
            currentYHourNeg = -(radiusHourNeg * Math.Sin(radianForCurrentHour));


            matrixRotation(ref currentXHour, ref currentYHour);
            matrixRotation(ref currentXHourNeg, ref currentYHourNeg);

            canvas.DrawLine(widthCenter, heightCenter, Convert.ToSingle(currentXHour) + widthCenter, Convert.ToSingle(currentYHour) + heightCenter, paintHour);

            canvas.DrawLine(widthCenter, heightCenter, Convert.ToSingle(currentXHourNeg) + widthCenter, Convert.ToSingle(currentYHourNeg) + heightCenter, paintHour);

        }

        private void drawMinutes(int minutes, Canvas canvas)
        {
            int minutesInDegris = minutes * 360 / 60;
            double degrisForCurrentMinutes = minutesInDegris - 90;
            double radianForCurrentMinutes = convertDegToRad(degrisForCurrentMinutes);


            currentXMinutes = radiusMinutes * Math.Cos(radianForCurrentMinutes);
            currentYMinutes = radiusMinutes * Math.Sin(radianForCurrentMinutes);

            currentXMinutesNeg = -(radiusMinutesNeg * Math.Cos(radianForCurrentMinutes));
            currentYMinutesNeg = -(radiusMinutesNeg * Math.Sin(radianForCurrentMinutes));


            matrixRotation(ref currentXMinutes, ref currentYMinutes);
            matrixRotation(ref currentXMinutesNeg, ref currentYMinutesNeg);

            canvas.DrawLine(widthCenter, heightCenter, Convert.ToSingle(currentXMinutes) + widthCenter, Convert.ToSingle(currentYMinutes) + heightCenter, paintMinutes);

            canvas.DrawLine(widthCenter, heightCenter, Convert.ToSingle(currentXMinutesNeg) + widthCenter, Convert.ToSingle(currentYMinutesNeg) + heightCenter, paintMinutes);
        }

        private void drawSeconds(int seconds, Canvas canvas)
        {
            oneSecondRadian = convertDegToRad(360 / 60);

            radianForCurrentSeconds = oneSecondRadian * seconds;

            zeroXSeconds = 0;
            zeroYSeconds = 0 - radiusCircle + 30;

            nextXSeconds = zeroXSeconds * Math.Cos(radianForCurrentSeconds) - zeroYSeconds * Math.Sin(radianForCurrentSeconds);
            nextYSeconds = zeroXSeconds * Math.Sin(radianForCurrentSeconds) + zeroYSeconds * Math.Cos(radianForCurrentSeconds);


            matrixRotation(ref nextXSeconds, ref nextYSeconds);
            //matrixRotation(ref widthCenter, ref heightCenter);

            canvas.DrawLine(widthCenter, heightCenter, Convert.ToSingle(nextXSeconds + widthCenter), Convert.ToSingle(nextYSeconds + heightCenter), paintSeconds);


            zeroXSecondsNeg = 0;
            zeroYSecondsNeg = 0 - radiusCircle + 100;

            nextXSeconds = -(zeroXSecondsNeg * Math.Cos(radianForCurrentSeconds) - zeroYSecondsNeg * Math.Sin(radianForCurrentSeconds));
            nextYSeconds = -(zeroXSecondsNeg * Math.Sin(radianForCurrentSeconds) + zeroYSecondsNeg * Math.Cos(radianForCurrentSeconds));

            matrixRotation(ref nextXSeconds, ref nextYSeconds);

            canvas.DrawLine(widthCenter, heightCenter, Convert.ToSingle(nextXSeconds + widthCenter), Convert.ToSingle(nextYSeconds + heightCenter), paintSeconds);

        }

        private double convertDegToRad(double deg)
        {
            double rad = deg * Math.PI / 180;
            return rad;
        }

        private void matrixRotation(ref double x, ref double y)
        {


            float xFloat = (float)x;
            float yFloat = (float)y;

            rhsVec[0] = xFloat;
            rhsVec[1] = yFloat;
            rhsVec[2] = 0;
            rhsVec[3] = 1;



            Android.Opengl.Matrix.SetIdentityM(objectDraw, 0);
            Android.Opengl.Matrix.RotateM(objectDraw, 0, a, 1, 1, 1);
            //Android.Opengl.Matrix.PerspectiveM(objectDraw, 0, 1, 1, 1, 1);
            Android.Opengl.Matrix.ScaleM(objectDraw, 0, 2, 2, 2);

            Android.Opengl.Matrix.MultiplyMV(resultVec, 0, objectDraw, 0, rhsVec, 0);


            x = (double)resultVec[0];
            y = (double)resultVec[1];

        }


        //private void matrixRotation(ref float x, ref float y)
        //{


        //    float xFloat = x;
        //    float yFloat = y;

        //    rhsVec[0] = xFloat;
        //    rhsVec[1] = yFloat;
        //    rhsVec[2] = 0;
        //    rhsVec[3] = 1;


        //    Android.Opengl.Matrix.SetIdentityM(objectDraw, 0);

        //    Android.Opengl.Matrix.PerspectiveM(objectDraw, 0, 300, 300, 300, 300);

        //    Android.Opengl.Matrix.ScaleM(objectDraw, 0, a, a, a);

        //    Android.Opengl.Matrix.RotateM(objectDraw, 0, a, 0, 1, 0);

        //    Android.Opengl.Matrix.MultiplyMV(resultVec, 0, objectDraw, 0, rhsVec, 0);





        //    x = resultVec[0];
        //    y = resultVec[1];

        //}

    }
}

