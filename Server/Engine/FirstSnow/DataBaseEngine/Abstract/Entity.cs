﻿using DataBaseEngine.Attributes;
using DataBaseEngine.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseEngine.Abstract
{
    public abstract class Entity : IEntity
    {
        [PrimaryKey]
        [ColumnName("id")]
        public Guid Id { get; set; }
        [ColumnName("version_date")]
        public DateTimeOffset VersionDate { get; set; }
        [ColumnName("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}
