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
        private readonly RapidApiSettings _apiSettings;
        private readonly ILogger<RapidApiDestinationDAO> _logger;

        public RapidApiDestinationDAO(HttpClient httpClient, RapidApiSettings apiSettings, ILogger<RapidApiDestinationDAO> logger)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
            _logger = logger;

            // Set up HTTP client headers
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiSettings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _apiSettings.Host);
        }

        public async Task<List<RapidApiDestination>> SearchRapidApiDestinationsByQueryAsync(string cityname)
        {
            try
            {
                // Encode the query parameter
                var encodedQuery = HttpUtility.UrlEncode(cityname);
                var requestUrl = $"{_apiSettings.BaseUrl}?query={encodedQuery}";

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