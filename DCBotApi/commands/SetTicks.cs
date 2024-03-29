﻿using DCBotApi.Configuration;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.commands
{
    partial class Commands : BaseCommandModule
    {
        [Command("SetTicks")]
        [RequireGuild]
        public async Task SetTicksCommand(CommandContext ctx, int AmountOfTicks)
        {
            if (!Admins.AdminsID.Contains(ctx.Member.Id)) return;

            ConfigMenager.SetTicks(ctx.Guild.Id, AmountOfTicks);

            await ctx.RespondAsync(Language.LanguageManager.GetLang(
                ConfigMenager.GetLanguage(ctx.Guild.Id))
                .CommandResponses[ctx.Command.Name.ToLower()] + AmountOfTicks);
        }
    }
}
