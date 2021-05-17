using System;
using System.Data;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DataLibrary
{
    class Res
    {
        public static void Coll_Test(object data, DataChangedEventArgs args)
        {
            Console.WriteLine(args.ToString());
        }

        static void Main()
        {
            try
            {
                //file data      
                V2DataCollection Lab_2_Data_Coll = new V2DataCollection("data_1.txt");
                Console.WriteLine(Lab_2_Data_Coll.ToLongString());

                //Linq testing
                V2MainCollection Lab_2_Main_Coll = new V2MainCollection();
                Lab_2_Main_Coll.AddDefaults();
                Lab_2_Main_Coll.ToLongString();

                Console.WriteLine("\nMiddle module value:");
                Console.WriteLine(Lab_2_Main_Coll.Mid_Value);
                Console.WriteLine("\nMaxFarAway values:");

                foreach (DataItem item in Lab_2_Main_Coll.Max_Far_Away)
                {
                    Console.WriteLine(item.ToString());
                }

                /*Console.WriteLine("\nMoreThenOne values:");
                foreach (Vector2 item in Lab_2_Main_Coll.More_then_one)
                {
                    Console.WriteLine(item.ToString("f5"));
                }
                */

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            V2MainCollection Test_3 = new V2MainCollection();
            Test_3.DataChanged += Coll_Test;

        }






    }

}
