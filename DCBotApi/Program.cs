using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DCBotApi.Services.ChannelPrepare;

namespace DCBotApi
{
    internal class Program
    {
        public static DiscordClient DiscordClient { get; set; }

        public static Scraper _scraper { get; set; }

#if DEBUG
        const int intervaltime = 20000;
#else
        const int intervaltime = 3600000;
#endif

        static void Main(string[] args)
        {
            DiscordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = "MTAwOTU0MzkwMDY3NzAzNDE0NQ.GNgiHd.goTNYd1uBysFr429af57VMImklHV2qzFIAWWpw",
                TokenType = TokenType.Bot,
#if DEBUG
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
#else
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Error
#endif
            });;

            var task = Task.Run(async () =>  // <- marked async
            {
                while (true)
                {
                    await Task.Delay(intervaltime); // <- await with cancellation
                    Console.WriteLine("Updating servers...");
                    Update();
                }
            });

            MainAsync().GetAwaiter().GetResult();
        }

        private static void Update()
        {
            var scrapper = new Scraper();
            foreach (var guild in DiscordClient.Guilds.Values)
            {
#if DEBUG
                //if (guild.Name != "Testowy Server dla bota") continue;
#endif
                Console.WriteLine("Updating Server: " + guild.Name + "\n");
                DiscordChannel channel = guild.Channels.Where(x => x.Value.Name == "free-games").FirstOrDefault().Value;
                ChannelUpdateService.UpdateFreeGamesChannel(channel, guild, scrapper.ExtractedData);
            }
        }

        static async Task MainAsync()
        {
            DiscordClient.GuildCreated += Events.DiscordClient_GuildCreated;
            DiscordClient.ChannelCreated += Events.DiscordClient_ChannelCreated;
            DiscordClient.GuildAvailable += Events.DiscordClient_GuildAvailable;

            DiscordClient.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.KeepEmojis,
            });

            await DiscordClient.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}