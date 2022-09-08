using DCBotApi.Configuration;
using DCBotApi.Services.ChannelPrepare;
using DCBotApi.Sources.Games;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
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
            Console.WriteLine("Joined server:" + e.Guild.Name);


            ulong FreeGamesChannelID = Freegameschannel(e);

            ConfigMenager.CreateConfig(e.Guild.Id, FreeGamesChannelID);

            return null;
        }

        private static ulong Freegameschannel(GuildCreateEventArgs e)
        {
            var channels = e.Guild.Channels;
            DiscordChannel newchannel = null;
            IEnumerable<KeyValuePair<ulong, DSharpPlus.Entities.DiscordChannel>> channel;

            if ((channel = channels.Where(x => x.Value.Name == "free-games")).Count() > 0)
            {
                Console.WriteLine("Channel found on server: " + e.Guild.Name);
                ChannelPreparedService.PrepareFreeGamesChannel(channel.First().Value, e.Guild);
            }
            else
            {
                Console.WriteLine("Channel not found: " + e.Guild.Name);
                newchannel = e.Guild.CreateChannelAsync("free-games", ChannelType.Text).Result;
                ChannelPreparedService.PrepareFreeGamesChannel(newchannel, e.Guild);
            }

            GamerPowerScraper scraped = null;
            if (newchannel == null)
            {
                scraped = new GamerPowerScraper();
                ChannelUpdateService.UpdateFreeGamesChannel(channel.First().Value, e.Guild, scraped.ExtractedData);
                return channel.First().Value.Id;
            }
            else
            {
                scraped = new GamerPowerScraper();
                ChannelUpdateService.UpdateFreeGamesChannel(newchannel, e.Guild, scraped.ExtractedData);
                return newchannel.Id;
            }
        }

        internal async static Task DiscordClient_GuildAvailable(DiscordClient sender, DSharpPlus.EventArgs.GuildCreateEventArgs e)
        {
#if DEBUG
            if (e.Guild.Name != "Testowy Server dla bota") return;
#endif
            Console.WriteLine("Server: " + e.Guild.Name);
            var channels = e.Guild.Channels;
            DiscordChannel Channel = null;
            ulong channelid = ConfigMenager.GetChannelID(e.Guild.Id, ChannelEnum.FGChannel);
            if ((Channel = channels.Where(x => x.Key == channelid).FirstOrDefault().Value) != null)
            {
                Console.WriteLine("Channel found on server: " + e.Guild.Name);
            }
            else
            {
                Console.WriteLine("Channel not found: " + e.Guild.Name);
                Channel = await e.Guild.CreateChannelAsync("free-games", ChannelType.Text);
                Console.WriteLine(e.Guild.Name + ": Channel Created");
                ConfigMenager.SetChannelId(e.Guild.Id, Channel.Id, ChannelEnum.FGChannel);
                ChannelPreparedService.PrepareFreeGamesChannel(Channel, e.Guild);
            }
            Console.WriteLine();

            GamerPowerScraper scraped = new GamerPowerScraper();
            ChannelUpdateService.UpdateFreeGamesChannel(Channel, e.Guild, scraped.ExtractedData);
            return;
        }
    }
}
