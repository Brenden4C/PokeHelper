using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using PokeHelper.Classes;


namespace PokeHelper.Views
{
    /// <summary>
    /// Interaction logic for QuizPage.xaml
    /// </summary>
    public partial class QuizPage : Page {


        private static readonly HttpClient HttpClient = new HttpClient();


        public QuizPage() {
            InitializeComponent();
        }

        private void EffectivenessButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string effectiveness = button.Content.ToString();
                MessageBox.Show($"You selected: {effectiveness}");

                // Later you'll compare this to the actual effectiveness and give feedback
            }
        }

        private async void GetRandomPokemonButton_Click(object sender, RoutedEventArgs e) {
            GenerateRandomPokemon();
        }

        private async void GenerateRandomPokemon() {
            try {
                Random rand = new Random();
                int randomPokemonId = rand.Next(1, 899); // Pokémon IDs range from 1 to 898

                string url = $"https://pokeapi.co/api/v2/pokemon/{randomPokemonId}/";
                string response = await HttpClient.GetStringAsync(url);

                var pokemon = JsonConvert.DeserializeObject<PokemonResponse>(response);

                // Display sprite
                PokemonSpriteImage.Source = new BitmapImage(new Uri(pokemon.sprites.front_default));

                // Display name (capitalize first letter)
                PokemonNameTextBlock.Text = char.ToUpper(pokemon.name[0]) + pokemon.name.Substring(1);

                // Display types
                //PokemonTypesTextBlock.Text = "Types: " + string.Join(", ", pokemon.types.Select(t => t.type.name));
                DisplayTypeImages(string.Join(", ", pokemon.types.Select(t => t.type.name)));
            }
            catch (Exception ex) {
                MessageBox.Show("Failed to load Pokémon: " + ex.Message);
            }
        }

        public void DisplayTypeImages(string typeString)
        {
            // Clear any existing images
            TypeImagePanel.Children.Clear();

            // Split types by comma
            string[] types = typeString.Split(',');

            foreach (string type in types)
            {
                Image typeImage = ImageProcessing.GetTypeImage(type);
                TypeImagePanel.Children.Add(typeImage);
            }
        }


        public class PokemonResponse {
            public string name { get; set; }
            public List<PokemonType> types { get; set; }
            public PokemonSprites sprites { get; set; }
        }

        public class PokemonType {
            public PokemonTypeDetails type { get; set; }
        }

        public class PokemonTypeDetails {
            public string name { get; set; }
        }

        public class PokemonSprites {
            public string front_default { get; set; }
        }


    }
}
