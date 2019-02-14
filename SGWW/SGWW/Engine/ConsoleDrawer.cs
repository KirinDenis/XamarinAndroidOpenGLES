using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SGWW
{
    /// <summary>
    /// Console item to draw in canvas
    /// </summary>
    class ConsoleItem
    {
        public string text;
        public DateTime time;
        public int status;
    }

    class ConsoleDrawer
    {
        /// <summary>
        /// app console 
        /// </summary>
        private List<ConsoleItem> console = new List<ConsoleItem>();
        private DateTime lastTime;
        private int refreshPeriod = 500;
        private int linesCount = 20;
        private int consoleX = 50;
        private int consoleY = 50;

        private int n = 0;
        private int lastCount = 0;

        public void OnDraw(Canvas canvas)
        {
            try
            {
                TimeSpan span = DateTime.Now - lastTime;
                if (span.TotalMilliseconds > refreshPeriod)
                {

                    if (console.Count > linesCount)
                    {
                        console.RemoveRange(0, console.Count - linesCount);
                    }
                    lastTime = DateTime.Now;

                }


                if (span.TotalMilliseconds > 300)
                {
                    if (lastCount != console.Count)
                        n++;
                }
                lastCount = console.Count;



                Paint paint = new Paint();
                int startColor = 0;
                paint.TextSize = 20;
                int top = consoleY - n;

                foreach (ConsoleItem consoleItem in console)
                {
                    string text = consoleItem.time.ToString() + " " + consoleItem.text;
                    Rect bounds = new Rect();
                    paint.GetTextBounds(text, 0, text.Length, bounds);


                    if (n > bounds.Height() + 4) n = 0;

                    switch (consoleItem.status)
                    {
                        case 3: paint.Color = Color.Argb(0xF0, startColor, 0x00, 0x00); break;
                        case 2: paint.Color = Color.Argb(0xF0, 0x00, startColor, 0x00); break;
                        case 1: paint.Color = Color.Argb(0xF0, 0x00, 0x00, startColor); break;
                        default: paint.Color = Color.Argb(0xF0, 0x00, 0x00, 0x00); break;
                    }

                    canvas.DrawText(text, consoleX, top, paint);

                    top += bounds.Height() + 4;
                    startColor += 0xFF / console.Count;
                }
            }
            catch{ }


        }

        public void Add(string text, int status)
        {
            ConsoleItem consoleItem = new ConsoleItem();
            consoleItem.text = text;
            consoleItem.time = DateTime.Now;
            consoleItem.status = status;
            console.Add(consoleItem);
        }
    }
}