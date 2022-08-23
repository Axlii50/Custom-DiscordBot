using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Objects
{
    struct GameObject
    {
        public PlatformType type;
        public string Name;
        public string ImageURL;
        public string RedirectURL;
    }
}

namespace DCBotApi
{
    internal enum PlatformType
    {
        All,
        PC,
        STEAM,
        EPIC,
        XBOXONE,
    }
}