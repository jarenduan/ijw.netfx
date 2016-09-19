﻿using System;
using System.Configuration;

namespace ijw.Client {
    public class AppConfig {
        private Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public string this[string settingName]
        {
            get
            {
                var setting = config.AppSettings.Settings[settingName];
                return setting?.Value;
            }
            set
            {
                var setting = config.AppSettings.Settings[settingName];
                if (setting == null)
                    throw new NullReferenceException();
                else
                    setting.Value = value;
            }
        }
    }
}