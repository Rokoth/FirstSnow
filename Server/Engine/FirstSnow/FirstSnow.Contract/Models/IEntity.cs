using System;

namespace FirstSnow.Contract.Models
{
    /// <summary>
    /// interface: Guid Id field
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}