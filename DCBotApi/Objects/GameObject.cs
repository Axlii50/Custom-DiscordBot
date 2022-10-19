using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Objects
{
    struct GameObject
    {
        /// <summary>
        /// platforms 
        /// </summary>
        public PlatformType type;

        /// <summary>
        /// game name 
        /// </summary>
        public string Name;

        /// <summary>
        /// URL for game thumbnail
        /// </summary>
        public string ImageURL;

        /// <summary>
        /// URL for the site with instructions and descriptions 
        /// </summary>
        public string RedirectURL;
    }
}

