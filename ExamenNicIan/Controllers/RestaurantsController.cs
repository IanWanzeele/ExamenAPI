using ExamenNicIan.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExamenNicIan.Controllers
{
    public class RestaurantsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(double latitude, double longitude)
        {
            // Make API call to retrieve restaurant data based on latitude and longitude
            var restaurants = await GetRestaurantsFromApi(latitude, longitude);

            // Pass the list of restaurants to the view
            return View(restaurants);
        }

        private async Task<List<Restaurant>> GetRestaurantsFromApi(double latitude, double longitude)
        {
            // Initialize the HttpClient
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Construct the URL for your API request
                    var apiUrl = $"https://overpass-api.de/api/interpreter?data=[out:json];node[\"amenity\"=\"restaurant\"](around:10000,{latitude},{longitude});out;";

                    // Make the GET request to the API
                    var response = await httpClient.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        var responseBody = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response into a list of Restaurant objects
                        var restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(responseBody);

                        // Return the list of restaurants
                        return restaurants;
                    }
                    else
                    {
                        // If the request was not successful, log or handle the error accordingly
                        // For example:
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during the API call
                    // For example:
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            // If an error occurred or the API call failed, return an empty list
            return new List<Restaurant>();
        }

    }
}
