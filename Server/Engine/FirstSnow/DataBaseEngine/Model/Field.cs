using DataBaseEngine.Abstract;
using System;
using System.Collections.Generic;

namespace DataBaseEngine.Model
{
    public class Field : Entity
    {
        public Guid MapId { get; set; }
        public FieldPosition Position { get; set; }
        public virtual ICollection<FieldProperty> Properties { get; set; }
    }
}
