
namespace ClassLibrary
{
    public struct VMTime
    {
        public VMGrid grid { get; set; }
        public double time_vml_ha { get; set; }
        public double time_vml_ep { get; set; }
        public float coeff_ha_ep { get; set; }

        override public string ToString()
        {
            return String.Format($"Сетка: начало: {grid.left}, конец: {grid.right}, длина: {grid.len}, шаг: {grid.step}, \nвремя в HA: {time_vml_ha}, \nвремя в EP: {time_vml_ep}, \nкоэффициент HA и EP: {coeff_ha_ep}");
        }
    }
}
