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
        /// one ticks occure once 10 min 
        /// deafult server will update once an hour
        /// </summary>
        public int CustomNumberOfticks { get; set; } = 6;

        

    }
}
