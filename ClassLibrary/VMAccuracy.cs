
namespace ClassLibrary
{
    public struct VMAccuracy
    {
        public VMGrid grid { get; set; }
        public double max_dif_abs { get; set; }
        public double max_dif_arg { get; set; }
        public double HA_value { get; set; }
        public double EP_value { get; set; }

        override public string ToString()
        {
            return String.Format($"Сетка: начало: {grid.left}, конец: {grid.right}, длина: {grid.len}, шаг: {grid.step}, \nмаксимальное значения модуля разности в режимах HA и EP: {max_dif_abs}, \nаргумент функции: {max_dif_arg}, зачение в HA: {HA_value}, значение в EP: {EP_value}");
        }
    }
}
