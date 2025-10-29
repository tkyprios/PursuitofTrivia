using Pursuit_of_Trivia.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.NetworkInformation;

namespace Pursuit_of_Trivia
{
    /// <summary>
    /// Class representing the overall game state, including setting up players (and bots) and initializing the game table with the master question deck.
    /// Implements the "how" (turn logic, game flow, win conditions)
    /// </summary>
    internal class Game
    {
        private Table _gameTable;
        private readonly IGameUI _gameUI;
        private readonly IInputHandler _inputHandler;
        private TurnManager _turnManager;

        /// <summary>
        /// Constructor for the Game class, initializes the game table with a master deck of questions covering all categories.
        /// </summary>
        public Game() 
        {
            _gameTable = new Table(new Deck(GetAllQuestionCategories()));
            _inputHandler = new ConsoleInputHandler();
            _gameUI = new ConsoleGameUI(_gameTable, _inputHandler);
            _turnManager = new TurnManager(_gameTable, _gameUI, _inputHandler);
        }

        /// <summary>
        /// Retrieves a list of all available question categories.
        /// </summary>
        /// <remarks>The method returns all values of the <see cref="Category"/> enumeration as a list. 
        /// <returns>A list of all categories defined in the <see cref="Category"/> enumeration.</returns>
        private List<Category> GetAllQuestionCategories() => 
            Enum.GetValues(typeof(Category)).Cast<Category>().ToList();

        /// <summary>
        /// Sets up the players for the game by prompting the user for their username and initializing the current
        /// player and a bot player.
        /// </summary>
        private void SetupPlayers()
        {
            _gameUI.Clear();

            _gameUI.WriteLine("Let's get you set up!");
            _gameUI.WriteLine("What would you like your username to be?");

            string playerUsername;
            do
            {
                playerUsername = Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(playerUsername));

            _gameUI.WriteLine();

            var player = new Player(playerUsername);
            _gameUI.WriteLine($"Welcome, {player.Name}!");
            
            _gameTable.AddPlayerToGame(player);
            _gameTable.AddBotToGame();
        }

        /// <summary>
        /// Starts the game loop, managing player setup, turn processing, and game state transitions.
        /// </summary>
        /// <remarks>This method initializes the game, processes turns until a winner is determined, and
        /// prompts the user to decide whether to play again. If the user chooses to play again, the game state is
        /// reset; otherwise, the game exits.</remarks>
        public void StartGame()
        {
            bool playAgain;
            do
            {
                SetupPlayers();
                bool gameRunning = true;

                while (gameRunning)
                {
                    var turnResult = _turnManager.ProcessTurn();
                    switch (turnResult)
                    {
                        case TurnManager.TurnResult.Exit:
                            ExitGame();
                            return;
                        case TurnManager.TurnResult.GameWon:
                            gameRunning = false; // exit loop
                            break;
                        case TurnManager.TurnResult.Continue:
                            break; // continue game
                    }
                }

                // Flow will only occur when there is a winner and game is ended.
                var winner = _gameTable.GetCurrentPlayer();
                _gameUI.DisplayWinMessage(winner);

                playAgain = _gameUI.PromptPlayAgain();

                if (playAgain)
                {
                    ResetGame();
                }
                else
                {
                    ExitGame();
                }
            } while (playAgain);

        }

        /// <summary>
        /// Resets the game to its initial state, preparing a new game session.
        /// </summary>
        /// <remarks>This method initializes a new game table with a fresh deck of questions and resets
        /// the turn manager. It should be called to start a new game or restart after a game session ends.</remarks>
        private void ResetGame()
        {
            _gameTable = new Table(new Deck(GetAllQuestionCategories()));
            _turnManager = new TurnManager(_gameTable, _gameUI, _inputHandler);
        }


        /// <summary>
        /// Exits the game and displays a farewell message to the user.
        /// </summary>
        /// <remarks>Clears the console, displays a thank-you message, and prompts the user to press any
        /// key before exiting.</remarks>
        private void ExitGame()
        {
            _gameUI.DisplayExitMessage();
            Environment.Exit(0);
        }
    }
}
