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
        [Command("SetFGChannel")]
        [RequireGuild]
        public async Task SetFGChannelCommand(CommandContext ctx, ulong ChannelId)
        {
            if (!Admins.AdminsID.Contains(ctx.Member.Id)) return;

            Configuration.ConfigManager.SetChannelId(ctx.Guild.Id, ChannelId, ChannelEnum.FGChannel);

            Console.WriteLine("Channel free games id has been updated on: " + ctx.Guild.Name);

            await ctx.RespondAsync(Language.LanguageManager.GetLang(
                ConfigManager.GetLanguage(ctx.Guild.Id))
                .CommandResponses[ctx.Command.Name.ToLower()]);
        }
    }
}
