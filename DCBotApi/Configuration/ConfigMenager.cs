using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Configuration
{
    internal class ConfigMenager
    {
        public static void CreateConfig(ulong id)
        {
            Config config = new Config(id);
            PrepareConfigFile(id);

            string configjson = JsonConvert.SerializeObject(config,Formatting.Indented);

            File.WriteAllText(DCBotApi.Utility.Directory.GetPath($"Configs\\{id}.txt"), configjson);
        }

        private static void PrepareConfigFile(ulong id)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{id}.txt");

            File.Create(ConfigPath).Dispose();

            Console.WriteLine($"Config file: \n {ConfigPath} \n {id}");
        }



    }
}
