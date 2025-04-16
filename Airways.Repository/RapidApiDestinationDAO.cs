using Airways.Domain;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Airways.Repository
{
    public class RapidApiDestinationDAO : IRapidApiDestinationDAO
    {
        private readonly HttpClient _httpClient;
        private readonly RapidApiDestinationSettings _apiDestinationSettings;
        private readonly ILogger<RapidApiDestinationDAO> _logger;

        public RapidApiDestinationDAO(HttpClient httpClient, RapidApiDestinationSettings apiDestinationSettings, ILogger<RapidApiDestinationDAO> logger)
        {
            _httpClient = httpClient;
            _apiDestinationSettings = apiDestinationSettings;
            _logger = logger;

            // Set up HTTP client headers
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiDestinationSettings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _apiDestinationSettings.Host);
        }

        public async Task<List<RapidApiDestination>> SearchRapidApiDestinationsByQueryAsync(string cityname)
        {
            try
            {
                // Encode the query parameter
                var encodedQuery = HttpUtility.UrlEncode(cityname);
                var requestUrl = $"{_apiDestinationSettings.BaseUrl}?query={encodedQuery}";

                _logger.LogInformation($"Sending request to: {requestUrl}");

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Received response: {jsonResponse}");

                var rapidApiDestinationResponse = JsonSerializer.Deserialize<RapidApiDestinationResponse>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return rapidApiDestinationResponse?.RapidApiDestinations ?? new List<RapidApiDestination>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching destinations for query: {cityname}");
                return new List<RapidApiDestination>();
            }
        }
    }
}