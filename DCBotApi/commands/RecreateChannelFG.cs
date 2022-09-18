using DCBotApi.Configuration;
using DCBotApi.Services.ChannelPrepare;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.commands
{
    partial class Commands : BaseCommandModule
    {
        [Command("RecreateFGChannel")]
        [RequireGuild]
        public async Task RecreateFGChannelCommand(CommandContext ctx)
        {
            if (!Admins.AdminsID.Contains(ctx.Member.Id)) return;

            DiscordChannel Channel = await ctx.Guild.CreateChannelAsync("free-games", ChannelType.Text);

            ChannelPreparedService.PrepareFreeGamesChannel(Channel, ctx.Guild);

            ConfigMenager.SetChannelId(ctx.Guild.Id, Channel.Id, ChannelEnum.FGChannel);

            await ctx.RespondAsync(Language.LanguageMenager.GetLang(
                ConfigMenager.GetLanguage(ctx.Guild.Id))
                .CommandResponses[ctx.Command.Name.ToLower()]);
        }
    }
}
