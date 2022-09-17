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
        /// <summary>
        /// create config for given server ID with/without free gamess channel id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="freegameschannelID"></param>
        public static void CreateConfig(ulong id, ulong freegameschannelID = 0)
        {
            Config config = new Config(id);
            config.FGChannelID = freegameschannelID;
            PrepareConfigFile(id);

            string configjson = JsonConvert.SerializeObject(config,Formatting.Indented);

            File.WriteAllText(DCBotApi.Utility.Directory.GetPath($"Configs\\{id}.txt"), configjson);
        }

        /// <summary>
        /// create config file in specific path and specific name that coresponds a ID of server
        /// </summary>
        /// <param name="id"></param>
        private static void PrepareConfigFile(ulong id)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{id}.txt");

            File.Create(ConfigPath).Dispose();

            Console.WriteLine($"Config file: \n {ConfigPath} \n {id}");
        }

        public static void UpdateConfig(ulong serverId)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string configtext = File.ReadAllText(ConfigPath);
            Config config = JsonConvert.DeserializeObject<Config>(configtext);
            File.WriteAllText(ConfigPath,
                JsonConvert.SerializeObject(config,Formatting.Indented));
        }

        //rewrite this all SET/GET functions to few universal for all of them

        /// <summary>
        /// for given type of channel set its ID in config file for given id of server
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="channelid"></param>
        /// <param name="channel"></param>
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

        /// <summary>
        /// get channel id of specific type for specific server ids
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="numberofticks"></param>
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


        public static void SetLanguage(ulong serverId, Language.LangTypes type)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string[] lines = File.ReadAllLines(ConfigPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Lang"))
                {
                    lines[i] = string.Format(@"  ""{0}"": {1},", "Lang", (int)type);
                    break;
                }
            }
            File.WriteAllLines(ConfigPath, lines);
        }

        public static Language.LangTypes GetLanguage(ulong serverId)
        {
            string ConfigPath = DCBotApi.Utility.Directory.GetPath($"Configs\\{serverId}.txt");
            string configtext = File.ReadAllText(ConfigPath);

            Config config = JsonConvert.DeserializeObject<Config>(configtext);
            return config.Lang;
        }
    }
}
