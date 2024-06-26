using System;

namespace DataBaseEngine.Model
{
    public class PersonProperty : Property
    {
        public Guid PersonId { get; set; }
        public string Value { get; set; }
    }
}
