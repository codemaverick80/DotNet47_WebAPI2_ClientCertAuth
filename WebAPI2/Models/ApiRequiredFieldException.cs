using System.Net;

namespace WebAPI2.Models
{
    public class ApiRequiredFieldException
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }
}