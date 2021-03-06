﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpLogger
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, Task> _requestAction;
        private readonly Func<HttpResponseMessage, Task> _responseAction;

        public HttpLoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _responseAction = DefaultResponseAction;
            _requestAction = DefaultRequestAction;
        }

        public HttpLoggingHandler(HttpMessageHandler innerHandler,
            Func<HttpRequestMessage, Task> requestAction,
            Func<HttpResponseMessage, Task> responseAction)
            : base(innerHandler)
        {
            _responseAction = responseAction;
            _requestAction = requestAction;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_requestAction != null)
                await _requestAction(request);
            var result = await base.SendAsync(request, cancellationToken);
            if (_responseAction != null)
                await _responseAction(result);
            return result;
        }

        private async Task DefaultRequestAction(HttpRequestMessage request)
        {
            Debug.WriteLine("Request:");
            Debug.WriteLine(request.ToString());
            if (request.Content != null)
                Debug.WriteLine(await request.Content.ReadAsStringAsync());
            Debug.WriteLine("");
        }

        private async Task DefaultResponseAction(HttpResponseMessage response)
        {
            Debug.WriteLine("Response:");
            Debug.WriteLine(response.ToString());
            if (response.Content != null)
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
            Debug.WriteLine("");
        }

        public static string GetRequestLogString(HttpRequestMessage request)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Method: {request.Method}");
            stringBuilder.Append(", RequestUri: '");
            stringBuilder.Append(request.RequestUri == null ? null : request.RequestUri.ToString());
            stringBuilder.Append($"', Version: {request.Version}");
            stringBuilder.Append(", Content: ");
            stringBuilder.Append(request.Content == null ? null : request.Content.GetType().ToString());
            stringBuilder.Append($", Headers:\r\n {request.Headers.ToString()}");
            return stringBuilder.ToString();
        }
    }
}