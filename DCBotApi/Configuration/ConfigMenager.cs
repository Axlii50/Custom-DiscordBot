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
        public static void CreateConfig(ulong id, ulong freegameschannelID = 0)
        {
            Config config = new Config(id);
            config.FGChannelID = freegameschannelID;
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

        //rewrite this all SET/GET functions to few universal for all of them

        public static void SetChannelId(ulong serverId, ulong channelid, ChannelEnum channel)
        {
            string ChannelName = channel.ToString() + "ID";
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string[] lines = File.ReadAllLines(ConfigPath);
            for(int i = 0; i<lines.Length;i++)
            {
                if (lines[i].Contains(ChannelName))
                {
                    lines[i] = string.Format(@"  ""{0}"": {1},",ChannelName,channelid);
                    break;
                }
            }
            File.WriteAllLines(ConfigPath, lines);
        }


        public static ulong GetChannelID(ulong serverId, ChannelEnum channel)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string configtext = File.ReadAllText(ConfigPath);

            Config config = JsonConvert.DeserializeObject<Config>(configtext);

            switch (channel)
            {
                case ChannelEnum.FGChannel: return config.FGChannelID;
                case ChannelEnum.MainChannel: return config.MainChannelID;

                default: return 0;
            }
        }


        public static void SetIntervalTicks(ulong serverId, int numberofticks)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string[] lines = File.ReadAllLines(ConfigPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("CustomNumberOfticks"))
                {
                    lines[i] = string.Format(@"  ""{0}"": {1},", "CustomNumberOfticks", numberofticks);
                    break;
                }
            }
            File.WriteAllLines(ConfigPath, lines);
        }


        public static int GetIntervalTicks(ulong serverId)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string configtext = File.ReadAllText(ConfigPath);

            Config config = JsonConvert.DeserializeObject<Config>(configtext);
            return config.CustomNumberOfticks;
        }


        public static void SetTicks(ulong serverId, int numberofticks)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string[] lines = File.ReadAllLines(ConfigPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("CurrentTicks"))
                {
                    lines[i] = string.Format(@"  ""{0}"": {1},", "CurrentTicks", numberofticks);
                    break;
                }
            }
            File.WriteAllLines(ConfigPath, lines);
        }

        public static int GetTicks(ulong serverId)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string configtext = File.ReadAllText(ConfigPath);

            Config config = JsonConvert.DeserializeObject<Config>(configtext);
            return config.CurrentTicks;
        }
    }
}
