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
    internal class SetFGChannel : BaseCommandModule
    {
        [Command("SetFGChannel")]
        public async Task SetFGChannelCommand(CommandContext ctx, ulong ChannelId)
        {
            Configuration.ConfigMenager.SetChannelId(ctx.Guild.Id, ChannelId, ChannelEnum.FGChannel);

            await ctx.RespondAsync($"Kanał ustawiony");
        }
    }
}
