using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PokeHelper.Classes
{
    public class TypeChart
    {
        public static Dictionary<string, Dictionary<string, double>> typeChart = new Dictionary<string, Dictionary<string, double>>
    {
            // Normal is not effective against Rock and Steel
            // Normal cannot hit Ghost
        { "Normal", new Dictionary<string, double> {

                { "Rock", 0.5 }, { "Steel", 0.5 }, 
                { "Ghost", 0 }

            }
        },
            // Fire is not effective against Fire, Water, Rock, and Dragon
            // Fire is super effective against Grass, Ice, Bug, and Steel
        { "Fire", new Dictionary<string, double> {

                { "Fire", 0.5 }, { "Water", 0.5 }, { "Rock", 0.5 }, { "Dragon", 0.5 },
                { "Grass", 2 }, { "Ice", 2 }, { "Bug", 2 }, { "Steel", 2 }

            }
        },
            // Water is not effective against Water, Grass, and Dragon
            // Water is super effective against Fire, Ground, and Rock
        { "Water", new Dictionary<string, double> {

                { "Water", 0.5 }, { "Grass", 0.5 }, { "Dragon", 0.5 },
                { "Fire", 2 }, { "Ground", 2 }, { "Rock", 2 }

            }
        },
            // Grass is not effective against Fire, Grass, Poison, Flying, Bug, Dragon, and Steel
            // Grass is super effective against Water, Ground, and Rock
        { "Grass", new Dictionary<string, double> {

                { "Fire", 0.5 }, { "Grass", 0.5 }, { "Poison", 0.5 }, { "Flying", 0.5 }, { "Bug", 0.5 }, { "Dragon", 0.5 }, { "Steel", 0.5 },
                { "Water", 2 }, { "Ground", 2 }, { "Rock", 2 }

            }
        },
            // Electric is not effective against Grass, Electric, and Dragon
            // Electric is super effective against Water, and Flying
            // Electric cannot hit Ground
        { "Electric", new Dictionary<string, double> {

                {"Grass", 0.5 }, { "Electric", 0.5 }, { "Dragon", 0.5 },
                { "Water", 2 }, { "Flying", 2 },
                { "Ground", 0 }

            }
        },
            // Ice is not effective against Fire, Water, Ice, and Steel
            // Ice is super effective against Grass, Ground, Flying, and Dragon
        { "Ice", new Dictionary<string, double> {

                { "Fire", 0.5 }, { "Water", 0.5 }, { "Ice", 0.5 }, { "Steel", 0.5 },
                { "Grass", 2 }, { "Ground", 2 }, { "Flying", 2 }, { "Dragon", 2 }

            }
        },
            // Fighting is not effective agaisnt Poison, Flying, Psychic, Bug, and Fairy
            // Fighting is super effective against Normal, Ice, Rock, Dark, and Steel
            // Fighting cannot hit Ghost
        { "Fighting", new Dictionary<string, double> {

                { "Poison", 0.5 }, { "Flying", 0.5 }, { "Psychic", 0.5 }, { "Bug", 0.5 }, { "Fairy", 0.5 },
                { "Normal", 2 }, { "Ice", 2 }, { "Rock", 2 }, { "Dark", 2 }, { "Steel", 2 },
                { "Ghost", 0 }

            }
        },
            // Poison is not effective against Poison, Ground, Rock, Ghost
            // Poison is super effective against Grass, and Fairy
            // Poison cannot hit Steel
        { "Poison", new Dictionary<string, double> {

                { "Poison", 0.5 }, { "Ground", 0.5 }, {"Rock", 0.5 }, { "Ghost", 0.5 },
                { "Grass", 2 }, { "Fairy", 2 },
                { "Steel", 0 }

            }
        },
            // Ground is not effective against Grass, and Bug
            // Ground is super effective against Fire, Electric, Poison, Rock, and Steel
            // Ground cannot hit Flying
        { "Ground", new Dictionary<string, double> {

                { "Grass", 0.5 }, { "Bug", 0.5 },
                { "Fire", 2 }, { "Electric", 2 }, { "Poison", 2 }, { "Rock", 2 }, { "Steel", 2 },
                { "Flying", 0 }

            }
        },
            // Flying is not effective against Electric, Rock, and Steel
            // Flying is super effective against Grass, Fighting, and Bug
        { "Flying", new Dictionary<string, double> {

                { "Electric", 0.5 }, { "Rock", 0.5 }, { "Steel", 0.5 },
                { "Grass", 2 }, { "Fighting", 2 }, { "Bug", 2 }

            }
        },
            // Psychic is not effective against Psychic, and Steel
            // Psychic is super effective against Fighting, and Poison
            // Psychic cannot hit Dark
        { "Psychic", new Dictionary<string, double> {

                { "Psychic", 0.5 }, { "Steel", 0.5 },
                { "Fighting", 2 }, { "Poison", 2 },
                { "Dark", 0 },

            }
        },
            // Bug is not effective against Fire, Fighting, Poison, Flying, Ghost, and Fairy
            // Bug is super effective against Grass, Psychic, and Dark
        { "Bug", new Dictionary<string, double> {

                { "Fire", 0.5 }, { "Fighting", 0.5 }, { "Poison", 0.5 }, { "Flying", 0.5 }, { "Ghost", 0.5 }, { "Fairy", 0.5 },
                { "Grass", 2 }, { "Psychic", 2 }, { "Dark", 2 }

            }
        },
            // WRITE COMMENTS HERE
        { "Rock", new Dictionary<string, double> {

                { "Fighting", 0.5 }, { "Ground", 0.5 }, { "Steel", 0.5 },
                { "Fire", 2 }, { "Ice", 2 }, { "Flying", 2 }, { "Bug", 2 }

            }
        },
            // Ghost is not effective against Dark
            // Ghost is super effective against Psychic, and Ghost
            // Ghost cannot hit normal
        { "Ghost", new Dictionary<string, double> {

                { "Dark", 0.5 },
                { "Psychic", 2 }, { "Ghost", 2 },
                { "Normal", 0 }

            }
        },
            // Dragon is not effective against Steel
            // Dragon is super effective against Dragon
            // Dragon cannot hit fairy
        { "Dragon", new Dictionary<string, double> {

                { "Steel", 0.5 },
                { "Dragon", 2 },
                { "Fairy", 0 }

            }
        },
            // Dark is not effective against Fighting, Dark, and Fairy
            // Dark is super effective against Psychic, and Ghost
        { "Dark", new Dictionary<string, double> {

                { "Fighting", 0.5 }, { "Dark", 0.5 }, { "Fairy", 0.5 },
                { "Psychic", 2 }, { "Ghost", 2 }

            }
        },
            // Steel is not effective against Fire, Water, Electric, and Steel
            // Steel is super effective against Ice, Rock, and Fairy
        { "Steel", new Dictionary<string, double> {

                { "Fire", 0.5 }, { "Water", 0.5 }, { "Electric", 0.5 }, { "Steel", 0.5 },
                { "Ice", 2 }, { "Rock", 2 }, { "Fairy", 2 }

            }
        },
            // Fairy is not effective against Fire, Poison, and Steel
            // Fairy is super effective against Fighting, Dragon, and Dark
        { "Fairy", new Dictionary<string, double> {

                { "Fire", 0.5 }, { "Poison", 0.5 }, { "Steel", 0.5 },
                { "Fighting", 2 }, { "Dragon", 2 }, { "Dark", 2 }

            }
        }
    };

        // Given string moveType, and string targetType returns a value represents the effectiveness of the move against the defeneder.
        public static double GetMonoTypeEffectiveness(string moveType, string targetType) {
            
            // Check that strings are valid
            if (string.IsNullOrEmpty(moveType) || string.IsNullOrEmpty(targetType))
                return -1;

            // Convert to Title Case or use ToLowerInvariant for consistency
            moveType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(moveType.ToLowerInvariant());
            targetType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(targetType.ToLowerInvariant());


            if (typeChart.ContainsKey(moveType) && typeChart[moveType].ContainsKey(targetType)) {
                return typeChart[moveType][targetType];
            }
            return 1; // Default to neutral (1x) if not found
        }

        // Given string moveType, and strings targetType and targetType2 returns a value represents the effectiveness of the move against the defeneder.
        public static double GetDualTypeEffectiveness(string moveType, string targetType, string targetType2) {

            // Check that strings are valid
            if (string.IsNullOrEmpty(moveType) || string.IsNullOrEmpty(targetType) || string.IsNullOrEmpty(targetType2))
                return -1;

            moveType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(moveType.ToLowerInvariant());
            targetType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(targetType.ToLowerInvariant());
            targetType2 = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(targetType2.ToLowerInvariant());


            double effectiveness = 1;
            if (typeChart.ContainsKey(moveType) && typeChart[moveType].ContainsKey(targetType)) {

                effectiveness *= typeChart[moveType][targetType];
            }
            if (typeChart.ContainsKey(moveType) && typeChart[moveType].ContainsKey(targetType2)) {

                effectiveness *= typeChart[moveType][targetType2];
            }
            return effectiveness; // Default to neutral (1x) if not found

            
        }
    }
}
