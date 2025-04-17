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
    public partial class QuizPage : Page
    {


        private static readonly HttpClient HttpClient = new HttpClient();
        private double correctEffectiveness;  // Will store the correct effectiveness value
        private string[] defenseTypes = Array.Empty<string>();
        private string attackType;


        public QuizPage()
        {
            InitializeComponent();
            StartNewRound();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            else if (defenseTypes.Length == 2)
            {
                // Call the method for two types (dual-type)
                correctEffectiveness = TypeChart.GetDualTypeEffectiveness(attackType, defenseTypes[0], defenseTypes[1]);
            }
            else
            {
                MessageBox.Show("No defensive types?");
            }
        }

        private async void GetRandomPokemonButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewRound();
        }

        private async void StartNewRound()
        {
            await GenerateRandomPokemon();
            await GenerateRandomMove();
            UpdateCorrectEffectiveness();
        }

        private async Task GenerateRandomPokemon()
        {
            try
            {
                var pokemon = await PokeApiService.GetRandomPokemonAsync();

                // Display sprite
                PokemonSpriteImage.Source = new BitmapImage(new Uri(pokemon.SpriteUrl));

                // Display name (capitalize first letter)
                PokemonNameTextBlock.Text = char.ToUpper(pokemon.Name[0]) + pokemon.Name.Substring(1);

                // Display types (as a comma-separated string)
                PokemonTypesTextBlock.Text = string.Join(", ", pokemon.Types);

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

                SetMoveTypeStyle(moveType);

                MoveNameTextBlock.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(moveName);
                MoveTypeTextBlock.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(moveType);

                // Set PP 15/15 (we can set a static value here for now, but you can adjust this to reflect actual move data)
                PPTextBlock.Text = "15/15";

                attackType = moveType;
            }
            else
            {
                MessageBox.Show("Failed to get a valid damaging move.");
            }
        }

        public void DisplayTypeImages(string typeString)
        {
            // Clear any existing images
            //TypeImagePanel.Children.Clear();

            // Split types by comma
            string[] types = typeString.Split(',');

            foreach (string type in types)
            {
                Image typeImage = ImageProcessing.GetTypeImage(type);
                //TypeImagePanel.Children.Add(typeImage);
            }
        }

        private void SetMoveTypeStyle(string moveType)
        {
            Color gradientTop = Colors.LightGray;
            Color gradientBottom = Colors.Gray;
            Brush textColor = Brushes.Black;

            switch (moveType.ToLower())
            {
                case "normal":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#D3D3D3");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#A9A9A9");
                    textColor = Brushes.Black;
                    break;
                case "fire":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#FF7F50");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#8B0000");
                    textColor = Brushes.White;
                    break;
                case "water":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#00BFFF");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#1E90FF");
                    textColor = Brushes.White;
                    break;
                case "electric":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#FFF700");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#E6BE00");
                    textColor = Brushes.Black;
                    break;
                case "grass":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#7CFC00");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#228B22");
                    textColor = Brushes.White;
                    break;
                case "ice":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#B0E0E6");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#5F9EA0");
                    textColor = Brushes.Black;
                    break;
                case "fighting":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#CD5C5C");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#8B0000");
                    textColor = Brushes.White;
                    break;
                case "poison":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#BA55D3");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#800080");
                    textColor = Brushes.White;
                    break;
                case "ground":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#DEB887");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#8B4513");
                    textColor = Brushes.Black;
                    break;
                case "flying":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#ADD8E6");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#87CEEB");
                    textColor = Brushes.Black;
                    break;
                case "psychic":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#FF69B4");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#C71585");
                    textColor = Brushes.White;
                    break;
                case "bug":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#9ACD32");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#6B8E23");
                    textColor = Brushes.Black;
                    break;
                case "rock":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#A0522D");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#5C4033");
                    textColor = Brushes.White;
                    break;
                case "ghost":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#9370DB");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#4B0082");
                    textColor = Brushes.White;
                    break;
                case "dragon":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#4169E1");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#00008B");
                    textColor = Brushes.White;
                    break;
                case "dark":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#2F4F4F");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#000000");
                    textColor = Brushes.White;
                    break;
                case "steel":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#C0C0C0");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#808080");
                    textColor = Brushes.Black;
                    break;
                case "fairy":
                    gradientTop = (Color)ColorConverter.ConvertFromString("#FFB6C1");
                    gradientBottom = (Color)ColorConverter.ConvertFromString("#DB7093");
                    textColor = Brushes.Black;
                    break;
                default:
                    // fallback for unknown types
                    gradientTop = Colors.LightGray;
                    gradientBottom = Colors.Gray;
                    textColor = Brushes.Black;
                    break;
            }

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = new GradientStopCollection
        {
            new GradientStop(gradientTop, 0),
            new GradientStop(gradientBottom, 1)
        }
            };

            MoveInfoBorder.Background = gradientBrush;
            MoveNameTextBlock.Foreground = textColor;
            MoveTypeTextBlock.Foreground = textColor;
        }
    }
}
