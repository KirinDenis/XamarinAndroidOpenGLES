using Android.App;
using Android.Widget;
using Android.OS;

using Android.Opengl;
using Android.Views;
using Android.Content;

/// <summary>
/// Android Xamarin OpenGL ES lessons:
/// https://github.com/KirinDenis/XamarinAndroidOpenGLES
/// </summary>
namespace SGWW
{
    [Activity(Label = "@string/app_name",  MainLauncher = true)]
    public class SelectActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_select);

            Button buttonPublic = (Button)this.FindViewById(Resource.Id.buttonPublic);
            buttonPublic.Click += ButtonPublic_Click;

            Button buttonVitaly = (Button)this.FindViewById(Resource.Id.buttonVitaly);
            buttonVitaly.Click += ButtonVitaly_Click;

            Button buttonRuslan = (Button)this.FindViewById(Resource.Id.buttonRuslan);
            buttonRuslan.Click += ButtonRuslan_Click;

            Button buttonVladimir = (Button)this.FindViewById(Resource.Id.buttonVladimir);
            buttonVladimir.Click += ButtonVladimir_Click;

            Button buttonKonstantin = (Button)this.FindViewById(Resource.Id.buttonKonstantin);
            buttonKonstantin.Click += ButtonKonstantin_Click;

            Button buttonStas = (Button)this.FindViewById(Resource.Id.buttonStas);
            buttonStas.Click += ButtonStas_Click;

            Button buttonSergek = (Button)this.FindViewById(Resource.Id.buttonSergek);
            buttonSergek.Click += ButtonSergek_Click;

            Button buttonDen = (Button)this.FindViewById(Resource.Id.buttonDen);
            buttonDen.Click += ButtonDen_Click;

        }

        private void ButtonPublic_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(PublicActivity));            
            this.StartActivity(intent);
        }

        private void ButtonVitaly_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(VitalyActivity));
            this.StartActivity(intent);
        }

        private void ButtonRuslan_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(RuslanActivity));
            this.StartActivity(intent);
        }

        private void ButtonVladimir_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(VladimirActivity));
            this.StartActivity(intent);
        }

        private void ButtonKonstantin_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(KonstantinActivity));
            this.StartActivity(intent);
        }

        private void ButtonStas_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(StasActivity));
            this.StartActivity(intent);
        }

        private void ButtonSergek_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(SergekActivity));
            this.StartActivity(intent);
        }

        private void ButtonDen_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(DenActivity));
            this.StartActivity(intent);
        }
    }
}

