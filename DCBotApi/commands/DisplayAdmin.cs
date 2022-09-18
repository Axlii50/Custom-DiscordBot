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
    partial class Commands : BaseCommandModule
    {
        [Command("DisplayAdmins")]
        public async Task DisplayAdminsCommand(CommandContext ctx)
        {

            //await ctx.RespondAsync();
        }
    }
}
