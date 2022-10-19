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
        /// <summary>
        /// display all avaiable commands
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [Command("UpdateConfig")]
        public async Task UpdateConfigCommand(CommandContext ctx)
        {
            if (!Admins.AdminsID.Contains(ctx.Member.Id)) return;

            Configuration.ConfigMenager.UpdateConfig(ctx.Guild.Id);

            await ctx.RespondAsync(Language.LanguageManager.GetLang(
               ConfigMenager.GetLanguage(ctx.Guild.Id))
               .CommandResponses[ctx.Command.Name.ToLower()]);
        }
    }
}
