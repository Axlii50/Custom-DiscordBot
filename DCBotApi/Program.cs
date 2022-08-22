using DSharpPlus;
using DSharpPlus.Entities;

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
            foreach(var guild in DiscordClient.Guilds.Values)
            {
                Console.WriteLine("Updating Server: " + guild.Name);
                DiscordChannel channel = guild.Channels.Where(x => x.Value.Name == "free-games").FirstOrDefault().Value;
                _ = new Scraper(channel);
            }
        }

        static async Task MainAsync()
        {
            DiscordClient.GuildCreated += Events.DiscordClient_GuildCreated;
            DiscordClient.ChannelCreated += Events.DiscordClient_ChannelCreated;
            DiscordClient.GuildAvailable += Events.DiscordClient_GuildAvailable;

            await DiscordClient.ConnectAsync();
            Console.WriteLine();

            await Task.Delay(-1);
        }
    }
}