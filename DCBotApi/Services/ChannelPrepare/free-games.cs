using DCBotApi.Language;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Services.ChannelPrepare
{
    static partial class ChannelPreparedService
    {
        internal static void PrepareFreeGamesChannel(DiscordChannel channel, DiscordGuild server)
        {
            var roles = server.Roles;
            Console.WriteLine("changing roles on server: " + server.Name);

            foreach (var role in roles)
            {
                if (role.Value.Name == "PIPI")
                {
                    channel.AddOverwriteAsync(
                                       role.Value,
                                       DSharpPlus.Permissions.ManageMessages | DSharpPlus.Permissions.SendMessages,
                                       DSharpPlus.Permissions.None);
                }
                else
                {
                    channel.AddOverwriteAsync(
                                      role.Value,
                                      DSharpPlus.Permissions.None,
                                      DSharpPlus.Permissions.ManageMessages | DSharpPlus.Permissions.SendMessages);
                }
            }

            //clearing channel
            ChannelsUtil.ClearChannel(channel);

            //preapre settings message
            SettingsFreeGames(server, channel);
        }

        private static void SettingsFreeGames(DiscordGuild server, DiscordChannel channel)
        {
            DiscordMessageBuilder message = new DiscordMessageBuilder();

            LanguageObject lang = LanguageManager.GetLang(Configuration.ConfigMenager.GetLanguage(server.Id));

            message.Content = $"{lang.FGfilltersMessage}\n"
                + ":one: - PC\n"
                + ":two: - Steam\n"
                + ":three: - Epic\n"
                + ":four: - Xbox one\n";

            ChannelsUtil.SendMessage(message, channel);
        }
    }
}
