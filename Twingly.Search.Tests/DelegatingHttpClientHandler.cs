using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Twingly.Search.Tests
{
    public class DelegatingHttpClientHandler : HttpClientHandler
    {
        private readonly Action<HttpRequestMessage> requestCallback = null;
        private readonly HttpResponseMessage response = null;

        public DelegatingHttpClientHandler(Action<HttpRequestMessage> requestCallback, HttpResponseMessage response)
        {
            if (requestCallback == null) throw new ArgumentNullException(nameof(requestCallback));
            if (response == null) throw new ArgumentNullException(nameof(response));

            this.requestCallback = requestCallback;
            this.response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            requestCallback(request);

            return Task.Factory.StartNew<HttpResponseMessage>(() => response, cancellationToken);
        }

        /// <summary>
        /// Helper method to easily return a simple HttpResponseMessage.
        /// </summary>
        public static HttpResponseMessage GetStreamHttpResponseMessage(string content,
            HttpStatusCode httpStatusCode = HttpStatusCode.OK,
            string mediaType = "application/json")
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(contentBytes);

            return new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StreamContent(stream)
            };
        }
    }
}
