using System;
using System.Net;

namespace ApiWrapper
{
    public class ApiException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public string Content { get; set; }

        public ApiException(string content, HttpStatusCode statuscode) : base(content)
        {
            Code = statuscode;
            Content = content;
        }
    }
}
