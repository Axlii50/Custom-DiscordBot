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
        All = 1,
        PC = 2,
        STEAM = 4,
        EPIC = 8,
        XBOXONE = 16,
    }
}