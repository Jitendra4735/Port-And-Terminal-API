using System.Runtime.Serialization;

namespace WebApi.Utilities
{
    public class HttpClientException : Exception
    {
        /// <summary>
        /// Gets or sets the HTTP response message associated with the exception.
        /// </summary>
        public HttpResponseMessage? HttpResponseMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientException"/> class with serialized data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected HttpClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientException"/> class with an HTTP status code and an error message.
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <param name="message"></param>
        public HttpClientException(System.Net.HttpStatusCode httpStatusCode, string message)
        {
            // Create and assign an HttpResponseMessage with the specified status code and message
            this.HttpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(message, System.Text.Encoding.UTF8, "application/json")
            };
        }


    }
}
