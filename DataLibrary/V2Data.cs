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


namespace DataLibrary
{
    [Serializable]
    abstract public class V2Data : IEnumerable<DataItem>
    {
        public string Indef;
        public double Freq;
        public V2Data(string a, double b)
        {
            Indef = a;
            Freq = b;
        }

        public abstract Complex[] NearAverage(float eps);

        public abstract string ToLongString();

        public override string ToString()
        { return "V2Data: " + Indef + "," + Freq; }

        public abstract string ToLongString(string format);

        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void P_C(object source, string propertyName)
        {
            PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(propertyName));
        }

        public string Ind
        {
            get => Indef;
            set
            {
                Indef = value;
                P_C(this, "Fr");
            }
        }

        public double Fr
        {
            get => Freq;
            set
            {
                Freq = value;
                P_C(this, "Fr");
            }
        }

    }
}
