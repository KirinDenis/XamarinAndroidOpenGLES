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

namespace SGWW.SectionVladimir
{
    public class Utilits
    {
        public static void comparing(List<Distanсe> listDistance)
        {
            //List<Distanсe> bufferDistance = new List<Distanсe>();

            Distanсe itemBufferDistance = new Distanсe();



            for (int i = 0; i < listDistance.Count; i++)
            {                

                for (int k = 0; k < listDistance.Count; k++)
                {

                    if (listDistance[i].distance < listDistance[k].distance)
                    {
                        itemBufferDistance = listDistance[k];

                        listDistance[k] = listDistance[i];

                        listDistance[i] = itemBufferDistance;

                    }

                  
                }
            }

        }


        public static void compare(List<Distanсe> listDistance)
        {



        }
    }
}