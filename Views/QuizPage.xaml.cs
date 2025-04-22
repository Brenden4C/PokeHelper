using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
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
        private string attackType = "";



        public QuizPage()
        {
            InitializeComponent();
            StartNewRound();

            MusicSlider.ValueChanged += OnMusicValueChanged;
            SfxSlider.ValueChanged += OnSfxValueChanged;
        }

        private void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReturnHomeButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Implement returning to home screen.
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = MenuPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
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
                    //MessageBox.Show("Incorrect, try again!");
                    Shake();
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

        private async Task GetRandomPokemonButton_Click(object sender, RoutedEventArgs e)
        {
            await StartNewRound();
        }

        private async Task StartNewRound()
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
                PokemonNameTextBlock.Text = "Defending Pokemon: " + char.ToUpper(pokemon.Name[0]) + pokemon.Name.Substring(1);
                
                // Store types in private field
                defenseTypes = pokemon.Types.ToArray();

                // Display types (as a comma-separated string)
                if(defenseTypes.Length == 1)
                    SetDefenderTypeStyle(defenseTypes[0]);
                else
                    SetDefenderTypeStyle(defenseTypes[0], defenseTypes[1]);

                

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
                //Image typeImage = ImageProcessing.GetTypeImage(type);
                //TypeImagePanel.Children.Add(typeImage);
            }
        }

        private void SetDefenderTypeStyle(string type1, string type2)
        {
            // Get gradient + text color for first type
            GetTypeColors(type1, out Color gradientTop1, out Color gradientBottom1, out Brush textColor1);
            // Get gradient + text color for second type
            GetTypeColors(type2, out Color gradientTop2, out Color gradientBottom2, out Brush textColor2);

            // Use gradient from first type to second type (just top to top, bottom to bottom works fine)
            DefenderGradient.GradientStops[0].Color = gradientTop1;
            DefenderGradient.GradientStops[1].Color = gradientTop2;

            // Choose the more contrasting text color (or default to white if unsure)
            DefenderTypesTextBlock.Foreground = textColor1 == Brushes.White || textColor2 == Brushes.White
                ? Brushes.White
                : textColor1;

            // Set the text content
            DefenderTypesTextBlock.Text = "Types: " + $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(type1.ToLowerInvariant())} / {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(type2.ToLowerInvariant())}";
        }

        private void SetDefenderTypeStyle(string type)
        {
            GetTypeColors(type, out Color gradientTop, out Color gradientBottom, out Brush textColor);

            // Use the same color for both gradient stops to create a solid background
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

            DefenderInfoBox.Background = gradientBrush;
            DefenderTypesTextBlock.Text = "Type: " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(type.ToLowerInvariant());
            DefenderTypesTextBlock.Foreground = textColor;
        }

        private async void Shake()
        {
            var transform = ShakeTransform;
            int shakeCount = 10;
            double amplitude = 5;

            for (int i = 0; i < shakeCount; i++)
            {
                double offset = (i % 2 == 0 ? amplitude : -amplitude);
                transform.X = offset;
                await Task.Delay(30);
            }

            transform.X = 0; // Reset position
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

        private void GetTypeColors(string type, out Color gradientTop, out Color gradientBottom, out Brush textColor)
        {
            gradientTop = Colors.LightGray;
            gradientBottom = Colors.Gray;
            textColor = Brushes.Black;

            switch (type.ToLower())
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
            }
        }

        /*
         * =========================================================
         *  Methods for Custom Volume Sliders
         * =========================================================
         */

        private void OnMusicValueChanged( double newValue )
        {
            MusicVolumeText.Text = "Music Volume: " + (int)newValue + "/100";
        }

        private void OnSfxValueChanged( double newValue )
        {
            SfxVolumeText.Text = "SFX Volume: " + (int)newValue + "/100";
        }


        






    }
}
