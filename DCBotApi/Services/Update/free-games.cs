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
        internal static void UpdateFreeGamesChannel(DiscordChannel channel, DiscordGuild server, List<GameObject> games)
        {
            Stopwatch timer = new Stopwatch();
            var messages = channel.GetMessagesAsync().Result;
            //var settings = messages.First();

            timer.Start();
            UpdateMessages(games, channel, messages.ToList());
            timer.Stop();
            
            Console.WriteLine("Updating: " + timer.Elapsed + "ms" + "\n");
        }

        private static void UpdateMessages(IEnumerable<GameObject> games, DiscordChannel channel, List<DiscordMessage> messages)
        {
            int changes = 0;

            foreach (var game in games)
            {
                if (!messages.Any(x => x.Embeds.FirstOrDefault()?.Title == game.Name))
                {
                    ChannelsUtil.SendMessage(CreateMessage(game), channel);
                    changes++;
                }
            }

            foreach (var message in messages)
            {
                if (message.Content.StartsWith("Ustawienia")) continue;

                if (!games.Any(x => x.Name == message.Embeds.FirstOrDefault()?.Title))
                {
                    ChannelsUtil.RemoveMessage(message, channel);
                    changes++;
                }
            }

            Console.WriteLine("\nchanges: " + changes + " ");
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
