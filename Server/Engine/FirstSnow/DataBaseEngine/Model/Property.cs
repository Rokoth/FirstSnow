using DataBaseEngine.Abstract;

namespace DataBaseEngine.Model
{
    public class Property : Entity
    {
        public PropertyType PropertyType { get; set; }
        public string MaxValue { get; set; }
        public string MinValue { get; set; }
        public string DefaultValue { get; set; }
    }
}
