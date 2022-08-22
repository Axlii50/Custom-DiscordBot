using DSharpPlus;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCBotApi.Objects;

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
                Console.WriteLine("Scrapping");
                var Acutaldata = BeginScraping();
                Console.WriteLine("Updating");
                UpdateMessages(Acutaldata);
            });
        }


        private void UpdateMessages(IEnumerable<GameObject> games)
        {
            var data = ChannelsUtil.GetMessages(Channel).Result;

            foreach(var game in games)
            {
                Console.WriteLine(game.Name);
                if (!data.Any(x => x.Embeds.First().Title == game.Name))
                    ChannelsUtil.SendMessage(CreateMessage(game),Channel);
            }

            //foreach(var message in data)
            //{
            //    if (!games.Any(x => x.Name == message.Embeds.First().Title))
            //        ChannelsUtil.RemoveMessage(message,Channel);
            //}


        }

        /// <summary>
        /// separet for functions
        /// </summary>
        public List<GameObject> BeginScraping()
        {
            List<GameObject> ScrappedData = new List<GameObject>();
            HttpClient client = new HttpClient();
            HtmlDocument html = new HtmlDocument();
            List<HtmlNode> validNodes = new List<HtmlNode>();

            var response = client.GetStringAsync("https://www.gamerpower.com/all/free-games").Result;

            html.LoadHtml(response);

            //get all containers with informations
            List<HtmlNode> nodes =
                  html.DocumentNode.Descendants(0)
                     .Where(n => n.HasClass("col-xl-3")
                              && n.HasClass("col-lg-4")
                              && n.HasClass("col-md-6")
                              && n.HasClass("col-sm-6")
                              && n.HasClass("mb-4")).ToList();

            foreach (HtmlNode x in nodes)
            {
               _ = x.InnerHtml;
                //Console.WriteLine(x.InnerHtml);
                //Console.WriteLine();
            }

            foreach (HtmlNode x in nodes)
            {
                html.LoadHtml(x.InnerHtml);

                //if containers is not expired
                if (!html.DocumentNode.Descendants().Any(x => x.HasClass("expire_stamp")))
                {
                    var objet = ExtractData(x);

                }
            }
            return ScrappedData;
        }

        public GameObject ExtractData(HtmlNode node)
        {
            HtmlDocument html = new HtmlDocument();

            //load new html to proccesing
            html.LoadHtml(node.InnerHtml);

            //get game name 
            string? GameName = html.DocumentNode.Descendants().FirstOrDefault(x => x.HasClass("card-title"))?.InnerText;
            Console.WriteLine(GameName);
            //if (GameName == null) continue;

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


            Console.WriteLine(GameName);
            GameObject _game = new()
            {
                Name = GameName,
                ImageURL = ImageURL,
                RedirectURL = URL,
            };
            return _game;

        }

        private DiscordEmbedBuilder CreateMessage(GameObject game)
        {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.ImageUrl = game.ImageURL;
            embedBuilder.Title = game.Name;
            embedBuilder.Url = game.RedirectURL;
            embedBuilder.Build();
            return embedBuilder;
        }
    }
}
