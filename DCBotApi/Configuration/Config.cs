using DCBotApi.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Configuration
{
    class Config
    {
        public Config(ulong serverid) => this.ServerId = serverid;
       
        public ulong ServerId { get; private set; }
        
        /// <summary>
        /// channel ids
        /// </summary>
        public ulong FGChannelID { get; set; } = 0;
        public ulong MainChannelID { get; set; } = 0;

        /// <summary>
        /// one ticks occures every 10 min 
        /// default server will update once per hour
        /// </summary>
        public int CustomNumberOfticks { get; set; } = 6;

        /// <summary>
        /// represents how many ticks have passed 
        /// update occures when current tick is equal to a custom amount of ticks 
        /// </summary>
        public int CurrentTicks { get; set; } = 0;

        /// <summary>
        /// language in what the responses are send 
        /// </summary>
        public LangTypes Lang { get; set; } = LangTypes.English;
    }
}
