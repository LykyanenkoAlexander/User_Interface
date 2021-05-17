using System;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace DataLibrary
{


    public delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);
    public enum ChangeInfo
    {
        ItemChanged, Add, Remove, Replace
    }
    public class DataChangedEventArgs
    {
        public ChangeInfo ch_inf { get; set; }
        public double data { get; set; }

        public DataChangedEventArgs(ChangeInfo ch_inf_c, double data_c) { ch_inf = ch_inf_c; data = data_c; }
        public override string ToString() => ch_inf.ToString() + "\n" + data;
    }


}