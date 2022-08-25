using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi
{
    internal class ChannelsUtil
    {
        /// <summary>
        /// send list of messages to specififc channel
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="channel"></param>
        public async static void SendMessages(IEnumerable<DiscordEmbedBuilder> messages, DiscordChannel channel)
        {
            foreach (var message in messages)
                await channel.SendMessageAsync(message);
        }

        /// <summary>
        /// send one message to specific channel
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channel"></param>
        public async static void SendMessage(DiscordEmbedBuilder message, DiscordChannel channel)
        {
            try
            {
                await channel.SendMessageAsync(message);

            }
            catch (UnauthorizedException e)
            {

                Console.WriteLine();
            }
        }

        public async static void SendMessage(DiscordMessageBuilder message, DiscordChannel channel)
        {
            try
            {
                await channel.SendMessageAsync(message);

            }catch(UnauthorizedException e)
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// clear given channel from all messages
        /// </summary>
        /// <param name="channel"></param>
        public async static void ClearChannel(DiscordChannel channel)
        {
            var messages = channel.GetMessagesAsync();

            if (messages.Result.Count > 0)
                try
                {
                    await channel.DeleteMessagesAsync(messages.Result);
                }
                catch (BadRequestException)
                {
                    ClearChannelOneByOne(channel);
                }
        }

        private async static void ClearChannelOneByOne(DiscordChannel channel)
        {
            var messages = channel.GetMessagesAsync();

            foreach (var message in messages.Result)
                await channel.DeleteMessageAsync(message);
        }


        public async static Task<IEnumerable<DiscordMessage>> GetMessages(DiscordChannel channel)
        {
            return await channel.GetMessagesAsync();
        }

        public async static void RemoveMessage(DiscordMessage message, DiscordChannel channel)
        {
           await channel.DeleteMessageAsync(message);
        }
    }
}
