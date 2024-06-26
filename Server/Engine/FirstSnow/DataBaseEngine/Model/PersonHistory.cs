﻿using DataBaseEngine.Abstract;
using DataBaseEngine.Attributes;
using System;

namespace DataBaseEngine.Model
{
    [TableName("h_person")]
    public class PersonHistory : EntityHistory
    {
        [ColumnName("name")]
        public string Name { get; set; }
        [ColumnName("description")]
        public string Description { get; set; }
        [ColumnName("userid")]
        public Guid UserId { get; set; }
        [ColumnName("location_id")]
        public Guid LocationId { get; set; }
    }
}
