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
    internal class SetTicks : BaseCommandModule
    {
        [Command("SetTicks")]
        [RequireGuild]
        public async Task SetTicksCommand(CommandContext ctx, int AmountOfTicks)
        {
            if (!Admins.AdminsID.Contains(ctx.Member.Id)) return;

            ConfigMenager.SetTicks(ctx.Guild.Id, AmountOfTicks);

            await ctx.RespondAsync($"Amount of current ticks per update is: " + AmountOfTicks);
        }
    }
}
