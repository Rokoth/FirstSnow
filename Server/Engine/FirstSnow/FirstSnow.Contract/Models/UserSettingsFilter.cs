using System;

namespace FirstSnow.Contract.Models
{
    public class UserSettingsFilter : Filter<UserSettings>
    {
        public UserSettingsFilter(int size, int page, string sort) : base(size, page, sort)
        {

        }

    }
}
