﻿using System.ComponentModel.DataAnnotations;

namespace FirstSnow.Contract.Models
{
    public class UserHistory : EntityHistory
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Логин")]
        public string Login { get; set; }
    }

}
