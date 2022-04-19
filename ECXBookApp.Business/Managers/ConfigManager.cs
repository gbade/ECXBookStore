using System;
using ECXBookApp.Business.Contracts;

namespace ECXBookApp.Business.Managers
{
    public class ConfigManager : IConfigManager
    {
        public string DataSource { get; set; }
    }
}
