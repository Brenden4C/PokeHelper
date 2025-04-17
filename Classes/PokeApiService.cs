using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PokeHelper.Classes
{
    class PokeApiService {

        private static readonly HttpClient client = new HttpClient();


        //Returns a given Pokemon's Type(s)
        public static async Task<List<string>> GetPokemonTypesAsync(string pokemonName)
        {
            var types = new List<string>();

            string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonString);

                var typeArray = json["types"];
                foreach (var type in typeArray)
                {
                    types.Add(type["type"]["name"].ToString());
                }
            }

            return types;
        }

        //Returns a random Pokemon as a string
        public static async Task<string> GenerateRandomPokemon()
        {
            // Define the base URL for fetching Pokémon data
            string url = "https://pokeapi.co/api/v2/pokemon?limit=1000"; // Limits to 1000 Pokémon

            // Make a request to the PokéAPI to fetch a list of Pokémon
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            // Deserialize the response into a list of Pokémon
            var json = JObject.Parse(response);
            var results = json["results"].ToObject<List<JObject>>();

            // Generate a random index to select a Pokémon from the list
            Random random = new Random();
            int randomIndex = random.Next(results.Count);

            // Return the name of the randomly selected Pokémon
            return results[randomIndex]["name"].ToString();
        }
    }
}
