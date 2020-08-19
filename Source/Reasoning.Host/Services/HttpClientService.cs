using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Abstraction;
using Microsoft.Extensions.Logging;

using Reasoning.Core.Contracts;

namespace Reasoning.Host.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientService> _logger;
        private readonly Dictionary<ReasoningRequestMethod, HttpMethod> _methodDictionary = new Dictionary<ReasoningRequestMethod, HttpMethod>
        {
            { ReasoningRequestMethod.GET, HttpMethod.Get },
            { ReasoningRequestMethod.POST, HttpMethod.Post },
            { ReasoningRequestMethod.PUT, HttpMethod.Put },
            { ReasoningRequestMethod.DELETE, HttpMethod.Delete }
        };
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(), new JsonAbstractionConverter() }
        };

        public HttpClientService(HttpClient httpClient, ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<T> ExecuteTaskAsync<T>(IReasoningRequest reasoningRequest)
        {
            try
            {
                var request = new HttpRequestMessage(_methodDictionary[reasoningRequest.Method], reasoningRequest.Uri);

                reasoningRequest.Headers?.ToList().ForEach(header =>
                {
                    request.Headers.Add(header.Key, header.Value);
                });

                _logger.LogInformation($"Reasoning request: Attempting HTTP request to {reasoningRequest.Uri}");

                var result = await _httpClient.SendAsync(request);

                _logger.LogInformation($"Reasoning request: Response code: {result.StatusCode}");

                if (!result.IsSuccessStatusCode) return default;

                var content = await result.Content.ReadFromJsonAsync<T>(_serializerOptions);

                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Reasoning request: An error occured");

                return default;
            }
        }

        public async Task ExecuteTaskAsync(IReasoningRequest reasoningRequest)
        {
            try
            {
                var request = new HttpRequestMessage(_methodDictionary[reasoningRequest.Method], reasoningRequest.Uri);

                reasoningRequest.Headers?.ToList().ForEach(header =>
                {
                    request.Headers.Add(header.Key, header.Value);
                });

                _logger.LogInformation($"Reasoning request: Attempting HTTP request to {reasoningRequest.Method} {reasoningRequest.Uri}");

                await _httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Reasoning request: An error occured");
            }
        }
    }
}
