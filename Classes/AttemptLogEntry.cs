using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeHelper.Classes
{
    public class AttemptLogEntry
    {
        // The move's type used in the quiz
        public string AttackingType { get; set; }

        // The two defending Pokémon types
        public string DefenderType1 { get; set; }
        public string DefenderType2 { get; set; }

        // Whether the user's answer was correct
        public bool WasCorrect { get; set; }

        // Timestamp is optional but useful for later analysis or display
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

}
