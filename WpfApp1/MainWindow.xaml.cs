using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataLibrary;
using System.Globalization;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Threading;
using System.IO;



namespace WpfApp1
{
    
    public partial class MainWindow : Window
    {
        V2MainCollection Lab_Main_Coll = new V2MainCollection();
        DataItemElements dataItemElements;
        public static RoutedCommand AddCustomCommand = new RoutedCommand("AddCustom", typeof(WpfApp1.MainWindow));

        public MainWindow()
        {
            InitializeComponent();  
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox_Main.SelectedItem != null) 
            {
                dataItemElements = new DataItemElements(listBox_Main.SelectedItem as V2DataCollection);
            }
            else if(listBox_DataCollection.SelectedItem != null)
            {
                dataItemElements = new DataItemElements(listBox_DataCollection.SelectedItem);
            }
            
            grid_DataItemCreator.DataContext = dataItemElements;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // MessageBox.Show("Closed");
            IF_Saved();
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void menuItem_AddDefaults(object sender, RoutedEventArgs e)
        {
            Lab_Main_Coll.AddDefaults();
           
        }

        private void menuItem_AddDefaults_V2DataCollection(object sender, RoutedEventArgs e)
        {
            V2DataCollection V2_Coll = new V2DataCollection("def_1_Collection", 1);
            V2_Coll.InitRandom(10, 10, 10, -20, 30);
            Lab_Main_Coll.Add(V2_Coll);
        }

        private void menuItem_AddDefaults_V2DataOnGrid(object sender, RoutedEventArgs e)
        {
            Grid1D G_1 = new Grid1D(3, 3);
            Grid1D G_2 = new Grid1D(5, 3);

            V2DataOnGrid V2_Grid = new V2DataOnGrid("def_1_Grid", 5, G_1, G_2);
            V2_Grid.InitRandom(5, 10);
            Lab_Main_Coll.Add(V2_Grid);
            
        }

        private void menuItem_Add_Element_from_File(object sender, RoutedEventArgs e)
        {
           
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    Lab_Main_Coll.Add(new V2DataCollection(dlg.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItem_Remove(object sender, RoutedEventArgs e)
        {
            Lab_Main_Coll.RemoveInterface(listBox_Main.SelectedIndex);
        }

        private void Save()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                Lab_Main_Coll.Save(dlg.FileName);
            }
        }

        public void IF_Saved()
        {
            if (!Lab_Main_Coll.HasUnsavedChanges)
            {
                return;
            }
            else
            {


                MessageBoxResult result = MessageBox.Show(
                    "Сохранить данные?\n", "", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    Save();
                }
            }
        }


        private void menuItem_New(object sender, RoutedEventArgs e)
        {
            IF_Saved();
            Lab_Main_Coll = new V2MainCollection();
            DataContext = Lab_Main_Coll;
        }

        private void menuItem_Open(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                V2MainCollection coll = new V2MainCollection();
                coll.Load(dialog.FileName);
                Lab_Main_Coll = coll;

            }
           
        }

        private void menuItem_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Lab_Main_Coll.Mid_Value.ToString());
        }


        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            IF_Saved();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {

                Lab_Main_Coll.Load(dlg.FileName);

            }
            DataContext = Lab_Main_Coll;
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Lab_Main_Coll.HasUnsavedChanges;
        }

        private void RemoveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Lab_Main_Coll.RemoveInterface(listBox_Main.SelectedIndex);
        }

        private void CanRemoveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            /*if(listBox_Main.Items.Contains(listBox_Main.SelectedItem))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }*/
        }

        private void CanAddCustomCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (grid_DataItemCreator!= null)
            {
                e.CanExecute = !(Validation.GetHasError(Xcoord) || Validation.GetHasError(Xcoord) ||
                                Validation.GetHasError(ReValue) || Validation.GetHasError(ImValue));
            }
            else
            { 
                e.CanExecute = false; 
            }
            

        }

        private void AddCustomCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                dataItemElements.AddDataItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox_details_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FilterDataOnGrid(object c, FilterEventArgs e) => e.Accepted = e.Item is V2DataOnGrid;
        private void FilterDataCollection(object c, FilterEventArgs e) => e.Accepted = e.Item is V2DataCollection;

    }





   



}
