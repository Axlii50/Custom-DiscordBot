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

            foreach (KeyValuePair<string, string> x in Language.LanguageManager.GetLang(
                    Configuration.ConfigMenager.GetLanguage(ctx.Guild.Id)).HelpCommandsDescription)
            {
                discordEmbedBuilder.Description += $"{x.Key}: {x.Value} \n";
            }
            discordEmbedBuilder.Build();
            discordMessageBuilder.AddEmbed(discordEmbedBuilder.Build());

            await ctx.RespondAsync(discordMessageBuilder);
        }
    }
}
