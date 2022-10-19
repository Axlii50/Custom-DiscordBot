using DCBotApi.Configuration;
using DCBotApi.Language;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.commands
{
    partial class Commands : BaseCommandModule
    {
        [Command("Languages")]
        public async Task LanguagesCommand(CommandContext ctx)
        {
         string Langs = String.Empty;
            foreach (var x in (LangTypes[])Enum.GetValues(typeof(LangTypes)))
                Langs += x.ToString() + "\n";

            await ctx.RespondAsync(Language.LanguageManager.GetLang(
               ConfigManager.GetLanguage(ctx.Guild.Id))
               .CommandResponses[ctx.Command.Name.ToLower()] + Langs);

        }
    }
}
