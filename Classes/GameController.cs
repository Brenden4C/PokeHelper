using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeHelper.Classes
{
    public class GameController
    {
        private readonly StatisticsManager _statisticsManager;

        public GameController()
        {
            _statisticsManager = new StatisticsManager();
        }

        // This method is called whenever the player answers a question
        public void LogAttempt(bool isCorrect, string attackType, string defendType, string defendType2)
        {
            _statisticsManager.LogAttempt(attackType, defendType, defendType2, isCorrect);
        }
    }

}
