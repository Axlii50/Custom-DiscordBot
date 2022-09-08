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

namespace DCBotApi.Sources.Games
{
    internal class GamerPowerScraper
    {
        public List<GameObject> ExtractedData;

        public GamerPowerScraper()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            BeginScraping();
            timer.Stop();
            Console.WriteLine("Scrapping: " + timer.Elapsed + "ms");
        }

        public void BeginScraping()
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
            ExtractedData = ScrappedData;
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

            //set all platforms for given node
            if (node.Contains("Steam")) _game.type |= PlatformType.STEAM;
            if (node.Contains("Windows PC")) _game.type |= PlatformType.PC;
            if (node.Contains("Xbox Store")) _game.type |= PlatformType.XBOXONE;

            //probabilit that not always will detect in node
            //beacause there is no tag on page so only option (for now that i can think of) is just checking if in description appears "Epic games"
            if (node.Contains("Epic Games")) _game.type |= PlatformType.EPIC;

            return _game;
        }
    }
}
