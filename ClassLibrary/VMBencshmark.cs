using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ClassLibrary
{
    public class VMBenchmark : INotifyPropertyChanged
    {

        public ObservableCollection<VMTime> coll1 { get; set; }
        public ObservableCollection<VMAccuracy> coll2 { get; set; }

        public float Max_HA_EP_Coeff
        {
            get
            {
                if (coll1.Count < 1)
                    return -1;
                float max_coeff = coll1[0].coeff_ha_ep;
                foreach (VMTime item in coll1)
                {
                    if (item.coeff_ha_ep > max_coeff)
                        max_coeff = item.coeff_ha_ep;
                }
                return max_coeff;
            }
        }
        public float Min_HA_EP_Coeff
        {
            get
            {
                if (coll1.Count < 1)
                    return -1;
                float min_coeff = coll1[0].coeff_ha_ep;
                foreach (VMTime item in coll1)
                {
                    if (item.coeff_ha_ep < min_coeff)
                        min_coeff = item.coeff_ha_ep;
                }
                return min_coeff;
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        public VMBenchmark()
        {
            coll1 = new();
            coll2 = new();
            coll1.CollectionChanged += Collection_CollectionChanged;
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Min_HA_EP_Coeff)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Max_HA_EP_Coeff)));
        }


        [DllImport("..\\..\\..\\..\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double MKL_Function(int n, double[] arr, float[] arr_floats, float[] results_time, int curr_function,
            double[] res_ha, double[] res_ep, float[] res_ha_floats, float[] res_ep_floats);

        public void AddVMTime(VMGrid grid)
        {
            VMTime val = new();
            val.grid = new(grid);
            double[] vec = new double[grid.len];
            float[] vec_floats = new float[grid.len];
            for (int i = 0; i < grid.len; i++)
            {
                vec[i] = grid.left + (i * grid.step);
                vec_floats[i] = grid.left + (i * grid.step);
            }

            double[] res_ha_double = new double[grid.len];
            double[] res_ep_double = new double[grid.len];
            float[] res_ha_float = new float[grid.len];
            float[] res_ep_float = new float[grid.len];
            float[] results_time = new float[3];

            int ret = (int)MKL_Function(grid.len, vec, vec_floats, results_time, (int)(grid.curr_function), res_ha_double, res_ep_double, res_ha_float, res_ep_float);

            if (ret != 0)
            {
                throw new InvalidCastException($"Функция MKL_Function неудачно завершила работу: {ret}");
            }

            val.time_vml_ha = results_time[0];
            val.time_vml_ep = results_time[1];
            val.coeff_ha_ep = results_time[2];
            coll1.Add(val);
        }

        public void AddVMAccuracy(VMGrid grid)
        {
            VMAccuracy val = new();
            val.grid = new(grid);
            double[] vec = new double[grid.len];
            float[] vec_floats = new float[grid.len];
            for (int i = 0; i < grid.len; i++)
            {
                vec[i] = grid.left + (i * grid.step);
                vec_floats[i] = grid.left + (i * grid.step);
            }
            double[] res_ha_double = new double[grid.len];
            double[] res_ep_double = new double[grid.len];
            float[] res_ha_float = new float[grid.len];
            float[] res_ep_float = new float[grid.len];
            float[] results_time = new float[3];

            int ret = (int)MKL_Function(grid.len, vec, vec_floats, results_time, (int)(grid.curr_function), res_ha_double, res_ep_double, res_ha_float, res_ep_float);

            if (ret != 0)
                throw new InvalidCastException($"Функция MKL_Function неудачно завершила работу: {ret}");

            val.max_dif_abs = 0;
            if ((int)grid.curr_function == 0 || (int)grid.curr_function == 2)    // Если массив аргументов типа double (vmdLn, vmdLGamma)
            {
                for (int i = 0; i < grid.len; i++)
                {
                    if (Math.Abs(res_ha_double[i] - res_ep_double[i]) > val.max_dif_abs)
                    {
                        val.max_dif_abs = Math.Abs(res_ha_double[i] - res_ep_double[i]);
                        val.max_dif_arg = vec[i];
                        val.HA_value = res_ha_double[i];
                        val.EP_value = res_ep_double[i];
                    }
                }
            }
            else if ((int)grid.curr_function == 1 || (int)grid.curr_function == 3)    // Если массив аргументов типа float (vmsLn, vmsLGamma)
            {
                for (int i = 0; i < grid.len; i++)
                {
                    if (Math.Abs(res_ha_float[i] - res_ep_float[i]) > val.max_dif_abs)
                    {
                        val.max_dif_abs = Math.Abs(res_ha_float[i] - res_ep_float[i]);
                        val.max_dif_arg = vec[i];
                        val.HA_value = res_ha_float[i];
                        val.EP_value = res_ep_float[i];
                    }
                }
            }
            coll2.Add(val);
        }

    }
}
