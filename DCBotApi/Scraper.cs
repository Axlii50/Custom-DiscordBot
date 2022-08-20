using DSharpPlus;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi
{
    internal class Scraper
    {
        private DiscordChannel Channel;
        public Scraper(DiscordChannel Channel)
        {
            this.Channel = Channel;
            Task.Run(() =>
            {
                BeginScraping();
            });
        }

        public async void BeginScraping()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://www.gamerpower.com/all/free-games");

            HtmlDocument htmlresponse = new HtmlDocument();
            htmlresponse.LoadHtml(response);

            //get all containers with informations
            IEnumerable<HtmlNode> nodes =
                  htmlresponse.DocumentNode.Descendants(0)
                     .Where(n => n.HasClass("col-xl-3")
                              && n.HasClass("col-lg-4")
                              && n.HasClass("col-md-6")
                              && n.HasClass("col-sm-6")
                              && n.HasClass("mb-4"));

            List<HtmlNode> validNodes = new List<HtmlNode>();

            HtmlDocument html = new HtmlDocument();
            foreach (HtmlNode x in nodes)
            {
                html.LoadHtml(x.InnerHtml);

                //if containers is not expired
                if (!html.DocumentNode.Descendants().Any(x => x.HasClass("expire_stamp")))
                {
                    //load new html to proccesing
                    html.LoadHtml(x.InnerHtml);

                    //get game name 
                    string? GameName = html.DocumentNode.Descendants().FirstOrDefault(x => x.HasClass("card-title"))?.InnerText;

                    if (GameName == null) continue;

                    //get image url for displaying
                    string ImageURL = "https://www.gamerpower.com" + html.DocumentNode.Descendants()
                        .Where(x => x.HasClass("card-img-top") && x.HasClass("thumbnail")).FirstOrDefault()?.Attributes["src"].Value;

                    //i think it can be optimized but for now its just works
                    //get URL for this item
                    var URlNode = html.DocumentNode.Descendants()
                        .Where((x) =>
                        x.HasClass("card")
                        && x.HasClass("box-shadow")
                        && x.HasClass("shadow")
                        && x.HasClass("grow")).ToList()[0];
                    html.LoadHtml(URlNode.InnerHtml);

                    var text = html.DocumentNode.Descendants(1).Where(x => x.HasClass("thumbnail")).ToList()[0].InnerHtml;
                    html.LoadHtml(text);

                    var URL = "https://www.gamerpower.com" + html.DocumentNode.FirstChild.Attributes["href"].Value;

                    ChannelsUtil.SendMessage(CreateMessage(GameName, ImageURL, URL),Channel);
                }
            }
        }

        private DiscordEmbedBuilder CreateMessage(string GameName, string ImageURL, string URL = "")
        {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.ImageUrl = ImageURL;
            embedBuilder.Title = GameName;
            embedBuilder.Url = URL;
            embedBuilder.Build();
            return embedBuilder;
        }
    }
}
