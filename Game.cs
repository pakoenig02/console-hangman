using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Game
    {
        private bool _gameOver;
        private bool _playerGuessedWord;

        private string _wordToGuess;
        private string _wordToGuessWithUnderscores;

        private int _guessErrorsMaximum;
        private int _playerGuessErrorCounter;

        private readonly List<char> _alreadyGuessedLetters;
        private readonly HangmanDrawer _hangmanDrawer;

        public Game()
        {
            // set default values
            this._gameOver = false;
            this._playerGuessedWord = false;

            this._wordToGuess = "";
            this._wordToGuessWithUnderscores = "";

            this._guessErrorsMaximum = 0;
            this._playerGuessErrorCounter = 0;

            this._alreadyGuessedLetters = new List<char>();
            this._hangmanDrawer = new HangmanDrawer();
        }

        public void Start()
        {
            Console.Clear();
            Welcome();
            SetGameSettingsFromPlayer();
            GuessLoop();
        }

        private static void Welcome()
        {
            Console.WriteLine("WELCOME TO HANGMAN!");
            HangmanDrawer.Draw();
        }

        private void SetGameSettingsFromPlayer()
        {
            this._wordToGuess = GetWordToGuessFromPlayer();
            this._wordToGuessWithUnderscores = ConvertWordToUnderscores(this._wordToGuess);
            this._guessErrorsMaximum = GetGuessesMaximumFromPlayer();
        }

        private static string GetWordToGuessFromPlayer()
        {
            bool wordToGuessIsValid = false;
            string wordToGuess = "";

            Console.Write("Write word to guess: ");

            while (!wordToGuessIsValid)
            {
                wordToGuess = Console.ReadLine();

                if (wordToGuess.All(Char.IsLetter))
                {
                    wordToGuessIsValid = true;
                }
                else
                {
                    Console.Write("Word is not allowed to have numbers and special chars, try again: ");
                    wordToGuessIsValid = false;
                }
            }

            return wordToGuess.ToUpper();
        }

        private static string ConvertWordToUnderscores(string word)
        {
            StringBuilder wordAsUnderscores = new StringBuilder();

            for (int index = 0; index < word.Length; index++)
            {
                wordAsUnderscores.Append("_");
            }

            return wordAsUnderscores.ToString();
        }

        private static int GetGuessesMaximumFromPlayer()
        {
            bool playerInputIsValid = false;
            int guessesMaximum = 0;

            Console.Write("Type amount of guesses (Standard 10): ");

            while (!playerInputIsValid)
            {
                if (!(int.TryParse(Console.ReadLine(), out guessesMaximum)))
                {
                    Console.WriteLine("Input needs to be a number, try again: ");
                    continue;
                }
                else if (guessesMaximum <= 0)
                {
                    Console.WriteLine("Input number needs to be possitiv, try again: ");
                    continue;
                }
                else if (guessesMaximum < 10)
                {
                    Console.WriteLine("Input number must be 10 or bigger, try again: ");
                    continue;
                }

                playerInputIsValid = true;
            }

            return guessesMaximum;
        }

        private void GuessLoop()
        {
            while (!this._gameOver)
            {
                Console.Clear();
                PrintGuessOrLetterHeadinfo();
                ExecuteGuessChoice(GetPlayerGuessChoice());
                CheckGameOverConditions();
                Console.Write("\n\n\nPRESS ANY KEY TO CONTINUE");
                Console.ReadKey();
            }
        }

        private void PrintGuessOrLetterHeadinfo()
        {
            this._hangmanDrawer.DrawInDependenceOfMaxAndCurrentCount(this._guessErrorsMaximum, this._playerGuessErrorCounter);
            PrintWordWithSpaces(this._wordToGuessWithUnderscores);
            Console.Write($"\n[{this._playerGuessErrorCounter + 1} / {this._guessErrorsMaximum}] Choose guess or letter (1 or 2):\n");
            Console.WriteLine("----------------------------------------");
        }

        private void ExecuteGuessChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    WordGuess();
                    break;
                case 2:
                    LetterGuess();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

        private static int GetPlayerGuessChoice()
        {
            bool playerInputIsValid = false;
            int guessOrLetter = 0;
            string tryAgain = " Try again.\n";

            while (!playerInputIsValid)
            {
                if (!(int.TryParse(Console.ReadLine(), out guessOrLetter)))
                {
                    Console.Write("Input needs to be a digit!" + tryAgain);
                    continue;
                }
                else if (!(guessOrLetter == 1 || guessOrLetter == 2))
                {
                    Console.Write("Input must be 1 or 2!" + tryAgain);
                    continue;
                }

                playerInputIsValid = true;
            }

            return guessOrLetter;
        }

        private void WordGuess()
        {
            Console.Write("\nInput your guess: ");
            string playerWordGuess = Console.ReadLine().ToUpper();

            if (playerWordGuess == this._wordToGuess)
            {
                _playerGuessedWord = true;
            }
            else
            {
                Console.WriteLine("You guessed wrong...");
                _playerGuessErrorCounter++;
            }
        }

        private void LetterGuess()
        {
            char playerLetterGuess;

            Console.Write("\nInput your letter: ");
            PrintAlreadyGuessedLetters();
            playerLetterGuess = GetPlayerLetterGuess();

            if (CheckIfWordContainsLetterGuess(playerLetterGuess))
            {
                Console.WriteLine("Word contains your letter!\n");
                UpdateWordToGuessWithUnderscoresWithNewLetter(playerLetterGuess);
            }
            else
            {
                Console.WriteLine("Word does not contain letter...\n");
                _playerGuessErrorCounter++;
            }
        }

        private void PrintAlreadyGuessedLetters()
        {
            if (_alreadyGuessedLetters.Count != 0)
            {
                Console.WriteLine("\nAlready Guessed Letters(" +
                    string.Join(",", _alreadyGuessedLetters.ToArray())
                    + ")"
                );
            }
        }

        private char GetPlayerLetterGuess()
        {
            bool playerInputIsValid = false;
            char playerLetterGuess = ' ';
            string tryAgain = " Try again.\n";

            while (!playerInputIsValid)
            {
                if (!(Char.TryParse(Console.ReadLine().ToUpper(), out playerLetterGuess)))
                {
                    Console.Write("Input needs to be a character!" + tryAgain);
                    continue;
                }
                else if (!Char.IsLetter(playerLetterGuess))
                {
                    Console.Write("Input needs to be a letter!" + tryAgain);
                    continue;
                }
                else if (CheckIfLetterWasAlreadyGuessed(playerLetterGuess))
                {
                    Console.WriteLine("This letter was already guessed!" + tryAgain);
                    continue;
                }

                playerInputIsValid = true;
            }

            return playerLetterGuess;
        }

        private bool CheckIfLetterWasAlreadyGuessed(char playerLetterGuess)
        {
            bool letterAlreadyGuessed = false;

            foreach (char alreadyGuessedletter in this._alreadyGuessedLetters)
            {
                if (playerLetterGuess == alreadyGuessedletter)
                {
                    letterAlreadyGuessed = true;
                }
            }

            if (letterAlreadyGuessed == false)
            {
                this._alreadyGuessedLetters.Add(playerLetterGuess);
            }

            return letterAlreadyGuessed;
        }

        private bool CheckIfWordContainsLetterGuess(char playerLetterGuess)
        {
            bool letterInString = false;

            for (int index = 0; index < this._wordToGuess.Length; index++)
            {
                if (this._wordToGuess[index] == playerLetterGuess)
                {
                    letterInString = true;
                    break;
                }
            }

            return letterInString;
        }

        private void UpdateWordToGuessWithUnderscoresWithNewLetter(char playerLetterGuess)
        {
            for (int index = 0; index < this._wordToGuess.Length; index++)
            {
                if (this._wordToGuess[index] == playerLetterGuess)
                {
                    StringBuilder sb = new StringBuilder(this._wordToGuessWithUnderscores);
                    sb[index] = playerLetterGuess;
                    this._wordToGuessWithUnderscores = sb.ToString();
                }
            }
        }

        private void CheckGameOverConditions()
        {
            // player has no guesses left
            if (this._playerGuessErrorCounter == this._guessErrorsMaximum)
            {
                Console.WriteLine("You LOSE word was: " + this._wordToGuess);
                this._gameOver = true;
            }
            // player guessed the word
            else if (this._playerGuessedWord)
            {
                PrintWinMessage();
                this._gameOver = true;
            }
            // player found out all letters
            else if (this._wordToGuess == this._wordToGuessWithUnderscores)
            {
                PrintWinMessage();
                this._gameOver = true;
            }
        }

        private void PrintWinMessage()
        {
            Console.WriteLine("You guessed right! EASY WIN!\n");
            Console.WriteLine("Word was: " + this._wordToGuess);
        }

        private static void PrintWordWithSpaces(string word, int amountSpaces = 1)
        {
            string letterSpace = new String(' ', amountSpaces);
            StringBuilder WordWithSpaces = new StringBuilder();

            for (int index = 0; index < word.Length; index++)
            {
                string letter = word[index].ToString();
                WordWithSpaces.Append(letter).Append(letterSpace);
            }

            Console.WriteLine(WordWithSpaces.ToString().Trim());
        }
    }
}
