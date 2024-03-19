﻿using ExamenNicIan.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using ExamenNicIan.Services;

namespace ExamenNicIan.Services
{
    public class RestaurantService
    {
        public async Task<Restaurant> GetRestaurantsFromApi(double latitude, double longitude)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Construct the URL for your API request
                    var apiUrl =
                        $"https://overpass-api.de/api/interpreter?data=[out:json];node[\"amenity\"=\"restaurant\"](around:10000,{latitude.ToString().Replace(',', '.')},{longitude.ToString().Replace(',', '.')});out;";

                    // Make the GET request to the API
                    var response = await httpClient.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        var responseBody = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response into a single Restaurant object
                        var restaurant = JsonConvert.DeserializeObject<Restaurant>(responseBody);

                        // Add the single restaurant to the list
                        return restaurant;
                    }
                    else
                    {
                        // If the request was not successful, log or handle the error accordingly
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during the API call
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            // If an error occurred or the API call failed, return an empty list
            return null;
        }
    }
}
