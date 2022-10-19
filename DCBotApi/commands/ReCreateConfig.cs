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
        /// recreates config file to server
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [Command("ReCreateConfig")]
        [RequireGuild]
        public async Task ReCreateConfigCommand(CommandContext ctx)
        {
            if (!Admins.AdminsID.Contains(ctx.Member.Id)) return;

            ConfigManager.CreateConfig(ctx.Guild.Id);

            await ctx.RespondAsync(Language.LanguageManager.GetLang(
                ConfigManager.GetLanguage(ctx.Guild.Id))
                .CommandResponses[ctx.Command.Name.ToLower()]);
        }
    }
}
