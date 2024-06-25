using System;

namespace FirstSnow.Contract.Model
{
    /// <summary>
    /// interface: Guid Id field
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}