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
    internal class SetMainChannel : BaseCommandModule
    {
        [Command("SetMainChannel")]
        [RequireGuild]
        public async Task SetMainChannelCommand(CommandContext ctx, ulong ChannelId)
        {
            if (!Admins.AdminsID.Contains(ctx.Member.Id)) return;

            Configuration.ConfigMenager.SetChannelId(ctx.Guild.Id, ChannelId, ChannelEnum.MainChannel);

            await ctx.RespondAsync($"Kanał ustawiony");
        }
    }
}
