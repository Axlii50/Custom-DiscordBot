using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DCBotApi.Objects
{
    internal class GiveAway
    {
        /// <summary>
        /// The unique identifier for the giveaway.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the giveaway.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The worth or GiveAwaysCollection of the giveaway.
        /// </summary>
        public string Worth { get; set; }

        /// <summary>
        /// The URL to the thumbnail image of the giveaway.
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// The URL to the main image of the giveaway.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// The description of the giveaway.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The instructions on how to participate in the giveaway.
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// The URL to open the giveaway page.
        /// </summary>
        [JsonProperty("open_giveaway_url")]
        public string OpenGiveawayUrl { get; set; }

        /// <summary>
        /// The date and time when the giveaway was published.
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// The type of the giveaway.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The platforms on which the giveaway is available.
        /// </summary>
        public string Platforms { get; set; }

        private PlatformType? _availablePlatforms { get; set; } = null;

        public PlatformType GetPlatforms { 
            get
            {
                if (_availablePlatforms != null) return _availablePlatforms.Value;

                PlatformType types = 0;
                var platfmors = Platforms.Split(',');

                foreach (var platform in platfmors)
                {
                    types |= platform.Trim() switch
                    {
                        "PC" => PlatformType.PC,
                        "Epic Games Store" => PlatformType.EPIC,
                        "Steam" => PlatformType.STEAM,
                        "xxx" => PlatformType.XBOXONE, //temporarly not used
                    };
                }

                _availablePlatforms = types;
                return types;
            }
        }


        /// <summary>
        /// The date and time when the giveaway ends.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The number of users participating in the giveaway.
        /// </summary>
        public int Users { get; set; }

        /// <summary>
        /// The current status of the giveaway.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The URL to the GamerPower page for the giveaway.
        /// </summary>
        [JsonProperty("gamerpower_url")]
        public string GamerpowerUrl { get; set; }
    }
}
