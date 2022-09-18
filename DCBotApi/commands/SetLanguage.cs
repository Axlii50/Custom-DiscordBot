using DCBotApi.Configuration;
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
        [Command("SetLanguage")]
        public async Task SetLanguageCommand(CommandContext ctx, string type)
        {
            Configuration.ConfigMenager.SetLanguage(ctx.Guild.Id, (Language.LangTypes)Enum.Parse(typeof(Language.LangTypes),type));

            await ctx.RespondAsync(Language.LanguageMenager.GetLang(
                ConfigMenager.GetLanguage(ctx.Guild.Id))
                .CommandResponses[ctx.Command.Name.ToLower()]);
        }
    }
}
