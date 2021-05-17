using System;
using System.Data;
using System.Numerics;
using System.Collections.Generic;
//using Лаба;
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
    [Serializable]
    public struct DataItem : ISerializable
    {
        public Vector2 Vect2 { get; set; }
        public Complex Compl { get; set; }

        public DataItem(float x, float y)
        {
            Vect2 = new Vector2(x, y);
            Compl = new Complex(x, y);
        }

        //2
        public DataItem(float x, float y, float Re, float Im)
        {
            Vect2 = new Vector2(x, y);
            Compl = new Complex(Re, Im);
        }
        //

        public override string ToString()
        { return Vect2 + " , " + Compl; }

        public string ToString(string format)
        {
            string res = Vect2.ToString();
            res = res + " : " + Compl.ToString(format) + Math.Sqrt(Compl.Real * Compl.Real + Compl.Imaginary * Compl.Imaginary).ToString(format);
            return res;
        }

        public DataItem(SerializationInfo info, StreamingContext context)
        {
            float x = info.GetSingle("Value_X");
            float y = info.GetSingle("Value_Y");
            Vect2 = new Vector2(x, y);
            Compl = new Complex(x, y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Vect2", Vect2.X);
            info.AddValue("Vect2", Vect2.Y);
            info.AddValue("Compl", Vect2.X);
            info.AddValue("Compl", Vect2.Y);
        }


    }
}