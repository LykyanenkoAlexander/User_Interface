using System;
using System.Numerics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using DataLibrary;

namespace WpfApp1
{
    class DataItemElements : IDataErrorInfo, INotifyPropertyChanged
    {
        public V2DataCollection elements;
       
        float xCoord, yCoord, re_value, im_value;

        
        public DataItemElements(V2DataCollection source)
        {
            elements = source;
        }

        
        public DataItemElements(object source)
        {
            elements = source as V2DataCollection;
        }
        

        //public DataItemElements()
        //{ }
       


        public float X
        {
            get => xCoord;
            set
            {
                this.xCoord = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
            }
        }

        public float Y
        {
            get => yCoord;
            set
            {
                this.yCoord = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
            }
        }

        public float Re_Value
        {
            get => re_value;
            set
            {
                this.re_value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Re_Value"));
            }
        }

        public float Im_Value
        {
            get => im_value;
            set
            {
                this.im_value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Im_Value"));
            }
        }

        public Vector2 Vec => new Vector2(xCoord, yCoord);
        public event PropertyChangedEventHandler PropertyChanged;

        public void AddDataItem()
        {
            
            
            elements.Add(new DataItem(X, Y, Re_Value, Im_Value));
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
        }

        //лекция 10 слайд 5
        public string Error { get { return "Invalid input data"; } }

        public string this[string property]
        {
            get
            {
                string msg = null;
                Vector2 test_Vec = new Vector2(X, Y);
                List<Vector2> back_Vect = new List<Vector2>();
                Vector2[] VectData;
                switch (property)
                {
                    case "X":
                    case "Y":
                        if (elements == null)
                        {
                            msg = "source == null (Data on grid is selected)";
                        }
                        
                        else
                        {
                            foreach(var item in elements)
                            {
                                if(item.Vect2 == test_Vec)
                                {
                                    msg = "Vect2 coord must not be repeated";
                                }
                            }
                        }
                        break;

                    case "Re_Value":
                    case "Im_Value":
                        if (Math.Sqrt(Re_Value * Re_Value + Im_Value * Im_Value) <= 0f)
                        { 
                            msg = "Value < 0"; 
                        }
                        break;

                    default:
                        break;
                }
                return msg;
            }
        }
    }
}

