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
    public struct Grid1D
    {
        public float Step { get; set; }
        public int Node { get; set; }

        public Grid1D(float a, int b)
        {
            Step = a;
            Node = b;
        }

        public float GetOXCoord(int ox_coord)
        {

            return ox_coord * Step;
        }

        public float GetOYCoord(int oy_coord)
        {

            return oy_coord * Step;
        }

        public override string ToString()
        { return "Step = " + Step + ", " + "Node = " + Node; }

        public string ToString(string format)
        {
            string res;
            res = "Step = " + Step.ToString(format) + " ,Node = " + Node.ToString(format);
            return res;
        }

    }
}
