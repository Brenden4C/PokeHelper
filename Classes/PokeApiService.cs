using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

        public static async Task<PokemonInfo> GetRandomPokemonAsync()
        {
            Random rand = new Random();
            int randomPokemonId = rand.Next(1, 899); // Pokémon IDs range from 1 to 898

            string url = $"https://pokeapi.co/api/v2/pokemon/{randomPokemonId}/";
            string response = await client.GetStringAsync(url);

            var json = JObject.Parse(response);

            string name = json["name"].ToString();
            string spriteUrl = json["sprites"]["front_default"]?.ToString() ?? "";

            List<string> types = json["types"]
                .Select(t => t["type"]["name"].ToString())
                .ToList();

            return new PokemonInfo
            {
                Name = name,
                SpriteUrl = spriteUrl,
                Types = types
            };
        }

        // Will return the name of a move as well as the moves type.
        // Will only find and return damaging moves. (status moves don't work)
        public static async Task<(string moveName, string moveType)?> GetRandomDamagingMoveAsync()
        {
            Random rand = new Random();

            while (true)
            {
                int moveId = rand.Next(1, 919); // Pokémon move IDs

                string url = $"https://pokeapi.co/api/v2/move/{moveId}/";

                try
                {
                    string response = await client.GetStringAsync(url);
                    using JsonDocument doc = JsonDocument.Parse(response);

                    string name = doc.RootElement.GetProperty("name").GetString();
                    string type = doc.RootElement.GetProperty("type").GetProperty("name").GetString();
                    string damageClass = doc.RootElement.GetProperty("damage_class").GetProperty("name").GetString();

                    if (damageClass == "physical" || damageClass == "special")
                    {
                        return (name, type);
                    }
                }
                catch
                {
                    // If there's a network or parsing error, just loop again
                }
            }
        }

    }
}
