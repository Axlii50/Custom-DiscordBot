using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.commands
{
    internal class DisplayAdmin : BaseCommandModule
    {
        [Command("DisplayAdmins")]
        [RequireGuild]
        public async Task DisplayAdminsCommand(CommandContext ctx)
        {

            //await ctx.RespondAsync();
        }
    }
}
