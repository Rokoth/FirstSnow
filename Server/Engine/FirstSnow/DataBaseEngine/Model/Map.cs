using DataBaseEngine.Abstract;
using System;
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

    public abstract class Position : Entity
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MapPosition : Position
    {
        public Guid MapId { get; set; }
    }

    public class FieldPosition : Position
    {
        public Guid FieldId { get; set; }
    }

    public class Field : Entity
    {
        public Guid MapId { get; set; }
        public FieldPosition Position { get; set; }
        public virtual ICollection<FieldProperty> Properties { get; set; }
    }

    public class Person : Entity
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public Guid MapId { get; set; }
        public FieldPosition Position { get; set; }
        public virtual ICollection<PersonProperty> PersonProperties { get; set; }
    }

    public class PersonProperty : Property
    {
        public Guid PersonId { get; set; }
        public string Value { get; set; }
    }

    public class FieldProperty : Property
    {
        public Guid FieldId { get; set; }
        public string Value { get; set; }
    }

    public class Property : Entity
    {
        public PropertyType PropertyType { get; set; }
        public string MaxValue { get; set; }
        public string MinValue { get; set; }
        public string DefaultValue { get; set; }
    }

    public class PropertyType : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
