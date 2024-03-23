using ExamenNicIan.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using ExamenNicIan.Services;
using System.Net.Http;

namespace ExamenNicIan.Services
{
    public class RestaurantService
    {
        private readonly HttpClient _httpClient;

        public RestaurantService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Restaurant> GetRestaurantsFromApi(double latitude, double longitude)
        {
            // restaurants ophalen met de basic coordinaten in buurt van 10km

            {
                try
                {
                    var apiUrl =
                        $"https://overpass-api.de/api/interpreter?data=[out:json];node[\"amenity\"=\"restaurant\"](around:10000,{latitude.ToString().Replace(',', '.')},{longitude.ToString().Replace(',', '.')});out;";

                    
                    var response = await _httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                       
                        var responseBody = await response.Content.ReadAsStringAsync();

                      
                        var restaurant = JsonConvert.DeserializeObject<Restaurant>(responseBody);

                        return restaurant;
                    }
                    else
                    {
                    
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
           
            return null;
        }
        public async Task<Restaurant> GetRestaurantById(long restaurantId)
        {
            try
            {
                var apiUrl = $"https://overpass-api.de/api/interpreter?data=[out:json];node({restaurantId});out;";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var restaurant = JsonConvert.DeserializeObject<Restaurant>(responseBody);
                    return restaurant;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return null;
        }
    }
}

