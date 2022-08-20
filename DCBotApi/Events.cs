using DCBotApi.Services.ChannelPrepare;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi
{
    class Events
    {
        internal static Task DiscordClient_GuildCreated(DiscordClient sender, DSharpPlus.EventArgs.GuildCreateEventArgs e)
        {
            var channels = e.Guild.Channels;
            DiscordChannel newchannel = null;
            IEnumerable<KeyValuePair<ulong, DSharpPlus.Entities.DiscordChannel>> channel;

            if ((channel = channels.Where(x => x.Value.Name == "free-games")).Count() > 0)
            {
                Console.WriteLine("Channel found on server: " + e.Guild.Name);

                ChannelPreparedService.FreeGamesChannel(channel.First().Value, e.Guild);
            }
            else
            {
                Console.WriteLine("Channel not found: " + e.Guild.Name);
                newchannel = e.Guild.CreateChannelAsync("free-games", ChannelType.Text).Result;
                ChannelPreparedService.FreeGamesChannel(newchannel, e.Guild);
            }

            if (newchannel == null)
                _ = new Scraper(channel.First().Value);
            else
                _ = new Scraper(newchannel);

            return null;
        }


        internal static Task DiscordClient_ChannelCreated(DiscordClient sender, DSharpPlus.EventArgs.ChannelCreateEventArgs e)
        {
            return null;
        }


        internal static Task DiscordClient_GuildAvailable(DiscordClient sender, DSharpPlus.EventArgs.GuildCreateEventArgs e)
        {
            var channels = e.Guild.Channels;
            IEnumerable<KeyValuePair<ulong, DSharpPlus.Entities.DiscordChannel>> channel;

            if ((channel = channels.Where(x => x.Value.Name == "free-games")).Count() > 0)
            {
                Console.Write("\n" + e.Guild.Name + " -> Channel found");

                ChannelsUtil.ClearChannel(channel.First().Value);
                Console.Write(" -> Channel cleared \n");
            }
            else
            {
                Console.Write("\n" + e.Guild.Name + " -> Channel not found");
                e.Guild.CreateChannelAsync("free-games", ChannelType.Text);
                Console.Write(" -> Channel Created \n");
            }

            if (channel.Count() > 0)
                _ = new Scraper(channel.First().Value);

            return null;
        }

        internal static Task DiscordClient_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {

            return null;
        }
    }
}
