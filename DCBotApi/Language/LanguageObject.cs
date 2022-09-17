using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Language
{
    internal struct LanguageObject
    {
        public LangTypes type;

        public string FGfilltersMessage;

        public Dictionary<string,string> HelpCommandsDescription;

        public Dictionary<string, string> CommandResponses;
    }
}
