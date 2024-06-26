using System;

namespace DataBaseEngine.Model
{
    public class FieldProperty : Property
    {
        public Guid FieldId { get; set; }
        public string Value { get; set; }
    }
}
