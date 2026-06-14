using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.Models
{
    public class UserSettings
    {
        public string UI_Color { get; set; }
        public User User { get; set; } = null!;

        private UserSettings() { }
        public UserSettings(string UI_Color)
        {
            this.UI_Color = UI_Color;
        }
    }
}
