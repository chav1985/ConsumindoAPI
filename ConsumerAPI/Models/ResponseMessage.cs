using System.Net;

namespace ConsumerAPI.Models
{
    public class ResponseMessage
    {
        public HttpStatusCode StatusId { get; set; }
        public string Content { get; set; }
    }
}
