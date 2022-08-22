using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Services.ChannelPrepare
{
    static partial class ChannelPreparedService
    {
        internal static void FreeGamesChannel(DiscordChannel channel, DiscordGuild server)
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
        }
    }
}
