using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DCBotApi.Services.ChannelPrepare;
using DSharpPlus.CommandsNext;
using DCBotApi.commands;
using DCBotApi.Configuration;
using DCBotApi.Sources.Games;
using DCBotApi.Language;
using System.Reflection;

namespace DCBotApi
{
    internal class Program
    {
        public static DiscordClient DiscordClient { get; set; }

        public static GamerPowerApi _scraper { get; set; }

#if DEBUG
        const int intervaltime = 20000;
#else
        const int intervaltime = 600000;
#endif


        static void Main(string[] args)
        {
            _scraper = new GamerPowerApi();
            _scraper.GetGiveAways().Wait();

            //create folder for configs
            if (!Directory.Exists(DCBotApi.Utility.Directory.GetPath("Configs")))
                Directory.CreateDirectory(DCBotApi.Utility.Directory.GetPath("Configs"));

            LanguageManager.LoadLanguages();

            DiscordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = File.ReadAllText("TokenFile.txt"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.Guilds | DiscordIntents.GuildMessages,
#if DEBUG
                //MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Error
#else
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Error
#endif
            }); ;

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

        static void Update()
        {

            _scraper.GetGiveAways();

            foreach (var guild in DiscordClient.Guilds.Values)
            {
#if DEBUG
                if (guild.Name != "Testowy Server dla bota") continue;
#endif
                int ticks = 0;
                int Interval = 0;
                if ((ticks = ConfigMenager.GetTicks(guild.Id)) == (Interval = ConfigMenager.GetIntervalTicks(guild.Id)) - 1)
                {
                    Console.WriteLine("Updating Server: " + guild.Name + "\n");
                    ulong channelid = ConfigMenager.GetChannelID(guild.Id, ChannelEnum.FGChannel);
                    DiscordChannel channel = guild.Channels.Where(x => x.Key == channelid).FirstOrDefault().Value;
                    if (channel == null)
                    {
                        Console.WriteLine("Channel couldnt be found by id: " + channelid);
                        continue;
                    }
                    ChannelUpdateService.UpdateFreeGamesChannel(channel, guild, _scraper.GiveAways.GetList);

                    ConfigMenager.SetTicks(guild.Id, 0);
                }
                else
                {
                    Console.WriteLine($"Ticks update for server: {guild.Name.PadLeft(32)} to: {++ticks}/{Interval}" +
                        $"\n aproximate time for update: {(Math.Abs(Interval - ticks) * 10)} min");

                    ConfigMenager.SetTicks(guild.Id, ticks++);
                }
            }
        }

        static async Task MainAsync()
        {
            DiscordClient.GuildCreated += Events.DiscordClient_GuildCreated;
            DiscordClient.GuildAvailable += Events.DiscordClient_GuildAvailable;

            DiscordClient.UseInteractivity(new InteractivityConfiguration()
            {
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.KeepEmojis,
            });

            var commands = DiscordClient.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" },
                EnableDefaultHelp = false
            }); ;

            RegisterCommands(commands);

            await DiscordClient.ConnectAsync();

            await Task.Delay(-1);
        }

        private static void RegisterCommands(CommandsNextExtension commands)
        {
            commands.RegisterCommands<Commands>();
        }
    }
}