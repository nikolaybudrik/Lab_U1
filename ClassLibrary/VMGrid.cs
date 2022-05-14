
namespace ClassLibrary
{
    public class VMGrid
    {
        public int len { get; set; }
        public float left { get; set; }
        public float right { get; set; }
        public float step
        {
            get
            {
                return Math.Abs(right - left) / len;
            }
        }
        public VMF curr_function { get; set; }
        public VMGrid(VMGrid other)
        {
            len = other.len;
            left = other.left;
            right = other.right;
            curr_function = other.curr_function;
        }
        public VMGrid(int len, float left, float right, VMF curr_function)
        {
            this.len = len;
            this.right = right;
            this.left = left;
            this.curr_function = curr_function;
        }
        
    }
}
