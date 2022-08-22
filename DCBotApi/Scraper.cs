using DSharpPlus;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCBotApi.Objects;
using System.Diagnostics;

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
                long scrapping = 0;
                long Updating = 0;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                var Acutaldata = BeginScraping();
                scrapping = timer.ElapsedMilliseconds;
                timer.Restart();
                UpdateMessages(Acutaldata);
                Updating = timer.ElapsedMilliseconds;
                timer.Stop();

                Console.WriteLine("Scrapping: " + scrapping + "ms" + "\n" +
                                  "Updating: " + Updating + "ms" + "\n");
            });
        }


        private void UpdateMessages(IEnumerable<GameObject> games)
        {
            var data = ChannelsUtil.GetMessages(Channel).Result;
            int changes = 0;

            foreach(var game in games)
            {
                if (!data.Any(x => x.Embeds.First().Title == game.Name))
                {
                    ChannelsUtil.SendMessage(CreateMessage(game), Channel);
                    changes++;
                }
            }

            foreach (var message in data)
            {
                if (!games.Any(x => x.Name == message.Embeds.First().Title))
                {
                    ChannelsUtil.RemoveMessage(message, Channel);
                    changes++;
                }
            }

            Console.WriteLine("\nchanges: " + changes + " ");
        }

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


            //remove all expired nodes
            List<string> Inner = new List<string>();
            foreach (HtmlNode x in nodes)
            {
                string c = x.InnerHtml;
                if (!c.Contains("expire_stamp")) Inner.Add(c);
            }

            //proccess all left nodes 
            foreach (string x in Inner)
            {
                var temp = ExtractData(x);
                if (temp.HasValue)
                    ScrappedData.Add(temp.Value);
            }
            return ScrappedData;
        }

        /// <summary>
        /// extract data from given node as string 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public GameObject? ExtractData(string node)
        {
            HtmlDocument html = new HtmlDocument();

            //load new html to proccesing
            html.LoadHtml(node);

            //get game name 
            string? GameName = html.DocumentNode.Descendants().FirstOrDefault(x => x.HasClass("card-title"))?.InnerText;

            if (GameName == null) return null;

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
