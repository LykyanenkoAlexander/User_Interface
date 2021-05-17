using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;


namespace DataLibrary
{

    //! [Serializable]
    public class V2MainCollection : IEnumerable<V2Data>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasUnsavedChanges { get; set; }


        //private List<V2Data> Main_Data { get; set; }
        List<V2Data> Main_Data; 


        public int number;
        public V2MainCollection()
        {
            Main_Data = new List<V2Data>();
            number = 0;
            CollectionChanged += OnChange;
            //не было того, что ниже
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasUnsavedChanges = false;
        }

        private void OnChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            HasUnsavedChanges = true;
          
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxDist"));

            MidVal_func = Mid_Value;
        }


        private double MidVal;
        public double MidVal_func
        {
            get
            {
                return MidVal;
            }
            set
            {
                MidVal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MidVal_func"));
            }
        }


        public void Save(string filename)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Open(filename, FileMode.OpenOrCreate);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(fileStream, Main_Data);
                OnCollectionChanged(serializer, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                HasUnsavedChanges = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Saving error" + ex.Message);

            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        public void Load(string filename)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Main_Data = binaryFormatter.Deserialize(fileStream) as List<V2Data>;
                CollectionChanged?.Invoke(binaryFormatter, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            }
            /*
            catch (Exception ex)
            {
                Console.WriteLine("Loading error" + ex.Message);

            }
            */
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
                CollectionChanged(sender, args);
        }

        public V2Data this[int index]
        {
            get
            {
                return Main_Data[index];
            }
            set
            {
                value.PropertyChanged += PC;
                Main_Data[index] = value;

                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace,
                              Main_Data[index].Freq));
            }
        }
        [field: NonSerialized]
        public event DataChangedEventHandler DataChanged;
        public void OnDataChanged(object source, DataChangedEventArgs args)
        {
            DataChanged?.Invoke(source, args);
        }

        public void PC(object sender, PropertyChangedEventArgs args)
        {
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged,
                          Main_Data[number - 1].Freq));
        }



        public int Count
        {
            set { }

            get
            {
                return (Main_Data.Count);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return Main_Data.GetEnumerator();
        }

        IEnumerator<V2Data> IEnumerable<V2Data>.GetEnumerator()
        {
            return Main_Data.GetEnumerator();
        }


        public double Mid_Value
        {
            get
            {
                var t = from data in Main_Data select data;

                if (t.Count() == 0)
                {
                    return 0;
                }
                else
                {
                    var val = from data in Main_Data where data.Count() > 0 from i in data select i.Compl;
                    var res = (from x in val select x.Magnitude).Average();

                    return res;

                }
                
            }
        }

        public IEnumerable<DataItem> Max_Far_Away
        {
            get
            {
                var united = Main_Data.SelectMany(x => x);
                double m_v = Mid_Value;

                double max = united.Max(v => v.Compl.Magnitude);
                double min = united.Min(v => v.Compl.Magnitude);

                var total = (Math.Abs(m_v - max) > Math.Abs(m_v - min)) || ((Math.Abs(m_v - max) < Math.Abs(m_v - min))) ?
                            ((Math.Abs(m_v - max) > Math.Abs(m_v - min)) ? max : min) : min;

                IEnumerable<V2DataOnGrid> V2 = from V2Data x in Main_Data where x.Indef == "A" select (V2DataOnGrid)x;
                //var V22 = from DataItem x in Main_Data where x.Compl.Magnitude == total select x;
                IEnumerable<V2DataCollection> C2 = from V2Data x in Main_Data where x.Indef == "B" select (V2DataCollection)x;

                var V2_res = V2.SelectMany(x => x).Where(y => y.Compl.Magnitude == total);
                var C2_res = C2.SelectMany(x => x).Where(y => y.Compl.Magnitude == total);

                IEnumerable<DataItem> Res = V2_res.Concat(C2_res);

                return Res;
            }
        }

        public IEnumerable<Vector2> More_then_one
        {
            get
            {
                IEnumerable<Vector2> vec_set = from x in Main_Data from item in x select item.Vect2;
                var result = (from x in vec_set where (vec_set.Count() > 1) select x);

                var res = from x in Main_Data
                          from i in x
                          group i by i.Vect2 into h
                          where h.Count() > 1
                          select h.Key;

                return res;
            }
        }

        public void Add(V2Data item)
        {
            item.PropertyChanged += PC;
            Main_Data.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Add,
                          Main_Data[number].Freq));
            number++;

        }


        public void RemoveInterface(int index)
        {
            if (index >= 0 && index < Main_Data.Count())
            {
                Main_Data.RemoveAt(index);
                //того, что ниже - не было
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Remove(string id, double w)
        {
            Console.WriteLine('\n' + "Before Removing:" + '\n');
            foreach (var item in Main_Data.ToArray())
            {
                Console.WriteLine(item);
            }

            foreach (var item in Main_Data.ToArray())
            {
                if ((item.Indef == id) && (item.Freq == w))
                {
                    Main_Data.Remove(item);
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }

            Console.WriteLine('\n' + "After Removing:" + '\n');
            foreach (var item in Main_Data.ToArray())
            {
                Console.WriteLine(item);
            }

            if (Main_Data.Count != 0)
            {
                Console.WriteLine("Main_Data.Length = " + Main_Data.Count);

                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove,
                              Main_Data[number - Main_Data.Count - 1].Freq));

            }
            number--;


            return Main_Data.Count > 0;

        }




        public void AddDefaults()
        {
            Random rnd = new Random();

            Grid1D d1 = new Grid1D(1, 2);
            Grid1D d2 = new Grid1D(2, 2);
            V2DataOnGrid New_Grid_1 = new V2DataOnGrid("A_1", 3, d1, d1);
            V2DataOnGrid New_Grid_2 = new V2DataOnGrid("A_2", 3, d2, d2);
            New_Grid_1.InitRandom(3, 7);
            New_Grid_2.InitRandom(4, 8);

            V2DataCollection New_Coll = new V2DataCollection("B_1", 5);
            New_Coll.InitRandom(4, 4, 6, 4, 6);

            Add(New_Grid_1);
            Add(New_Grid_2);
            Add(New_Coll);

           // Main_Data.Add(New_Grid_1);
           // Main_Data.Add(New_Grid_2);
            
            //Main_Data.Add(New_Coll);

        }

        public override string ToString()
        {
            foreach (var item in Main_Data)
            {
                Console.WriteLine(item.ToString());
            }
            return null;
        }

        public string ToLongString()
        {
            foreach (var item in Main_Data)
            {
                Console.WriteLine(item.ToLongString());
            }
            return null;
        }

        public string ToLongString(string format)
        {
            foreach (var item in Main_Data)
            {
                Console.WriteLine(item.ToLongString(format));
            }
            return null;
        }

    }


}