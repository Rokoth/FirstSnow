using DataBaseEngine.Abstract;
using DataBaseEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataBaseEngine.Model
{
    public class User: Entity
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    [TableName("settings")]
    public class Settings
    {
        [ColumnName("id")]
        public int Id { get; set; }

        [ColumnName("param_name")]
        public string ParamName { get; set; }

        [ColumnName("param_value")]
        public string ParamValue { get; set; }
    }

    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTimeOffset VersionDate { get; set; }
    }

    public interface IIdentity
    {
        string Login { get; set; }
        byte[] Password { get; set; }
    }

    public class Filter<T> where T : IEntity
    {
        public int? Page { get; set; }
        public int? Size { get; set; }
        public string Sort { get; set; }

        public Expression<Func<T, bool>> Selector { get; set; }
    }

    public class UserHistory : EntityHistory
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }
}
