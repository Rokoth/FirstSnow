﻿namespace FirstSnow.Contract.Models
{
    /// <summary>
    /// interface of identity classes
    /// </summary>
    public interface IIdentity
    {
        string Login { get; set; }
        string Password { get; set; }
    }
}