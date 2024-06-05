using System.Net;

namespace DCBotApi
{
    public interface IResponse<ResponseData>
    {
        HttpStatusCode status { get; set; }

        string? message { get; set; }

        ResponseData? data { get; set; }
    }
}
