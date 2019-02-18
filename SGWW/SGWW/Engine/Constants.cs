using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SGWW
{
    public class Constants
    {
        public static float CosR(float a)
        {
            return (float)Math.Cos(Math.PI * a / 180.0);
        }

        public static float SinR(float a)
        {
            return (float)Math.Sin(Math.PI * a / 180.0);
        }

    }
}