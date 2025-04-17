using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Media.Animation;
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
        private double correctEffectiveness;  // Will store the correct effectiveness value
        private string[] defenseTypes = Array.Empty<string>();
        private string attackType;


        public QuizPage() {
            InitializeComponent();
        }

        private void EffectivenessButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Get the user’s selected effectiveness (you can get this from the button text or value)
                button = sender as Button;
                double selectedEffectiveness = double.Parse(button.Content.ToString().Replace("x", ""));

                // Check if the selected effectiveness matches the correct one
                if (selectedEffectiveness == correctEffectiveness)
                {
                    MessageBox.Show("Correct!");
                    // You can add more logic here, like updating the score or proceeding to the next question
                    StartNewRound();
                }
                else
                {
                    MessageBox.Show("Incorrect, try again!");
                }
            }
        }

        private void UpdateCorrectEffectiveness()
        {
            // Check if the target has one or two types
            if (defenseTypes.Length == 1)
            {
                // Call the method for one type (mono-type)
                correctEffectiveness = TypeChart.GetMonoTypeEffectiveness(attackType, defenseTypes[0]);
            }
            else if(defenseTypes.Length == 2)
            {
                // Call the method for two types (dual-type)
                correctEffectiveness = TypeChart.GetDualTypeEffectiveness(attackType, defenseTypes[0], defenseTypes[1]);
            }
            else
            {
                MessageBox.Show("No defensive types?");
            }
        }

        private async void GetRandomPokemonButton_Click(object sender, RoutedEventArgs e) {
            StartNewRound();
        }

        private async void StartNewRound()
        {
            await GenerateRandomPokemon();
            await GenerateRandomMove();
            UpdateCorrectEffectiveness();
        }

        private async Task GenerateRandomPokemon() {
            try
            {
                var pokemon = await PokeApiService.GetRandomPokemonAsync();

                // Display sprite
                PokemonSpriteImage.Source = new BitmapImage(new Uri(pokemon.SpriteUrl));

                // Display name (capitalize first letter)
                PokemonNameTextBlock.Text = char.ToUpper(pokemon.Name[0]) + pokemon.Name.Substring(1);

                // Display types (as a comma-separated string)
                DisplayTypeImages(string.Join(", ", pokemon.Types));

                // Store types in private field
                defenseTypes = pokemon.Types.ToArray();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Pokémon: " + ex.Message);
            }
        }

        private async Task GenerateRandomMove()
        {
            var move = await PokeApiService.GetRandomDamagingMoveAsync();

            if (move != null)
            {
                var (moveName, moveType) = move.Value;

                MoveNameTextBlock.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(moveName);
                attackType = moveType;
                // Display the Image for the Move's Type
                MessageBox.Show("" + moveType);
                MoveTypeImage.Source = ImageProcessing.GetTypeImageWithImageSource(moveType); // Using ImageSource version

            }
            else
            {
                MessageBox.Show("Failed to get a valid damaging move.");
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

    }
}
