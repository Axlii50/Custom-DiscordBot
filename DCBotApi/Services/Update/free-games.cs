using DCBotApi.Objects;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Services.ChannelPrepare
{
    static partial class ChannelUpdateService
    {
        internal static void UpdateFreeGamesChannel(DiscordChannel channel, DiscordGuild server, List<GameObject> ExtractedGames)
        {
            Stopwatch timer = new Stopwatch();
            IReadOnlyList<DiscordMessage> messages = null;
            try
            {
                 messages = channel.GetMessagesAsync().Result;
            }catch(UnauthorizedAccessException e)
            {
                return;
            };

            var settings_message = messages.Last();

            PlatformType settings = PlatformType.All;
            if (settings_message.Reactions.Any(x=> x.Emoji.Name == "1️⃣")) settings |= PlatformType.PC;
            if (settings_message.Reactions.Any(x => x.Emoji.Name == "2️⃣")) settings |= PlatformType.STEAM;
            if (settings_message.Reactions.Any(x => x.Emoji.Name == "3️⃣")) settings |= PlatformType.EPIC;
            if (settings_message.Reactions.Any(x => x.Emoji.Name == "4️⃣")) settings |= PlatformType.XBOXONE;

            //if (settings != PlatformType.All) settings -= PlatformType.All;

            timer.Start();
            UpdateMessages(ExtractedGames, channel, messages.ToList(),settings);
            timer.Stop();
            
            Console.WriteLine("Updating: " + timer.Elapsed + "ms" + "\n");
        }

        private static void UpdateMessages(IEnumerable<GameObject> games,
            DiscordChannel channel,
            List<DiscordMessage> messages,
            PlatformType settings)
        {
            int changes = 0;

            //send new games
            foreach (var game in games)
            {
                if (!messages.Any(x => x.Embeds.FirstOrDefault()?.Title == game.Name))
                {
                    //if any platform match with settings it will display
                    bool add = false;
                    if (!settings.HasFlag(PlatformType.All))
                    {
                        foreach (PlatformType x in game.type)
                            if (settings.HasFlag(x))
                                add = true;
                    }
                    else
                    {
                        add = true;
                    }

                    if (add)
                    {
                        ChannelsUtil.SendMessage(CreateMessage(game), channel);
                        changes++;
                    }
                }
            }

            //remove expired games
            foreach (var message in messages)
            {
                if (message == messages.Last()) continue;

                if (!games.Any(x => x.Name == message.Embeds.FirstOrDefault()?.Title))
                {
                    ChannelsUtil.RemoveMessage(message, channel);
                    changes++;
                }
            }

            Console.WriteLine("\nchanges: " + changes);
        }

        private static DiscordEmbedBuilder CreateMessage(GameObject game)
        {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.ImageUrl = game.ImageURL;
            embedBuilder.Title = game.Name;
            embedBuilder.Url = game.RedirectURL;
            embedBuilder.Build();
            return embedBuilder;
        }
    }
}
