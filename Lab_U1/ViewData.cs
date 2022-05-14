using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.ComponentModel;
using ClassLibrary;


namespace Lab_U1
{
    public class ViewData : INotifyPropertyChanged
    {
        public VMBenchmark benchmark { get; set; }
        private bool _CheckChanged;
        // Properties
        public bool CheckChanged
        {
            get { return _CheckChanged; }
            set
            {
                if (value != _CheckChanged)
                {
                    _CheckChanged = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CheckChanged)));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public ViewData()
        {
            benchmark = new();
            CheckChanged = false;
            benchmark.coll1.CollectionChanged += Collection_CollectionChanged;
            benchmark.coll2.CollectionChanged += Collection_CollectionChanged;
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CheckChanged = true;
        }

        public void AddVMTime(VMGrid Grid)
        {
            try
            {
                benchmark.AddVMTime(Grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ошибка: {error.Message}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddVMAccuracy(VMGrid Grid)
        {
            try
            {
                benchmark.AddVMAccuracy(Grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ошибка: {error.Message}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool Save(string filename)
        {
            try
            {
                StreamWriter writer = new(filename, false);
                try
                {
                    writer.WriteLine(benchmark.coll1.Count());
                    foreach (VMTime item in benchmark.coll1)
                    {
                        writer.WriteLine(item.grid.len);
                        writer.WriteLine($"{item.grid.left:0.00000000}");
                        writer.WriteLine($"{item.grid.right:0.00000000}");
                        writer.WriteLine($"{item.grid.step:0.00000000}");
                        writer.WriteLine((int)item.grid.curr_function);
                        writer.WriteLine($"{item.time_vml_ha:0.00000000}");
                        writer.WriteLine($"{item.time_vml_ep:0.00000000}");
                        writer.WriteLine($"{item.coeff_ha_ep:0.00000000}");
                    }
                    writer.WriteLine(benchmark.coll2.Count());
                    foreach (VMAccuracy item in benchmark.coll2)
                    {
                        writer.WriteLine(item.grid.len);
                        writer.WriteLine($"{item.grid.left:0.00000000}");
                        writer.WriteLine($"{item.grid.right:0.00000000}");
                        writer.WriteLine($"{item.grid.step:0.00000000}");
                        writer.WriteLine((int)item.grid.curr_function);
                        writer.WriteLine($"{item.max_dif_abs:0.00000000}");
                        writer.WriteLine($"{item.max_dif_arg:0.00000000}");
                        writer.WriteLine($"{item.HA_value:0.00000000}");
                        writer.WriteLine($"{item.EP_value:0.00000000}");
                    }
                }
                catch (Exception e)
                {
                    benchmark.coll1.Clear();
                    benchmark.coll2.Clear();
                    MessageBox.Show($"Невозможно сохранить файл: {e.Message}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    writer.Close();
                    return false;
                }
                finally
                {
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                benchmark.coll1.Clear();
                benchmark.coll2.Clear();
                MessageBox.Show($"Невозможно сохранить файл: {e.Message}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool Load(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(filename);
                try
                {
                    benchmark.coll1.Clear();
                    benchmark.coll2.Clear();
                    int count1 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count1; i++)
                    {
                        VMTime item = new();
                        int grid_len = Int32.Parse(reader.ReadLine());
                        float grid_left = float.Parse(reader.ReadLine());
                        float grid_right = float.Parse(reader.ReadLine());
                        float grid_step = float.Parse(reader.ReadLine());
                        VMF grid_CurFunction = (VMF)int.Parse(reader.ReadLine());
                        VMGrid grid = new(grid_len, grid_left, grid_right, grid_CurFunction);
                        item.grid = grid;
                        item.time_vml_ha = double.Parse(reader.ReadLine());
                        item.time_vml_ep = double.Parse(reader.ReadLine());
                        item.coeff_ha_ep = float.Parse(reader.ReadLine());
                        benchmark.coll1.Add(item);
                    }
                    int count2 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count2; i++)
                    {
                        VMAccuracy item = new();
                        int grid_len = Int32.Parse(reader.ReadLine());
                        float grid_left = float.Parse(reader.ReadLine());
                        float grid_right = float.Parse(reader.ReadLine());
                        float grid_step = float.Parse(reader.ReadLine());
                        VMF grid_CurFunction = (VMF)int.Parse(reader.ReadLine());
                        VMGrid grid = new(grid_len, grid_left, grid_right, grid_CurFunction);
                        item.grid = grid;
                        item.max_dif_abs = double.Parse(reader.ReadLine());
                        item.max_dif_arg = double.Parse(reader.ReadLine());
                        item.HA_value = double.Parse(reader.ReadLine());
                        item.EP_value = double.Parse(reader.ReadLine());
                        benchmark.coll2.Add(item);
                    }
                }
                catch (Exception e)
                {
                    benchmark.coll1.Clear();
                    benchmark.coll2.Clear();
                    MessageBox.Show($"Невозможно загрузить файл: {e.Message}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    reader.Close();
                    return false;
                }
                finally
                {
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                benchmark.coll1.Clear();
                benchmark.coll2.Clear();
                MessageBox.Show($"Невохможно загрузить файл: {e.Message}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }


    }
}
