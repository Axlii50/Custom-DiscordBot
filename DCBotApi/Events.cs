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

            Program._scraper.GetGiveAways();

            //get free games channel id and prepare channel
            ulong FreeGamesChannelID = Freegameschannel(e);

            //create config file with server id and channel id 
            Console.WriteLine("Creating Config File");
            ConfigMenager.CreateConfig(e.Guild.Id, FreeGamesChannelID);

            return null;
        }

        private static ulong Freegameschannel(GuildCreateEventArgs e)
        {
            //get all channels on server
            var channels = e.Guild.Channels;
            DiscordChannel newchannel = null;

            IEnumerable<KeyValuePair<ulong, DSharpPlus.Entities.DiscordChannel>> channel;

            //check if there is channel with name "free games"
            //if there is non of such channel then create new and save id 
            if ((channel = channels.Where(x => x.Value.Name == "free-games")).Count() > 0)
            {
                Console.WriteLine("Channel found on server: " + e.Guild.Name);
                //preapre channel
                ChannelPreparedService.PrepareFreeGamesChannel(channel.First().Value, e.Guild);
            }
            else
            {
                Console.WriteLine("Channel not found: " + e.Guild.Name);
                newchannel = e.Guild.CreateChannelAsync("free-games", ChannelType.Text).Result;
                //prepare channel
                ChannelPreparedService.PrepareFreeGamesChannel(newchannel, e.Guild);
            }

            Thread.Sleep(2000);
           
            if (newchannel == null)
            {
                //update channel with all scraped data from webpage
                ChannelUpdateService.UpdateFreeGamesChannel(channel.First().Value, e.Guild, Program._scraper.GiveAways.GetList);
                return channel.First().Value.Id;
            }
            else
            {
                //update channel with all scraped data from webpage
                ChannelUpdateService.UpdateFreeGamesChannel(newchannel, e.Guild, Program._scraper.GiveAways.GetList);
                return newchannel.Id;
            }
        }

        internal async static Task DiscordClient_GuildAvailable(DiscordClient sender, DSharpPlus.EventArgs.GuildCreateEventArgs e)
        {
#if DEBUG
            if (e.Guild.Name != "Testowy Server dla bota") return;
#endif
            Console.WriteLine("Server: " + e.Guild.Name);
            //get all channel on server 
            var channels = e.Guild.Channels;
            DiscordChannel Channel = null;
            //retreview channel id saved in config file 
            ulong channelid = ConfigMenager.GetChannelID(e.Guild.Id, ChannelEnum.FGChannel);

            if (channelid == 0) return;
            //check if channel with CHANNELID exist on server
            //if not create new channel with name "free-games"
            //save channel id to config file
            //prepare channel
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

            
            //update channel with data
            ChannelUpdateService.UpdateFreeGamesChannel(Channel, e.Guild, Program._scraper.GiveAways.GetList);
            return;
        }
    }
}
