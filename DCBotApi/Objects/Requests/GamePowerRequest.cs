using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Objects.Requests
{
    internal class GamePowerRequest : IResponse<GiveAway[]>
    {
        public HttpStatusCode status { get; set; }

        public string? message { get; set; }

        public GiveAway[]? data { get; set; }
    }
}
