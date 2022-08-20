using DSharpPlus;
using DSharpPlus.Entities;

namespace DCBotApi
{
    internal class Program
    {
        public static DiscordClient DiscordClient { get; set; }

        public static Scraper _scraper { get; set; }

        const int intervaltime = 3600000;

        static void Main(string[] args)
        {

            DiscordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = "MTAwOTU0MzkwMDY3NzAzNDE0NQ.GNgiHd.goTNYd1uBysFr429af57VMImklHV2qzFIAWWpw",
                TokenType = TokenType.Bot,
            });;

            var task = Task.Run(async () =>  // <- marked async
            {
                Console.WriteLine("test");
                while (true)
                {

                    await Task.Delay(intervaltime); // <- await with cancellation
                    Console.WriteLine("Updating...");
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
                //ChannelsUtil.ClearChannel(channel);
                _ = new Scraper(channel);
            }
        }

        static async Task MainAsync()
        {
            DiscordClient.GuildCreated += Events.DiscordClient_GuildCreated;
            DiscordClient.GuildAvailable += Events.DiscordClient_GuildAvailable;

            await DiscordClient.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}