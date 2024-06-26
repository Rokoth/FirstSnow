using DataBaseEngine.Abstract;

namespace DataBaseEngine.Model
{
    public abstract class Position : Entity
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
