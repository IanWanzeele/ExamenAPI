using System;
using System.Net.Http;
using System.Threading.Tasks;
using ExamenNicIan.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
namespace ExamenNicIan.Services
{
    public class GeoCodingService
    {
        private readonly HttpClient _httpClient;

        public GeoCodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double latitude, double longitude)> GeoLocation(string userInput)
        {
            // Construct the URL for the geocoding API request

            string url = $"https://atlas.microsoft.com/search/address/json?query={userInput}&api-version=1.0&subscription-key=lG1hIRQvycC8mByD5fl2Rx-fJdneXcDeUTCfOa9aKnc";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return (0, 0);
            }

            var content = await response.Content.ReadAsStringAsync();
            var geoLocationResponse = JsonConvert.DeserializeObject<GeoLocationResponse>(content);

            if (geoLocationResponse?.Results != null && geoLocationResponse.Results.Count > 0)
            {
                var result = geoLocationResponse.Results[0];
                return (result.Position.Lat, result.Position.Lon);
            }

            return (0, 0);

        }


    }
}
