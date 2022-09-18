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
        /// <summary>
        /// display all avaiable commands
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        [Command("Help")]
        public async Task HelpCommand(CommandContext ctx)
        {
            DiscordMessageBuilder discordMessageBuilder = new DiscordMessageBuilder();
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();

            discordEmbedBuilder.Title = "Commands";

            foreach (KeyValuePair<string, string> x in
                Language.LanguageMenager.GetLang(
                    Configuration.ConfigMenager.GetLanguage(ctx.Guild.Id))
                .HelpCommandsDescription)
            {
                discordEmbedBuilder.Title = x.Key;
                discordEmbedBuilder.Description = x.Value;
                discordEmbedBuilder.Build();
                discordMessageBuilder.AddEmbed(discordEmbedBuilder.Build());
            }

            await ctx.RespondAsync(discordMessageBuilder);

            //TODO change this to embede message for better looking
            //await ctx.RespondAsync(
            //    "!Help   display all commands \n" +
            //    "!ReCreateConfig   recreates configuration file (ADMIN)\n" +
            //    "!SetMainChannel <ChannelID>   set main channel id for bot (ADMIN) \n" +
            //    "!SetFGChannel <ChannelID>   change channel for free games updates (ADMIN) \n" +
            //    "!RecreateChannelFG (ADMIN) \n" +
            //    "!SetTicks <Number> defualt number of ticks is 6 (10 min per tick)");
        }
    }
}
