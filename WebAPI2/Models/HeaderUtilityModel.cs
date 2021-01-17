using System;
using System.Linq;
using System.Net.Http.Headers;

namespace WebAPI2.Models
{
    public class HeaderUtilityModel
    {
        public static HeaderModel GetHeaders(HttpRequestHeaders headers)
        {
            HeaderModel header = new HeaderModel();

            foreach (var item in headers)
            {
                if (item.Key.StartsWith("API-"))
                {
                    switch (item.Key)
                    {
                        case "API-SenderMessageId":
                            header.SenderMessageId = item.Value.FirstOrDefault();
                            break;
                        case "API-SenderApplicationId":
                            header.SenderApplicationId = item.Value.FirstOrDefault();
                            break;
                        case "API-SenderHostName":
                            header.SenderHostName = item.Value.FirstOrDefault();
                            break;
                        case "API-CreationTimeStamp":
                            DateTime dt;
                            if(DateTime.TryParse(item.Value.FirstOrDefault(),out dt))
                            header.CreationTimeStamp = dt;
                            break;
                        case "API-SenderMessageIdEcho":
                            header.SenderMessageIdEcho = item.Value.FirstOrDefault();
                            break;
                        case "API-OriginationApplicationId":
                            header.OriginationApplicationId = item.Value.FirstOrDefault();
                            break;
                        case "API-TransactionId":
                            header.TransactionId = item.Value.FirstOrDefault();
                            break;
                    }
                }

            }
            return header;
        }

    }
}