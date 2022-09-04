using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.commands
{
    internal class Help : BaseCommandModule
    {
        /// <summary>
        /// display all avaiable commands
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [Command("Help")]
        public async Task HelpCommand(CommandContext ctx)
        {
            //TODO change this to embede message for better looking
            await ctx.RespondAsync(
                "!Help display all commands \n" +
                $"!ReCreateConfig  comming soon\n" +
                $"!SetMainChannel <ChannelID>   set main channel id for bot");
        }
    }
}
