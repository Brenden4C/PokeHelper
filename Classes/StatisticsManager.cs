using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeHelper.Classes
{
    public class StatisticsManager
    {
        private const string StatsFileName = "attempts.json";
        public List<AttemptLogEntry> Attempts { get; private set; } = new List<AttemptLogEntry>();

        // Add a new attempt to the list
        public void LogAttempt(string attackingType, string defender1, string defender2, bool wasCorrect)
        {
            Attempts.Add(new AttemptLogEntry
            {
                AttackingType = attackingType,
                DefenderType1 = defender1,
                DefenderType2 = defender2,
                WasCorrect = wasCorrect
            });
        }

        // Save to file (JSON)
        public void Save()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StatsFileName);
            File.WriteAllText(path, JsonSerializer.Serialize(Attempts));
        }

        // Load from file
        public void Load()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StatsFileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Attempts = JsonSerializer.Deserialize<List<AttemptLogEntry>>(json) ?? new List<AttemptLogEntry>();
            }
        }
    }

}
