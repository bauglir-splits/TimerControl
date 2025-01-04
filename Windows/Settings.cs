using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerControl
{
    public static class Settings
    {

        public static IConfigurationSection ConfigurationSection
        {
            get
            {
                if (configurationSection == null)
                {
                    configurationSection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings");
                }
                if (configurationSection == null) throw new Exception("Cannot load configuration!");
                return configurationSection;
            }
        }
        private static IConfigurationSection? configurationSection;

        public static string TimerStartKey
        {
            get => ConfigurationSection.GetSection("TimerStartKey").Value ?? String.Empty;
        }

        public static string TimerStopKey
        {
            get => ConfigurationSection.GetSection("TimerStopKey").Value ?? String.Empty;
        }

        public static string ServerAddress
        {
            get => ConfigurationSection.GetSection("ServerAddress").Value ?? String.Empty;
        }

        public static string ServerPort
        {
            get => ConfigurationSection.GetSection("ServerPort").Value ?? String.Empty;
        }

        public static string Endpoint
        {
            get => ConfigurationSection.GetSection("Endpoint").Value ?? String.Empty;
        }

        public static IEnumerable<KeyValuePair<string, string?>> TimerStartPostData
        {
            get => ConfigurationSection.GetSection("TimerStartPostData").AsEnumerable(true);
        }

        public static IEnumerable<KeyValuePair<string, string?>> TimerStopPostData
        {
            get => ConfigurationSection.GetSection("TimerStopPostData").AsEnumerable(true);
        }

        public static string EndpointURL
        {
            get => ServerAddress + ":" + ServerPort + Endpoint;
        }


    }
}
