﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FirstSnow.Contract.Models
{
    /// <summary>
    /// User Creator model
    /// </summary>
    public class UserCreator
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Display(Name = "Имя")]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [Display(Name = "Логин")]
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ИД формулы")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public Guid FormulaId { get; set; }

        [Display(Name = "Формула")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Formula { get; set; }

        [Display(Name = "Величина резерва по умолчанию")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public decimal DefaultReserveValue { get; set; }
        [Display(Name = "Только листовые элементы")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public bool LeafOnly { get; set; }
        [Display(Name = "Период добавления элементов в резервы")]
        public int AddPeriod { get; set; }
    }
}
