﻿using System;

namespace FirstSnow.Contract.Model
{
    public class UserSettingsFilter : Filter<UserSettings>
    {
        public UserSettingsFilter(int size, int page, string sort) : base(size, page, sort)
        {
                      
        }       
        
    }
}
