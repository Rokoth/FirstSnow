using DataBaseEngine.Abstract;
using System.Collections.Generic;

namespace DataBaseEngine.Model
{
    public class Map : Entity
    {
        public string Name { get; set; }

        public virtual MapPosition Position { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
