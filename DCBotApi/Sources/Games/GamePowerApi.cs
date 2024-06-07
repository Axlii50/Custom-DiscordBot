using DCBotApi.Objects;
using DCBotApi.Objects.Requests;
using DCBotApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Sources.Games
{
    internal class GamerPowerApi
    {
        const string BaseUrl = "https://www.gamerpower.com/api/";

        public GiveAways GiveAways { get; set; }

        public GamerPowerApi() { }


        public async Task GetGiveAways()
        {
            var response = await HttpService.Send<GamePowerRequest, GiveAway[]>(BaseUrl + "giveaways?type=game");

            if (response.status != System.Net.HttpStatusCode.OK) return;

            GiveAways = new GiveAways() { GiveAwaysCollection = response.data };
        }
    }
}
