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
using System.Collections.ObjectModel;
using ClassLibrary;

namespace Lab_U1
{
    public partial class MainWindow : Window
    {
        public ViewData view { get; set; }
        public VMGrid curr_grid { get; set; } = new(5, (float)3.5, (float)10.5, VMF.vmdLn);
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            view = new();
        }

        private void command_new(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DisplaySaveFileMessage())
                {
                    view.benchmark.coll1.Clear();
                    view.benchmark.coll2.Clear();
                    view.CheckChanged = false;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void command_open(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DisplaySaveFileMessage())
                {
                    Microsoft.Win32.OpenFileDialog dlgo = new Microsoft.Win32.OpenFileDialog
                    {
                        FileName = "",
                        DefaultExt = ".txt",
                        Filter = "Text documents (.txt)|*.txt"
                    };

                    bool? result;

                    result = dlgo.ShowDialog();


                    if (result == true)
                    {
                        string filename = dlgo.FileName;
                        bool errors = view.Load(filename);
                        view.CheckChanged = false;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Сохраняем в файл
        private void command_save(object sender, RoutedEventArgs e)
        {
            try
            {
                // Настриваем сохранение
                Microsoft.Win32.SaveFileDialog dlgs = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Document",
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt"
                };

                bool? result;

                result = dlgs.ShowDialog();


                if (result == true)
                {
                    // Сохраняем документ
                    string filename = dlgs.FileName;
                    bool errors = view.Save(filename);
                    view.CheckChanged = false;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_VMTime_Com(object sender, RoutedEventArgs e)
        {
            try
            {
                view.benchmark.AddVMTime(curr_grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Add_VMAccuracy_Com(object sender, RoutedEventArgs e)
        {
            try
            {
                view.benchmark.AddVMAccuracy(curr_grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.Source;
            try
            {
                // Какой элемент выбран
                switch (menuItem.Header.ToString())
                {
                    case "Add VMTime":
                        view.benchmark.AddVMTime(curr_grid);
                        break;
                    case "Add VMAccuracy":
                        view.benchmark.AddVMAccuracy(curr_grid);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error123: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool DisplaySaveFileMessage()
        {
            try
            {
                if (view.CheckChanged)
                {
                    MessageBoxResult UserChoice = MessageBox.Show($"Save changes to file?", "Lab 4", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    switch (UserChoice)
                    {
                        case MessageBoxResult.Cancel:
                            return false;
                        // Сохранить файл
                        case MessageBoxResult.Yes:
                            Microsoft.Win32.SaveFileDialog dlgs = new Microsoft.Win32.SaveFileDialog();
                            dlgs.FileName = "Document";
                            dlgs.DefaultExt = ".txt";
                            dlgs.Filter = "Text documents (.txt)|*.txt";
                            bool? result = dlgs.ShowDialog();


                            if (result == true)
                            {
                                string filename = dlgs.FileName;
                                bool errors = view.Save(filename);
                                return errors;
                            }
                            else
                            {

                                return false;
                            }
                        case MessageBoxResult.No:
                            return true;
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DisplaySaveFileMessage())
            {
                base.OnClosing(e);
            }
        }
    }
}
