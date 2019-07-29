using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    sealed class HangmanDrawer
    {
        private int _currentStage;
        private readonly static string[] _stages = {
            "               \n" +
            "               \n" +
            "               \n" +
            "               \n" +
            "               \n" +
            "               \n" +
            "                 ",

            "               \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|         |    \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|         |    \n" +
            "|         0    \n" +
            "|              \n" +
            "|              \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|         |    \n" +
            "|         0    \n" +
            "|         |    \n" +
            "|              \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|         |    \n" +
            "|         0    \n" +
            "|        /|    \n" +
            "|              \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|         |    \n" +
            "|         0    \n" +
            "|        /|\\  \n" +
            "|              \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|         |    \n" +
            "|         0    \n" +
            "|        /|\\  \n" +
            "|        /     \n" +
            "|              \n" +
            "|                ",

            " _________     \n" +
            "|         |    \n" +
            "|         0    \n" +
            "|        /|\\  \n" +
            "|        / \\  \n" +
            "|              \n" +
            "|                "};

        public HangmanDrawer()
        {
            this._currentStage = 0;
        }

        public void DrawInDependenceOfMaxAndCurrentCount(int maxCount, int currentCount)
        {
            int amountStages = _stages.Length;
            int stepsToNextStage = Convert.ToInt32(Math.Floor(Convert.ToDouble(maxCount / amountStages)));

            if (currentCount == 0)
            {
                this._currentStage = 0;
            }
            else if (currentCount == maxCount)
            {
                this._currentStage = amountStages;
            }
            else if (currentCount % stepsToNextStage == 0)
            {
                this._currentStage = (currentCount / stepsToNextStage);
            }

            Draw(this._currentStage);
        }

        public static void Draw(int stageNumber = 9)
        {
            for (int index = 0; index < _stages.Length; index++)
            {
                if (index == stageNumber)
                {
                    Console.WriteLine(_stages[index]);
                    break;
                }
            }
        }
    }
}
